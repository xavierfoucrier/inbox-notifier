using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using notifier.Languages;
using notifier.Properties;

namespace notifier {
	class Gmail {

		/// <summary>
		/// Reference to the gmail inbox
		/// </summary>
		internal Inbox Inbox;

		/// <summary>
		/// User credential for the gmail authentication
		/// </summary>
		private UserCredential Credential;

		/// <summary>
		/// Reference to the main interface
		/// </summary>
		private Main UI;

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="form">Reference to the application main window</param>
		public Gmail(ref Main form) {
			UI = form;
		}

		/// <summary>
		/// Asynchronous method used to get user credential
		/// </summary>
		public async void Authentication() {
			try {

				// waits for the user authorization
				Credential = await AuthorizationBroker();

				// instanciates a new inbox with the credential
				Inbox = new Inbox(ref UI, ref Credential);

				// displays the user email address
				UI.labelEmailAddress.Text = Inbox.GetEmailAddress();
				UI.labelTokenDelivery.Text = Credential.Token.IssuedUtc.ToLocalTime().ToString();
			} catch (Exception) {

				// exits the application if the google api token file doesn't exists
				if (!Directory.Exists(Core.GetApplicationDataFolder()) || !Directory.EnumerateFiles(Core.GetApplicationDataFolder()).Any()) {

					// displays the authentication icon and title
					UI.notifyIcon.Icon = Resources.authentication;
					UI.notifyIcon.Text = Translation.authenticationFailed;

					// exits the application
					MessageBox.Show(Translation.authenticationWithGmailRefused, Translation.authenticationFailed, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					Application.Exit();
				}
			} finally {

				// synchronizes the user mailbox, after checking for update depending on the user settings, or by default after the asynchronous authentication
				if (Settings.Default.UpdateService && UI.UpdateService.IsPeriodSetToStartup()) {
					UI.UpdateService.Check(!Settings.Default.UpdateDownload, true);
				} else {
					UI.GmailService.Inbox.Sync();
				}
			}
		}

		/// <summary>
		/// Asynchronous method used to refresh the authentication token
		/// </summary>
		/// <returns>Indicates if the token has been properly refreshed</returns>
		public async Task<bool> RefreshToken() {

			// refreshes the token and updates the token delivery date and time on the interface
			if (await Credential.RefreshTokenAsync(new CancellationToken())) {
				UI.labelTokenDelivery.Text = Credential.Token.IssuedUtc.ToLocalTime().ToString();
			}

			return true;
		}

		/// <summary>
		/// Disposes the service
		/// </summary>
		public void Dispose() {
			Inbox.Dispose();
		}

		/// <summary>
		/// Asynchronous task used to get the user authorization
		/// </summary>
		/// <returns>OAuth 2.0 user credential</returns>
		private async Task<UserCredential> AuthorizationBroker() {

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
					new FileDataStore(Core.GetApplicationDataFolder(), true)
				);

				// returns the user credential
				return credential;
			}
		}
	}
}
