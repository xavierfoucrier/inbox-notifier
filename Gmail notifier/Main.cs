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
using Google.Apis.Gmail.v1.Data;
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

		// unread threads
		private int? unreadthreads = 0;

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
			this.AsyncAuthentication();

			// synchronizes the user mailbox
			this.SyncInbox();

			// attaches the context menu to the systray icon
			notifyIcon.ContextMenu = contextMenu;

			// binds the "PropertyChanged" event of the settings to automatically save the user settings and display the setting label
			Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler((object o, PropertyChangedEventArgs target) => {
				Properties.Settings.Default.Save();
				labelSettingsSaved.Visible = true;
			});

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
					new string[] { GmailService.Scope.GmailModify },
					"user",
					CancellationToken.None,
					new FileDataStore(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Gmail Notifier", true)
				);
			}
		}

		/// <summary>
		/// Synchronizes the user inbox
		/// </summary>
		private void SyncInbox(bool timertick = false) {
			try {

				// displays the sync icon, but not on every tick of the timer
				if (!timertick) {
					notifyIcon.Icon = Properties.Resources.sync;
					notifyIcon.Text = "Synchronisation en cours ...";
				}

				// gets the "inbox" label
				this.inbox = this.service.Users.Labels.Get("me", "INBOX").Execute();

				// exits the sync if the number of unread threads is the same as before
				if (timertick && (this.inbox.ThreadsUnread == this.unreadthreads)) {
					return;
				}

				// manages unread threads
				if (this.inbox.ThreadsUnread > 0) {

					// sets the notification icon and text
					notifyIcon.Icon = this.inbox.ThreadsUnread <= 9 ? (Icon)Properties.Resources.ResourceManager.GetObject("mail_" + this.inbox.ThreadsUnread.ToString()) : Properties.Resources.mail_more;

					// plays a sound on unread threads
					if (Properties.Settings.Default.AudioNotification) {
						SystemSounds.Exclamation.Play();
					}

					// displays a balloon tip in the systray with the total of unread threads
					notifyIcon.ShowBalloonTip(450, this.inbox.ThreadsUnread.ToString() + " " + (this.inbox.ThreadsUnread > 1 ? "emails non lus" : "email non lu"), "Double-cliquez sur l'icône pour accéder à votre boîte de réception.", ToolTipIcon.Info);
					notifyIcon.Text = this.inbox.ThreadsUnread.ToString() + " " + (this.inbox.ThreadsUnread > 1 ? "emails non lus" : "email non lu");

					// enables the mark as read menu item
					menuItemMarkAsRead.Enabled = true;
				} else {

					// restores the default systray icon and text
					notifyIcon.Icon = Properties.Resources.normal;
					notifyIcon.Text = "Pas de nouveau message";
				}

				// saves the number of unread threads
				this.unreadthreads = this.inbox.ThreadsUnread;
			} catch(Exception exception) {

				// displays a balloon tip in the systray with the detailed error message
				notifyIcon.Icon = Properties.Resources.warning;
				notifyIcon.Text = "Erreur lors de la synchronisation";
				notifyIcon.ShowBalloonTip(450, "Erreur", "Une erreur est survenue lors de la synchronisation de la boite de réception : " + exception.Message, ToolTipIcon.Warning);
			}
		}

		/// <summary>
		/// Manages the AskonExit user setting
		/// </summary>
		private void fieldAskonExit_Click(object sender, EventArgs e) {
			Properties.Settings.Default.AskonExit = fieldAskonExit.Checked;
		}

		/// <summary>
		/// Manages the AudioNotification user setting
		/// </summary>
		private void fieldAudioNotification_Click(object sender, EventArgs e) {
			Properties.Settings.Default.AudioNotification = fieldAudioNotification.Checked;
		}

		/// <summary>
		/// Manages the NumericDelay user setting
		/// </summary>
		private void fieldNumericDelay_ValueChanged(object sender, EventArgs e) {
			Properties.Settings.Default.TimerInterval = 1000 * (fieldStepDelay.Text == "minute(s)" ? 60 : 3600) * Convert.ToInt32(fieldNumericDelay.Value);
			Properties.Settings.Default.NumericDelay = fieldNumericDelay.Value;
		}

		/// <summary>
		/// Manages the StepDelay user setting
		/// </summary>
		private void fieldStepDelay_SelectionChangeCommitted(object sender, EventArgs e) {
			Properties.Settings.Default.TimerInterval = 1000 * (fieldStepDelay.Text == "minute(s)" ? 60 : 3600) * Convert.ToInt32(fieldNumericDelay.Value);
			Properties.Settings.Default.StepDelay = fieldStepDelay.Text;
		}

		/// <summary>
		/// Opens the credit website
		/// </summary>
		private void linkWebsiteYusuke_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("http://p.yusukekamiyamane.com");
		}

		/// <summary>
		/// Hides the settings saved label
		/// </summary>
		private void tabControl_Selecting(object sender, TabControlCancelEventArgs e) {
			labelSettingsSaved.Visible = false;
		}

		/// <summary>
		/// Closes the preferences when the OK button is clicked
		/// </summary>
		private void buttonOK_Click(object sender, EventArgs e) {
			WindowState = FormWindowState.Minimized;
			ShowInTaskbar = false;
		}

		/// <summary>
		/// Manages the context menu Synchronize item
		/// </summary>
		private void menuItemSynchronize_Click(object sender, EventArgs e) {
			this.SyncInbox();
		}

		/// <summary>
		/// Manages the context menu MarkAsRead item
		/// </summary>
		private void menuItemMarkAsRead_Click(object sender, EventArgs e) {
			try {

				// displays the sync icon
				notifyIcon.Icon = Properties.Resources.sync;
				notifyIcon.Text = "Synchronisation en cours ...";

				// gets all unread threads
				UsersResource.ThreadsResource.ListRequest threads = this.service.Users.Threads.List("me");
				threads.LabelIds = "UNREAD";
				IList<Google.Apis.Gmail.v1.Data.Thread> unread = threads.Execute().Threads;

				// loops through all unread threads and removes the "unread" label for each one
				foreach (Google.Apis.Gmail.v1.Data.Thread thread in unread) {
					ModifyThreadRequest request = new ModifyThreadRequest();
					request.RemoveLabelIds = new List<string>() { "UNREAD" };
					this.service.Users.Threads.Modify(request, "me", thread.Id).Execute();
				}

				// restores the default systray icon and text
				notifyIcon.Icon = Properties.Resources.normal;
				notifyIcon.Text = "Pas de nouveau message";

				// disables the mark as read menu item
				menuItemMarkAsRead.Enabled = false;
			} catch (Exception exception) {

				// enabled the mark as read menu item
				menuItemMarkAsRead.Enabled = true;

				// displays a balloon tip in the systray with the detailed error message
				notifyIcon.Icon = Properties.Resources.warning;
				notifyIcon.Text = "Erreur lors de l'opération \"Marquer comme lu(s)\"";
				notifyIcon.ShowBalloonTip(450, "Erreur", "Une erreur est survenue lors de l'opération \"Marquer comme lu(s)\" : " + exception.Message, ToolTipIcon.Warning);
			}
		}

		/// <summary>
		/// Manages the context menu Settings item
		/// </summary>
		private void menuItemSettings_Click(object sender, EventArgs e) {
			ShowInTaskbar = true;
			WindowState = FormWindowState.Normal;
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

		/// <summary>
		/// Synchronizes the user mailbox on every timer tick
		/// </summary>
		private void timer_Tick(object sender, EventArgs e) {
			this.SyncInbox(true);
		}
	}
}
