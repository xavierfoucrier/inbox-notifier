﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using notifier.Properties;

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

			// attaches the context menu to the systray icon
			notifyIcon.ContextMenu = contextMenu;

			// binds the "PropertyChanged" event of the settings to automatically save the user settings and display the setting label
			Settings.Default.PropertyChanged += new PropertyChangedEventHandler((object o, PropertyChangedEventArgs target) => {
				Settings.Default.Save();
				labelSettingsSaved.Visible = true;
			});

			// binds the "NetworkAvailabilityChanged" event to automatically display a notification about network connectivity, depending on the user settings
			NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler((object o, NetworkAvailabilityEventArgs target) => {

				// discards notification
				if (!Settings.Default.NetworkConnectivityNotification) {
					return;
				}

				// checks for available networks
				if (!NetworkInterface.GetIsNetworkAvailable()) {
					notifyIcon.Icon = Resources.warning;
					notifyIcon.Text = "Erreur réseau";
					notifyIcon.ShowBalloonTip(450, "Erreur réseau", "Aucun réseau n'est actuellement disponible, vous n'êtes probablement pas connecté à Internet. Le service sera rétabli dès que vous serez à nouveau connecté à Internet.", ToolTipIcon.Warning);

					return;
				}

				// loops through all network interface to check network connectivity
				foreach (NetworkInterface network in NetworkInterface.GetAllNetworkInterfaces()) {

					// discards "non-up" status, modem, serial, loopback and tunnel
					if (network.OperationalStatus != OperationalStatus.Up || network.Speed < 0 || network.NetworkInterfaceType == NetworkInterfaceType.Loopback || network.NetworkInterfaceType == NetworkInterfaceType.Tunnel) {
						continue;
					}

					// discards virtual cards (like virtual box, virtual pc, etc.) and microsoft loopback adapter (showing as ethernet card)
					if (network.Name.ToLower().Contains("virtual") || network.Description.ToLower().Contains("virtual") || network.Description.ToLower() == ("microsoft loopback adapter")) {
						continue;
					}

					// syncs the inbox when a network interface is available
					this.SyncInbox();
					break;
				}

				// displays a notification to indicate that the network connectivity has been restored
				notifyIcon.ShowBalloonTip(450, "Connexion au réseau rétablie", "La connexion au réseau a été rétablie : vous êtes de nouveau connecté à Internet. La boite de réception a été automatiquement synchronisée.", ToolTipIcon.Info);
			});

			// displays the step delay setting
			fieldStepDelay.SelectedIndex = Settings.Default.StepDelay;

			// displays the privacy notification setting
			switch (Settings.Default.PrivacyNotification) {
				case 0:
					fieldPrivacyNotificationNone.Checked = true;
					break;
				default:
				case 1:
					fieldPrivacyNotificationShort.Checked = true;
					break;
				case 2:
					fieldPrivacyNotificationAll.Checked = true;
					break;
			}

			// displays the product version
			string[] version = Application.ProductVersion.Split('.');
			labelVersion.Text = version[0] + "." + version[1] + "-" + (version[2] == "0" ? "alpha" : version[2] == "1" ? "beta" : version[2] == "2" ? "rc" : version[2] == "3" ? "release" : "") + (version[3] != "0" ? " " + version[3] : "");
		}

		/// <summary>
		/// Prompts the user before closing the form
		/// </summary>
		private void Main_FormClosing(object sender, FormClosingEventArgs e) {

			// asks the user for exit, depending on the application settings
			if (e.CloseReason != CloseReason.ApplicationExitCall && Settings.Default.AskonExit) {
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

				// synchronizes the user mailbox
				this.SyncInbox();
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
		/// <param name="timertick">Indicates if the synchronization come's from the timer tick or has been manually triggered</param>
		private void SyncInbox(bool timertick = false) {

			// displays the sync icon, but not on every tick of the timer
			if (!timertick) {
				notifyIcon.Icon = Resources.sync;
				notifyIcon.Text = "Synchronisation en cours ...";
			}

			try {

				// manages the spam notification
				if (Settings.Default.SpamNotification) {

					// gets the "spam" label
					Google.Apis.Gmail.v1.Data.Label spam = this.service.Users.Labels.Get("me", "SPAM").Execute();

					// manages unread spams
					if (spam.ThreadsUnread > 0) {

						// sets the notification icon and text
						notifyIcon.Icon = Resources.spam;

						// plays a sound on unread spams
						if (Settings.Default.AudioNotification) {
							SystemSounds.Exclamation.Play();
						}

						// displays a balloon tip in the systray with the total of unread threads
						notifyIcon.ShowBalloonTip(450, spam.ThreadsUnread.ToString() + " " + (spam.ThreadsUnread > 1 ? "spams non lus" : "spam non lu"), "Double-cliquez sur l'icône pour accéder à votre boîte de réception. Tant que vous n'aurez pas vérifié ce spam, votre boite de réception ne sera pas synchronisée et vous ne serez pas informé de l'arrivée de nouveaux messages.", ToolTipIcon.Error);
						notifyIcon.Text = spam.ThreadsUnread.ToString() + " " + (spam.ThreadsUnread > 1 ? "spams non lus" : "spam non lu");

						return;
					}
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
					notifyIcon.Icon = this.inbox.ThreadsUnread <= 9 ? (Icon)Resources.ResourceManager.GetObject("mail_" + this.inbox.ThreadsUnread.ToString()) : Resources.mail_more;

					// plays a sound on unread threads
					if (Settings.Default.AudioNotification) {
						SystemSounds.Asterisk.Play();
					}

					//  displays a balloon tip in the systray with the total of unread threads and message details, depending on the user privacy setting
					if (this.inbox.ThreadsUnread == 1 && Settings.Default.PrivacyNotification != 2) {
						UsersResource.MessagesResource.ListRequest messages = this.service.Users.Messages.List("me");
						messages.LabelIds = "UNREAD";
						messages.MaxResults = 1;
						Google.Apis.Gmail.v1.Data.Message message = this.service.Users.Messages.Get("me", messages.Execute().Messages.First().Id).Execute();

						string subject = "";
						string from = "";

						foreach (MessagePartHeader header in message.Payload.Headers) {
							if (header.Name == "Subject") {
								subject = header.Value;
							} else if (header.Name == "From") {
								from = System.Text.RegularExpressions.Regex.Replace(header.Value, "<.*>", "");
							}
						}

						if (Settings.Default.PrivacyNotification == 0) {
							notifyIcon.ShowBalloonTip(450, from, WebUtility.HtmlDecode(message.Snippet), ToolTipIcon.Info);
						} else if (Settings.Default.PrivacyNotification == 1) {
							notifyIcon.ShowBalloonTip(450, from, subject, ToolTipIcon.Info);
						}
					} else {
						notifyIcon.ShowBalloonTip(450, this.inbox.ThreadsUnread.ToString() + " " + (this.inbox.ThreadsUnread > 1 ? "emails non lus" : "email non lu"), "Double-cliquez sur l'icône pour accéder à votre boîte de réception.", ToolTipIcon.Info);
					}

					// displays the notification text
					notifyIcon.Text = this.inbox.ThreadsUnread.ToString() + " " + (this.inbox.ThreadsUnread > 1 ? "emails non lus" : "email non lu");

					// enables the mark as read menu item
					menuItemMarkAsRead.Enabled = true;
				} else {

					// restores the default systray icon and text
					notifyIcon.Icon = Resources.normal;
					notifyIcon.Text = "Pas de nouveau message";

					// disables the mark as read menu item
					menuItemMarkAsRead.Enabled = false;
				}

				// saves the number of unread threads
				this.unreadthreads = this.inbox.ThreadsUnread;
			} catch(Exception exception) {

				// displays a balloon tip in the systray with the detailed error message
				notifyIcon.Icon = Resources.warning;
				notifyIcon.Text = "Erreur lors de la synchronisation";
				notifyIcon.ShowBalloonTip(450, "Erreur", "Une erreur est survenue lors de la synchronisation de la boite de réception : " + exception.Message, ToolTipIcon.Warning);
			}
		}

		/// <summary>
		/// Manages the AskonExit user setting
		/// </summary>
		private void fieldAskonExit_Click(object sender, EventArgs e) {
			Settings.Default.AskonExit = fieldAskonExit.Checked;
		}

		/// <summary>
		/// Manages the Language user setting
		/// </summary>
		private void fieldLanguage_SelectionChangeCommitted(object sender, EventArgs e) {
			Settings.Default.Language = fieldLanguage.Text;
		}

		/// <summary>
		/// Manages the AudioNotification user setting
		/// </summary>
		private void fieldAudioNotification_Click(object sender, EventArgs e) {
			Settings.Default.AudioNotification = fieldAudioNotification.Checked;
		}

		/// <summary>
		/// Manages the SpamNotification user setting
		/// </summary>
		private void fieldSpamNotification_Click(object sender, EventArgs e) {
			Settings.Default.SpamNotification = fieldSpamNotification.Checked;
		}

		/// <summary>
		/// Manages the NumericDelay user setting
		/// </summary>
		private void fieldNumericDelay_ValueChanged(object sender, EventArgs e) {
			Settings.Default.TimerInterval = 1000 * (fieldStepDelay.SelectedIndex == 0 ? 60 : 3600) * Convert.ToInt32(fieldNumericDelay.Value);
			Settings.Default.NumericDelay = fieldNumericDelay.Value;
		}

		/// <summary>
		/// Manages the StepDelay user setting
		/// </summary>
		private void fieldStepDelay_SelectionChangeCommitted(object sender, EventArgs e) {
			Settings.Default.TimerInterval = 1000 * (fieldStepDelay.SelectedIndex == 0 ? 60 : 3600) * Convert.ToInt32(fieldNumericDelay.Value);
			Settings.Default.StepDelay = fieldStepDelay.SelectedIndex;
		}

		/// <summary>
		/// Manages the NetworkConnectivityNotification user setting
		/// </summary>
		private void fieldNetworkConnectivityNotification_Click(object sender, EventArgs e) {
			Settings.Default.NetworkConnectivityNotification = fieldNetworkConnectivityNotification.Checked;
		}

		/// <summary>
		/// Manages the PrivacyNotificationNone user setting
		/// </summary>
		private void fieldPrivacyNotificationNone_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.PrivacyNotification = 0;
		}

		/// <summary>
		/// Manages the PrivacyNotificationShort user setting
		/// </summary>
		private void fieldPrivacyNotificationShort_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.PrivacyNotification = 1;
		}

		/// <summary>
		/// Manages the PrivacyNotificationAll user setting
		/// </summary>
		private void fieldPrivacyNotificationAll_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.PrivacyNotification = 2;
		}

		/// <summary>
		/// Opens the Yusuke website
		/// </summary>
		private void linkWebsiteYusuke_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("http://p.yusukekamiyamane.com");
		}

		/// <summary>
		/// Opens the Xavier website
		/// </summary>
		private void linkWebsiteXavier_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("http://www.xavierfoucrier.fr");
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
			labelSettingsSaved.Visible = false;
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
				notifyIcon.Icon = Resources.sync;
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
				notifyIcon.Icon = Resources.normal;
				notifyIcon.Text = "Pas de nouveau message";

				// disables the mark as read menu item
				menuItemMarkAsRead.Enabled = false;
			} catch (Exception exception) {

				// enabled the mark as read menu item
				menuItemMarkAsRead.Enabled = true;

				// displays a balloon tip in the systray with the detailed error message
				notifyIcon.Icon = Resources.warning;
				notifyIcon.Text = "Erreur lors de l'opération \"Marquer comme lu(s)\"";
				notifyIcon.ShowBalloonTip(450, "Erreur", "Une erreur est survenue lors de l'opération \"Marquer comme lu(s)\" : " + exception.Message, ToolTipIcon.Warning);
			}
		}

		/// <summary>
		/// Delays the inbox sync during a certain time
		/// </summary>
		/// <param name="item">Item selected in the menu</param>
		/// <param name="delay">Delay until the next inbox sync</param>
		private void DoNotDisturb(MenuItem item, int delay) {

			// exits if the selected menu item is already checked
			if (item.Checked) {
				return;
			}

			// unchecks all menu items
			foreach (MenuItem i in menuItemTimout.MenuItems) {
				i.Checked = false;
			}

			// displays the user choice
			item.Checked = true;

			// sets the new timer interval depending on the user settings
			timer.Interval = delay;

			// updates the systray icon and text
			if (delay != Settings.Default.TimerInterval) {
				notifyIcon.Icon = Resources.timeout;
				notifyIcon.Text = "Ne pas déranger - " + DateTime.Now.AddMilliseconds(delay).ToShortTimeString();
			} else {
				this.SyncInbox();
			}
		}

		/// <summary>
		/// Manages the context menu TimeoutDisabled item
		/// </summary>
		private void menuItemTimeoutDisabled_Click(object sender, EventArgs e) {
			DoNotDisturb((MenuItem)sender, Settings.Default.TimerInterval);
		}

		/// <summary>
		/// Manages the context menu Timeout30m item
		/// </summary>
		private void menuItemTimeout30m_Click(object sender, EventArgs e) {
			DoNotDisturb((MenuItem)sender, 1000 * 60 * 30);
		}

		/// <summary>
		/// Manages the context menu Timeout1h item
		/// </summary>
		private void menuItemTimeout1h_Click(object sender, EventArgs e) {
			DoNotDisturb((MenuItem)sender, 1000 * 60 * 60);
		}

		/// <summary>
		/// Manages the context menu Timeout2h item
		/// </summary>
		private void menuItemTimeout2h_Click(object sender, EventArgs e) {
			DoNotDisturb((MenuItem)sender, 1000 * 60 * 60 * 2);
		}

		/// <summary>
		/// Manages the context menu Timeout5h item
		/// </summary>
		private void menuItemTimeout5h_Click(object sender, EventArgs e) {
			DoNotDisturb((MenuItem)sender, 1000 * 60 * 60 * 5);
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

			// restores the default systray icon and text: pretends that the user had read all his mail
			notifyIcon.Icon = Resources.normal;
			notifyIcon.Text = "Pas de nouveau message";
		}

		/// <summary>
		/// Synchronizes the user mailbox on every timer tick
		/// </summary>
		private void timer_Tick(object sender, EventArgs e) {

			// restores the timer interval when the do not disturb time has elapsed
			if (timer.Interval != Settings.Default.TimerInterval) {
				DoNotDisturb(menuItemTimeoutDisabled, Settings.Default.TimerInterval);

				return;
			}

			// synchronizes the inbox
			this.SyncInbox(true);
		}
	}
}
