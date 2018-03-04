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

		// reference to the gmail inbox
		internal Inbox Inbox;

		// user credential for the gmail authentication
		private UserCredential credential;

		// reference to the main interface
		private Main Interface;

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="Form">Reference to the application main window</param>
		public Gmail(ref Main Form) {
			Interface = Form;
		}

		/// <summary>
		/// Asynchronous method used to get user credential
		/// </summary>
		public async void Authentication() {
			try {

				// waits for the user authorization
				credential = await AuthorizationBroker();

				// instanciates a new inbox with the credential
				Inbox = new Inbox(ref Interface, ref credential);

				// displays the user email address
				Interface.labelEmailAddress.Text = Inbox.GetEmailAddress();
				Interface.labelTokenDelivery.Text = credential.Token.IssuedUtc.ToLocalTime().ToString();
			} catch (Exception) {

				// exits the application if the google api token file doesn't exists
				if (!Directory.Exists(Core.GetApplicationDataFolder()) || !Directory.EnumerateFiles(Core.GetApplicationDataFolder()).Any()) {

					// displays the authentication icon and title
					Interface.notifyIcon.Icon = Resources.authentication;
					Interface.notifyIcon.Text = translation.authenticationFailed;

					// exits the application
					MessageBox.Show(translation.authenticationWithGmailRefused, translation.authenticationFailed, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					Application.Exit();
				}
			} finally {
				Interface.AuthenticationCallback();
			}
		}

		/// <summary>
		/// Asynchronous method used to refresh the authentication token
		/// </summary>
		public async Task<bool> RefreshToken() {

			// refreshes the token and updates the token delivery date and time on the interface
			if (await credential.RefreshTokenAsync(new CancellationToken())) {
				Interface.labelTokenDelivery.Text = credential.Token.IssuedUtc.ToLocalTime().ToString();
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
