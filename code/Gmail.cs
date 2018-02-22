using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using notifier.Languages;
using notifier.Properties;
using static notifier.Update;

namespace notifier {
	class Gmail : Main {

		// gmail api service
		public GmailService service;

		// user credential for the gmail authentication
		private UserCredential credential;

		/// <summary>
		/// Class constructor
		/// </summary>
		public Gmail() {
		}

		/// <summary>
		/// Asynchronous method used to get user credential
		/// </summary>
		public async void AsyncAuthentication() {
			try {

				// waits for the user authorization
				credential = await AsyncAuthorizationBroker();

				// creates the gmail api service
				this.service = new GmailService(new BaseClientService.Initializer() {
					HttpClientInitializer = credential,
					ApplicationName = "Gmail notifier for Windows"
				});

				// displays the user email address
				labelEmailAddress.Text = this.service.Users.GetProfile("me").Execute().EmailAddress;
				labelTokenDelivery.Text = credential.Token.IssuedUtc.ToLocalTime().ToString();
			} catch (Exception) {

				// exits the application if the google api token file doesn't exists
				if (!Directory.Exists(GetAppData()) || !Directory.EnumerateFiles(GetAppData()).Any()) {

					// displays the authentication icon and title
					notifyIcon.Icon = Resources.authentication;
					notifyIcon.Text = translation.authenticationFailed;

					// exits the application
					MessageBox.Show(translation.authenticationWithGmailRefused, translation.authenticationFailed, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					Application.Exit();
				}
			} finally {

				// synchronizes the user mailbox, after checking for update depending on the user settings, or by default after the asynchronous authentication
				if (Settings.Default.UpdateService && Settings.Default.UpdatePeriod == (int)Period.Startup) {
					Update UpdateService = new Update();
					UpdateService.AsyncCheckForUpdate(!Settings.Default.UpdateDownload, true);
				} else {
					this.AsyncSyncInbox();
				}
			}
		}

		/// <summary>
		/// Asynchronous task used to get the user authorization
		/// </summary>
		/// <returns>OAuth 2.0 user credential</returns>
		public async Task<UserCredential> AsyncAuthorizationBroker() {

			// uses the client secret file for the context
			using (FileStream stream = new FileStream(Path.GetDirectoryName(Application.ExecutablePath) + "/client_secret.json", FileMode.Open, FileAccess.Read)) {

				// defines a cancellation token source
				CancellationTokenSource cancellation = new CancellationTokenSource();
				cancellation.CancelAfter(TimeSpan.FromSeconds(20));

				// waits for the user validation, only if the user has not already authorized the application
				UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					new string[] { GmailService.Scope.GmailModify },
					"user",
					cancellation.Token,
					new FileDataStore(GetAppData(), true)
				);

				// returns the user credential
				return credential;
			}
		}

		/// <summary>
		/// Asynchronous method used to refresh the authentication token
		/// </summary>
		/// <returns></returns>
		public async Task<bool> AsyncRefreshToken() {

			// refreshes the token and updates the token delivery date and time on the interface
			if (await credential.RefreshTokenAsync(new CancellationToken())) {
				labelTokenDelivery.Text = credential.Token.IssuedUtc.ToLocalTime().ToString();
			}

			return true;
		}


		//
		new public void Dispose() {
			if (service != null) {
				service.Dispose();
			}
		}
	}
}
