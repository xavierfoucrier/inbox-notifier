using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace notifier {
	public partial class Main : Form {

		// gmail api service
		private GmailService service;

		// user credential for the gmail authentication
		private UserCredential credential;

		// inbox label
		private Google.Apis.Gmail.v1.Data.Label inbox;

		/// <summary>
		/// Initializes the class
		/// </summary>
		public Main() {
			InitializeComponent();
		}

		/// <summary>
		/// Loads the form
		/// </summary>
		private void Main_Load(object sender, EventArgs e) {

			// authenticates the user
			AsyncAuthentication();

			// synchronizes the user mailbox
			SyncInbox();

			// attaches the context menu to the systray icon
			notifyIcon.ContextMenu = contextMenu;

			// displays the product version
			string[] version = Application.ProductVersion.Split('.');
			labelVersion.Text = version[0] + "." + version[1] + "-" + (version[2] == "0" ? "alpha" : version[2] == "1" ? "beta" : version[2] == "2" ? "rc" : version[2] == "3" ? "release" : "") + (version[3] != "0" ? " " + version[3] : "");
		}

		/// <summary>
		/// Prompts the user before closing the form
		/// </summary>
		private void Main_FormClosing(object sender, FormClosingEventArgs e) {

			// asks the user for exit, depending on the application settings
			if (e.CloseReason != CloseReason.ApplicationExitCall && Properties.Settings.Default.AskonExit) {
				DialogResult dialog = MessageBox.Show("Vous êtes sur le point de quitter l'application.\n\nVoulez-vous vraiment quitter l'application ?", "Fermeture de l'application", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

				if (dialog == DialogResult.No) {
					e.Cancel = true;
				}
			}
		}

		/// <summary>
		/// Asynchronous method used to get user credential
		/// </summary>
		private async void AsyncAuthentication() {
			try {
				
				// waits for the user authorization
				this.credential = await AsyncAuthorizationBroker();

				// creates the gmail api service
				this.service = new GmailService(new BaseClientService.Initializer() {
					HttpClientInitializer = this.credential,
					ApplicationName = "Gmail notifier for Windows"
				});
			} catch(Exception) {
				MessageBox.Show("Vous avez refusé que l'application accède à votre compte Gmail. Cette étape est nécessaire et vous sera demandée à nouveau lors du prochain démarrage.\n\nL'application va désormais quitter.", "Erreur d'authentification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				Application.Exit();
			}
		}

		/// <summary>
		/// Asynchronous task used to get the user authorization
		/// </summary>
		/// <returns>OAuth 2.0 user credential</returns>
		private async Task<UserCredential> AsyncAuthorizationBroker() {

			// uses the client secret file for the context
			using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) {

				// waits for the user validation, only if the user has not already authorized the application
				return await GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					new string[] { GmailService.Scope.GmailReadonly },
					"user",
					CancellationToken.None,
					new FileDataStore(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Gmail Notifier", true)
				);
			}
		}

		/// <summary>
		/// Synchronizes the user inbox
		/// </summary>
		private void SyncInbox() {
			try {

				// gets the "inbox" label
				this.inbox = service.Users.Labels.Get("me", "INBOX").Execute();

				// manages unread threads
				if (this.inbox.ThreadsUnread > 0) {

					// plays a sound on unread threads
					if (Properties.Settings.Default.AudioNotification) {
						SystemSounds.Exclamation.Play();
					}

					// displays a balloon tip in the systray with the total of unread threads
					notifyIcon.ShowBalloonTip(450, this.inbox.ThreadsUnread.ToString() + " " + (this.inbox.ThreadsUnread > 1 ? "emails non lus" : "email non lu"), "Double-cliquez sur l'icône pour accéder à votre boîte de réception.", ToolTipIcon.Info);
				}

				// restores the default icon to the systray
				notifyIcon.Icon = Properties.Resources.normal;
			} catch(Exception exception) {
				MessageBox.Show(exception.Message);
			}
		}

		/// <summary>
		/// Manages the AskonExit user setting
		/// </summary>
		private void fieldAskonExit_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.AskonExit = fieldAskonExit.Checked;
			Properties.Settings.Default.Save();
			labelSettingsSaved.Visible = true;
		}

		/// <summary>
		/// Manages the AudioNotification user setting
		/// </summary>
		private void fieldAudioNotification_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.AudioNotification = fieldAudioNotification.Checked;
			Properties.Settings.Default.Save();
			labelSettingsSaved.Visible = true;
		}

		/// <summary>
		/// Closes the preferences when the OK button is clicked
		/// </summary>
		private void buttonOK_Click(object sender, EventArgs e) {
			this.WindowState = FormWindowState.Minimized;
			this.ShowInTaskbar = false;
		}

		/// <summary>
		/// Manages the context menu Settings item
		/// </summary>
		private void menuItemSettings_Click(object sender, EventArgs e) {
			this.ShowInTaskbar = true;
			this.WindowState = FormWindowState.Normal;
		}

		/// <summary>
		/// Manages the context menu exit item
		/// </summary>
		private void menuItemExit_Click(object sender, EventArgs e) {
			this.Close();
		}

		/// <summary>
		/// Opens the gmail inbox in a browser when you double click on the systray icon
		/// </summary>
		private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
			Process.Start("https://mail.google.com/mail/u/0/#inbox");
		}
	}
}
