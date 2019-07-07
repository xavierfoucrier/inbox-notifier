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
	class Gmail : IDisposable {

		#region #attributes

		/// <summary>
		/// Reference to the gmail inbox
		/// </summary>
		internal Inbox Inbox;

		/// <summary>
		/// Reference to the main interface
		/// </summary>
		private Main UI;

		#endregion

		#region #methods

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
		public async Task Authentication() {

			// display the authentication icon and text if the google api token file doesn't exists
			if (!Directory.Exists(Core.ApplicationDataFolder) || !Directory.EnumerateFiles(Core.ApplicationDataFolder).Any()) {
				UI.notifyIcon.Icon = Resources.authentication;
				UI.notifyIcon.Text = Translation.authenticationNeeded;
			}

			try {

				// wait for the user authorization
				Credential = await AuthorizationBroker().ConfigureAwait(false);

				// instanciate a new inbox
				Inbox = new Inbox(ref UI);

				// get the token delivery time
				UI.labelTokenDelivery.Text = Credential.Token.IssuedUtc.ToLocalTime().ToString();
			} catch (Exception exception) {

				// log the error
				Core.Log("Authentication: " + exception.Message);

				// exit the application if the google api token file doesn't exists
				if (!Directory.Exists(Core.ApplicationDataFolder) || !Directory.EnumerateFiles(Core.ApplicationDataFolder).Any()) {

					// display the authentication failure icon and text
					UI.notifyIcon.Icon = Resources.warning;
					UI.notifyIcon.Text = Translation.authenticationFailed;

					// exit the application
					MessageBox.Show(Translation.authenticationWithGmailRefused, Translation.authenticationFailed, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					Application.Exit();
					return;
				}
			}

			// synchronize the user mailbox, after checking for update depending on the user settings, or by default after the asynchronous authentication
			if (Settings.Default.UpdateService && Update.IsPeriodSetToStartup()) {
				await UI.UpdateService.Check(!Settings.Default.UpdateDownload, true);
			} else {
				Inbox.Sync();
			}
		}

		/// <summary>
		/// Asynchronous method used to refresh the authentication token
		/// </summary>
		/// <returns>Indicate if the token has been properly refreshed</returns>
		public async Task<bool> RefreshToken() {

			// refresh the token and update the token delivery date and time on the interface
			try {
				if (Credential.Token.IsExpired(Credential.Flow.Clock)) {
					if (await Credential.RefreshTokenAsync(new CancellationToken())) {
						UI.labelTokenDelivery.Text = Credential.Token.IssuedUtc.ToLocalTime().ToString();
					}
				}
			} catch (IOException) {
				// nothing to catch: IOException from mscorlib
				// sometimes the process can not access the token response file because it is used by another process
			}

			return true;
		}

		/// <summary>
		/// Dispose the service
		/// </summary>
		public void Dispose() {
			if (Inbox != null) {
				Inbox.Dispose();
			}
		}

		/// <summary>
		/// Asynchronous task used to get the user authorization
		/// </summary>
		/// <returns>OAuth 2.0 user credential</returns>
		private static async Task<UserCredential> AuthorizationBroker() {

			// use the client secret file for the context
			try {
				using (FileStream stream = new FileStream(Path.GetDirectoryName(Application.ExecutablePath) + "/client_secret.json", FileMode.Open, FileAccess.Read)) {

					// define a cancellation token source
					CancellationTokenSource cancellation = new CancellationTokenSource();
					cancellation.CancelAfter(TimeSpan.FromSeconds(Settings.Default.AUTH_TIMEOUT));

					// wait for the user validation, only if the user has not already authorized the application
					UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
						GoogleClientSecrets.Load(stream).Secrets,
						new string[] { GmailService.Scope.GmailModify },
						"user",
						cancellation.Token,
						new FileDataStore(Core.ApplicationDataFolder, true),
						new LocalServerCodeReceiver(Resources.oauth_message)
					);

					// return the user credential
					return credential;
				}
			} catch (Exception exception) {
				Core.Log("AuthorizationBroker: " + exception.Message);
				return null;
			}
		}

		#endregion

		#region #accessors

		/// <summary>
		/// User credential for the gmail authentication
		/// </summary>
		public UserCredential Credential {
			get; set;
		}

		#endregion
	}
}