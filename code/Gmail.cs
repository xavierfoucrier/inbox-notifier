using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
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
		/// Gmail base client service
		/// </summary>
		private GmailService Service;

		/// <summary>
		/// User credential for the gmail authentication
		/// </summary>
		private UserCredential Credential;

		/// <summary>
		/// Reference to the main interface
		/// </summary>
		private Main UI;

		/// <summary>
		/// Client identifier
		/// </summary>
		private const string ClientId = "723757215783-caqv5p17r04tdf1puar3n7u91mc7h2nq.apps.googleusercontent.com";

		/// <summary>
		/// Client secret
		/// </summary>
		private const string ClientSecret = "OkONKl4BpLud7uJpk7YDT_fC";

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
			if (!OAuth2TokenResponse) {
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
				Core.Log($"Authentication: {exception.Message}");

				// display the authentication failure icon and text
				UI.notifyIcon.Icon = Resources.warning;
				UI.notifyIcon.Text = Translation.authenticationFailed;

				// define a user input result
				DialogResult input;

				// display a message to the user depending on the exception
				switch (exception.GetType().Name) {
					default:
						input = MessageBox.Show(Translation.authenticationRequestError, Translation.authenticationFailed, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
						break;
					case "OperationCanceledException":
						input = MessageBox.Show(Translation.authenticationCanceled.Replace("{timeout}", Settings.Default.OAUTH_TIMEOUT.ToString()), Translation.authenticationFailed, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
						break;
					case "TokenResponseException":
						switch (((TokenResponseException)exception).Error.Error) {
							default:
								input = MessageBox.Show(Translation.authenticationRequestError, Translation.authenticationFailed, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
								break;
							case "access_denied":
								input = MessageBox.Show(Translation.authenticationDenied, Translation.authenticationFailed, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
								break;
							case "invalid_client":
								input = MessageBox.Show(Translation.authenticationRevoked, Translation.authenticationFailed, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								break;
						}

						break;
				}

				// restart or exit the application depending on the user input
				if (input == DialogResult.Retry) {
					Core.RestartApplication();
				} else if (input == DialogResult.OK) {
					Process.Start(Settings.Default.GITHUB_REPOSITORY);
				} else {
					Application.Exit();
				}

				return;
			}

			// enable the main timer
			UI.timer.Enabled = true;

			// check for update depending on the user settings
			if (Settings.Default.UpdateService && Update.IsPeriodSetToStartup()) {
				await UI.UpdateService.Check(!Settings.Default.UpdateDownload, true);
			}

			// synchronize the user mailbox
			await Inbox.Sync();
		}

		/// <summary>
		/// Asynchronous method used to refresh the authentication token
		/// </summary>
		/// <returns>Indicate if the token has been properly refreshed</returns>
		public async Task<bool> RefreshToken() {

			// refresh the token and update the token delivery date and time on the interface
			try {
				if (Credential.Token.IsStale) {
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
		/// Asynchronous method used to connect the gmail base client service
		/// </summary>
		/// <returns>Users resource</returns>
		public async Task<UsersResource> Connect() {
			Service = new GmailService(new BaseClientService.Initializer {
				HttpClientInitializer = UI.GmailService.Credential,
				ApplicationName = Settings.Default.APPLICATION_NAME
			});

			// retrieve the gmail address and store it in an application cache setting
			if (Settings.Default.EmailAddress == "-") {
				Profile UserProfile = await Service.Users.GetProfile("me").ExecuteAsync();
				Settings.Default.EmailAddress = UserProfile.EmailAddress;
			}

			return Service.Users;
		}

		/// <summary>
		/// Dispose the service
		/// </summary>
		public void Dispose() {
			Service?.Dispose();
		}

		/// <summary>
		/// Asynchronous task used to get the user authorization
		/// </summary>
		/// <returns>OAuth 2.0 user credential</returns>
		private static async Task<UserCredential> AuthorizationBroker() {

			// define a cancellation token source
			using (CancellationTokenSource cancellation = new CancellationTokenSource()) {
				cancellation.CancelAfter(TimeSpan.FromSeconds(Settings.Default.OAUTH_TIMEOUT));

				// use pkce code flow
				PkceGoogleAuthorizationCodeFlow flow = new PkceGoogleAuthorizationCodeFlow(
					new GoogleAuthorizationCodeFlow.Initializer {
						ClientSecrets = new ClientSecrets {
							ClientId = ClientId,
							ClientSecret = ClientSecret,
						},
						Scopes = new[] {
							GmailService.Scope.GmailModify
						},
						DataStore = new FileDataStore(Core.ApplicationDataFolder, true),
					}
				);

				// build authorization url
				AuthorizationCodeInstalledApp authCode = new AuthorizationCodeInstalledApp(flow, new LocalServerCodeReceiver(Resources.oauth_message));

				// wait for the user validation, only if the user has not already authorized the application
				UserCredential credential = await authCode.AuthorizeAsync("user", cancellation.Token);

				// return the user credential
				return credential;
			}
		}

		#endregion

		#region #accessors

		/// <summary>
		/// Flag defining if the OAuth2 token response file is present in the application data folder
		/// </summary>
		public bool OAuth2TokenResponse {
			get;
		} = Directory.Exists(Core.ApplicationDataFolder) && Directory.EnumerateFiles(Core.ApplicationDataFolder, "*.TokenResponse-user").Any();

		#endregion
	}
}