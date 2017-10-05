using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using HtmlAgilityPack;
using Microsoft.Win32;
using notifier.Languages;
using notifier.Properties;

namespace notifier {
	public partial class Main : Form {

		// privacy possibilities
		private enum Privacy : uint {
			None = 0,
			Short = 1,
			All = 2
		}

		// update period possibilities
		private enum Period : uint {
			Startup = 0,
			Day = 1,
			Week = 2,
			Month = 3
		}

		// gmail api service
		private GmailService service;

		// user credential for the gmail authentication
		private UserCredential credential;

		// inbox label
		private Google.Apis.Gmail.v1.Data.Label inbox;

		// unread threads
		private int? unreadthreads = 0;

		// number of automatic reconnection
		private uint reconnect = 0;

		// last synchronization time
		private DateTime synctime = DateTime.Now;

		// local application data folder name
		private string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Gmail Notifier";

		// version number
		private string version = "";

		// flag defining the update state
		private bool updating = false;

		// http client used to check for updates
		private HttpClient http = new HttpClient();

		// number of maximum automatic reconnection
		private const int MAX_AUTO_RECONNECT = 3;

		// number in seconds between reconnections (in seconds)
		private const int INTERVAL_RECONNECT = 10;

		// github repository root link
		private const string GITHUB_REPOSITORY = "https://github.com/xavierfoucrier/gmail-notifier";

		// gmail base root link
		private const string GMAIL_BASEURL = "https://mail.google.com/mail/u/0";

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

			// hides the form by default
			Visible = false;

			// upgrades the user configuration if necessary
			if (Settings.Default.UpdateRequired) {
				Settings.Default.Upgrade();

				// switches the update required state
				Settings.Default.UpdateRequired = false;
				Settings.Default.Save();

				// cleans temporary update files from previous upgrade
				if (Directory.Exists(appdata)) {
					IEnumerable<string> executables = Directory.EnumerateFiles(appdata, "*.exe", SearchOption.TopDirectoryOnly);

					foreach (string executable in executables) {
						try {
							File.Delete(executable);
						} catch(Exception) {
							// nothing to catch: executable is currently locked
							// setup package will be removed next time
						}
					}
				}
			}

			// initializes the application version number
			string[] version = Application.ProductVersion.Split('.');
			this.version = "v" + version[0] + "." + version[1] + (version[3] != "0" ? "." + version[3] : "") + "-" + (version[2] == "0" ? "alpha" : version[2] == "1" ? "beta" : version[2] == "2" ? "rc" : version[2] == "3" ? "release" : "");

			// displays a systray notification on first load
			if (Settings.Default.FirstLoad && !Directory.Exists(this.appdata)) {
				notifyIcon.ShowBalloonTip(7000, translation.welcome, translation.firstLoad, ToolTipIcon.Info);

				// switches the first load state
				Settings.Default.FirstLoad = false;
				Settings.Default.Save();

				// waits for 7 seconds to complete the thread
				System.Threading.Thread.Sleep(7000);
			}

			// configures the help provider
			HelpProvider help = new HelpProvider();
			help.SetHelpString(fieldRunAtWindowsStartup, translation.helpRunAtWindowsStartup);
			help.SetHelpString(fieldAskonExit, translation.helpAskonExit);
			help.SetHelpString(fieldLanguage, translation.helpLanguage);
			help.SetHelpString(labelEmailAddress, translation.helpEmailAddress);
			help.SetHelpString(labelTokenDelivery, translation.helpTokenDelivery);
			help.SetHelpString(buttonGmailDisconnect, translation.helpGmailDisconnect);
			help.SetHelpString(fieldMessageNotification, translation.helpMessageNotification);
			help.SetHelpString(fieldAudioNotification, translation.helpAudioNotification);
			help.SetHelpString(fieldSpamNotification, translation.helpSpamNotification);
			help.SetHelpString(fieldNumericDelay, translation.helpNumericDelay);
			help.SetHelpString(fieldStepDelay, translation.helpStepDelay);
			help.SetHelpString(fieldNotificationBehavior, translation.helpNotificationBehavior);
			help.SetHelpString(fieldPrivacyNotificationNone, translation.helpPrivacyNotificationNone);
			help.SetHelpString(fieldPrivacyNotificationShort, translation.helpPrivacyNotificationShort);
			help.SetHelpString(fieldPrivacyNotificationAll, translation.helpPrivacyNotificationAll);

			// authenticates the user
			this.AsyncAuthentication();

			// attaches the context menu to the systray icon
			notifyIcon.ContextMenu = contextMenu;

			// binds the "PropertyChanged" event of the settings to automatically save the user settings and display the setting label
			Settings.Default.PropertyChanged += new PropertyChangedEventHandler((object o, PropertyChangedEventArgs target) => {
				Settings.Default.Save();
				labelSettingsSaved.Visible = true;
			});

			// binds the "NetworkAvailabilityChanged" event to automatically sync the inbox when a network is available
			NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler((object o, NetworkAvailabilityEventArgs target) => {

				// stops the reconnect process if it is running
				if (this.reconnect != 0) {
					timerReconnect.Enabled = false;
					timerReconnect.Interval = 100;
					this.reconnect = 0;
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

					// syncs the inbox when a network interface is available and the timeout mode is disabled
					if (timer.Interval == Settings.Default.TimerInterval) {
						this.AsyncSyncInbox();
					}

					break;
				}
			});

			// binds the "PowerModeChanged" event to automatically pause/resume the application synchronization
			SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler((object o, PowerModeChangedEventArgs target) => {
				if (target.Mode == PowerModes.Suspend) {
					timer.Enabled = false;
				} else if (target.Mode == PowerModes.Resume) {

					// syncs the inbox if the timeout mode is disabled
					if (timer.Interval == Settings.Default.TimerInterval) {
						timer.Enabled = true;
					}
				}
			});

			// displays the step delay setting
			fieldStepDelay.SelectedIndex = Settings.Default.StepDelay;

			// displays the notification behavior setting
			fieldNotificationBehavior.SelectedIndex = Settings.Default.NotificationBehavior;

			// displays the privacy notification setting
			switch (Settings.Default.PrivacyNotification) {
				case (int)Privacy.None:
					fieldPrivacyNotificationNone.Checked = true;
					pictureBoxPrivacyPreview.Image = Resources.privacy_none;
					break;
				default:
				case (int)Privacy.Short:
					fieldPrivacyNotificationShort.Checked = true;
					pictureBoxPrivacyPreview.Image = Resources.privacy_short;
					break;
				case (int)Privacy.All:
					fieldPrivacyNotificationAll.Checked = true;
					pictureBoxPrivacyPreview.Image = Resources.privacy_all;
					break;
			}

			// displays the update period setting
			fieldUpdatePeriod.SelectedIndex = Settings.Default.UpdatePeriod;

			// displays the update control setting
			labelUpdateControl.Text = Settings.Default.UpdateControl.ToString();

			// displays the product version
			linkVersion.Text = this.version.Substring(1);

			// positioning the check for update link
			linkCheckForUpdate.Left = linkVersion.Right + 2;

			// displays a tooltip for the product version
			ToolTip tipTag = new ToolTip();
			tipTag.SetToolTip(linkVersion, GITHUB_REPOSITORY + "/releases/tag/" + this.version);
			tipTag.ToolTipTitle = translation.tipReleaseNotes;
			tipTag.ToolTipIcon = ToolTipIcon.Info;
			tipTag.IsBalloon = false;

			// displays a tooltip for the product version
			ToolTip tipCheckForUpdate = new ToolTip();
			tipCheckForUpdate.SetToolTip(linkCheckForUpdate, translation.checkForUpdate);
			tipCheckForUpdate.IsBalloon = false;

			// displays a tooltip for the website link
			ToolTip tipWebsiteYusuke = new ToolTip();
			tipWebsiteYusuke.SetToolTip(linkWebsiteYusuke, "http://p.yusukekamiyamane.com");
			tipWebsiteYusuke.IsBalloon = false;

			// displays a tooltip for the website link
			ToolTip tipWebsiteXavier = new ToolTip();
			tipWebsiteXavier.SetToolTip(linkWebsiteXavier, "http://www.xavierfoucrier.fr");
			tipWebsiteXavier.IsBalloon = false;

			// displays a tooltip for the license link
			ToolTip tipLicense = new ToolTip();
			tipLicense.SetToolTip(linkLicense, GITHUB_REPOSITORY + "/blob/master/LICENSE.md");
			tipLicense.IsBalloon = false;

			// displays a tooltip for the website link
			ToolTip tipSoftpedia = new ToolTip();
			tipSoftpedia.SetToolTip(linkSoftpedia, translation.freeSoftware);
			tipSoftpedia.ToolTipTitle = translation.tipSoftpedia;
			tipSoftpedia.ToolTipIcon = ToolTipIcon.Info;
			tipSoftpedia.IsBalloon = false;
		}

		/// <summary>
		/// Prompts the user before closing the form
		/// </summary>
		private void Main_FormClosing(object sender, FormClosingEventArgs e) {

			// asks the user for exit, depending on the application settings
			if (e.CloseReason != CloseReason.ApplicationExitCall && e.CloseReason != CloseReason.WindowsShutDown && Settings.Default.AskonExit) {
				DialogResult dialog = MessageBox.Show(translation.applicationExitQuestion, translation.applicationExit, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

				if (dialog == DialogResult.No) {
					e.Cancel = true;
				}
			}

			// disposes the gmail api service
			if (this.service != null) {
				this.service.Dispose();
			}
		}

		/// <summary>
		/// Opens the Google website to checks the internet connectivity
		/// </summary>
		/// <returns>Indicates if the user is connected to the internet, false means that the request to the Google server has failed</returns>
		private bool IsInternetAvailable() {
			try {
				using (var client = new WebClient()) {
					using (var stream = client.OpenRead("http://www.google.com")) {
						return true;
					}
				}
			} catch (Exception) {
				return false;
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

				// displays the user email address
				labelEmailAddress.Text = this.service.Users.GetProfile("me").Execute().EmailAddress;
				labelTokenDelivery.Text = this.credential.Token.IssuedUtc.ToString();
			} catch(Exception) {

				// exits the application if the google api token file doesn't exists
				if (!Directory.Exists(this.appdata) || !Directory.EnumerateFiles(this.appdata).Any()) {
					MessageBox.Show(translation.authenticationWithGmailRefused, translation.authenticationFailed, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					Application.Exit();
				}
			} finally {

				// synchronizes the user mailbox, after checking for update depending on the user settings, or by default after the asynchronous authentication
				if (Settings.Default.UpdateService && Settings.Default.UpdatePeriod == (int)Period.Startup) {
					this.AsyncCheckForUpdate(!Settings.Default.UpdateDownload, true);
				} else {
					this.AsyncSyncInbox();
				}
			}
		}

		/// <summary>
		/// Asynchronous task used to get the user authorization
		/// </summary>
		/// <returns>OAuth 2.0 user credential</returns>
		private async Task<UserCredential> AsyncAuthorizationBroker() {

			// uses the client secret file for the context
			using (var stream = new FileStream(Path.GetDirectoryName(Application.ExecutablePath) + "/client_secret.json", FileMode.Open, FileAccess.Read)) {

				// waits for the user validation, only if the user has not already authorized the application
				return await GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					new string[] { GmailService.Scope.GmailModify },
					"user",
					CancellationToken.None,
					new FileDataStore(this.appdata, true)
				);
			}
		}

		/// <summary>
		/// Asynchronous method used to get account statistics
		/// </summary>
		private async void AsyncStatistics() {
			try {
				labelTotalUnreadMails.Text = this.inbox.ThreadsUnread.ToString();
				labelTotalMails.Text = this.inbox.ThreadsTotal.ToString();

				ListDraftsResponse drafts = await this.AsyncDrafts();
				ListLabelsResponse labels = await this.AsyncLabels();

				if (drafts.Drafts != null) {
					labelTotalDrafts.Text = drafts.Drafts.Count.ToString();
				}

				if (labels.Labels != null) {
					labelTotalLabels.Text = labels.Labels.Count.ToString();
				}
			} catch (Exception) {
				// nothing to catch
			}
		}

		/// <summary>
		/// Asynchronous task used to get the user drafts
		/// </summary>
		/// <returns>List of drafts</returns>
		private async Task<ListDraftsResponse> AsyncDrafts() {
			return await this.service.Users.Drafts.List("me").ExecuteAsync();
		}

		/// <summary>
		/// Asynchronous task used to get the user labels
		/// </summary>
		/// <returns>List of labels</returns>
		private async Task<ListLabelsResponse> AsyncLabels() {
			return await this.service.Users.Labels.List("me").ExecuteAsync();
		}

		/// <summary>
		/// Asynchronous method used to synchronize the user inbox
		/// </summary>
		/// <param name="timertick">Indicates if the synchronization come's from the timer tick or has been manually triggered</param>
		private async void AsyncSyncInbox(bool timertick = false) {
			
			// prevents the application from syncing the inbox when updating
			if (this.updating) {
				return;
			}

			// updates the synchronization time
			this.synctime = DateTime.Now;

			// resets reconnection count and prevents the application from displaying continuous warning icon when a timertick synchronization occurs after a reconnection attempt
			if (this.reconnect != 0) {
				timertick = false;
				this.reconnect = 0;
			}

			// disables the timeout when the user do a manual synchronization
			if (timer.Interval != Settings.Default.TimerInterval) {
				Timeout(menuItemTimeoutDisabled, Settings.Default.TimerInterval);

				// exits the method because the timeout function automatically restarts a synchronization once it has been disabled
				return;
			}

			// if internet is down, attempts to reconnect the user mailbox
			if (!this.IsInternetAvailable()) {
				timerReconnect.Enabled = true;
				timer.Enabled = false;

				return;
			}

			// activates the necessary menu items
			menuItemSynchronize.Enabled = true;
			menuItemTimout.Enabled = true;
			menuItemSettings.Enabled = true;

			// displays the sync icon, but not on every tick of the timer
			if (!timertick) {
				notifyIcon.Icon = Resources.sync;
				notifyIcon.Text = translation.sync;
			}

			// check for update, depending on the user settings
			if (Settings.Default.UpdateService) {
				switch (Settings.Default.UpdatePeriod) {
					case (int)Period.Day:
						if (DateTime.Now >= Settings.Default.UpdateControl.AddDays(1)) {
							this.AsyncCheckForUpdate(false);
						}

						break;
					case (int)Period.Week:
						if (DateTime.Now >= Settings.Default.UpdateControl.AddDays(7)) {
							this.AsyncCheckForUpdate(false);
						}

						break;
					case (int)Period.Month:
						if (DateTime.Now >= Settings.Default.UpdateControl.AddMonths(1)) {
							this.AsyncCheckForUpdate(false);
						}

						break;
				}
			}

			try {

				// manages the spam notification
				if (Settings.Default.SpamNotification) {

					// exits if a spam is already detected
					if (timertick && notifyIcon.Tag != null && notifyIcon.Tag.ToString() == "#spam") {
						return;
					}

					// gets the "spam" label
					Google.Apis.Gmail.v1.Data.Label spam = await this.service.Users.Labels.Get("me", "SPAM").ExecuteAsync();

					// manages unread spams
					if (spam.ThreadsUnread > 0) {

						// sets the notification icon and text
						notifyIcon.Icon = Resources.spam;

						// plays a sound on unread spams
						if (Settings.Default.AudioNotification) {
							SystemSounds.Exclamation.Play();
						}

						// displays a balloon tip in the systray with the total of unread threads
						notifyIcon.ShowBalloonTip(450, spam.ThreadsUnread.ToString() + " " + (spam.ThreadsUnread > 1 ? translation.unreadSpams : translation.unreadSpam), translation.newUnreadSpam, ToolTipIcon.Error);
						notifyIcon.Text = spam.ThreadsUnread.ToString() + " " + (spam.ThreadsUnread > 1 ? translation.unreadSpams : translation.unreadSpam);
						notifyIcon.Tag = "#spam";

						return;
					}
				}

				// gets the "inbox" label
				this.inbox = await this.service.Users.Labels.Get("me", "INBOX").ExecuteAsync();

				// updates the statistics
				this.AsyncStatistics();

				// exits the sync if the number of unread threads is the same as before
				if (timertick && (this.inbox.ThreadsUnread == this.unreadthreads)) {
					return;
				}

				// manages unread threads
				if (this.inbox.ThreadsUnread > 0) {

					// sets the notification icon and text
					notifyIcon.Icon = this.inbox.ThreadsUnread <= 9 ? (Icon)Resources.ResourceManager.GetObject("mail_" + this.inbox.ThreadsUnread.ToString()) : Resources.mail_more;

					// manages message notification
					if (Settings.Default.MessageNotification) {

						// plays a sound on unread threads
						if (Settings.Default.AudioNotification) {
							SystemSounds.Asterisk.Play();
						}

						// gets the message details
						UsersResource.MessagesResource.ListRequest messages = this.service.Users.Messages.List("me");
						messages.LabelIds = "UNREAD";
						messages.MaxResults = 1;
						Google.Apis.Gmail.v1.Data.Message message = await this.service.Users.Messages.Get("me", await messages.ExecuteAsync().ContinueWith(m => {
							return m.Result.Messages.First().Id;
						})).ExecuteAsync();

						//  displays a balloon tip in the systray with the total of unread threads and message details, depending on the user privacy setting
						if (this.inbox.ThreadsUnread == 1 && Settings.Default.PrivacyNotification != (int)Privacy.All) {
							string subject = "";
							string from = "";

							foreach (MessagePartHeader header in message.Payload.Headers) {
								if (header.Name == "Subject") {
									subject = header.Value != "" ? header.Value : translation.newUnreadMessage;
								} else if (header.Name == "From") {
									Match match = Regex.Match(header.Value, ".* <");

									if (match.Length != 0) {
										from = match.Captures[0].Value.Replace(" <", "").Replace("\"", "");
									} else {
										match = Regex.Match(header.Value, "<?.*>?");
										from = match.Length != 0 ? match.Value.ToLower().Replace("<", "").Replace(">", "") : header.Value.Replace(match.Value, this.inbox.ThreadsUnread.ToString() + " " + translation.unreadMessage);
									}
								}
							}

							if (Settings.Default.PrivacyNotification == (int)Privacy.None) {
								notifyIcon.ShowBalloonTip(450, from, message.Snippet != "" ? WebUtility.HtmlDecode(message.Snippet) : translation.newUnreadMessage, ToolTipIcon.Info);
							} else if (Settings.Default.PrivacyNotification == (int)Privacy.Short) {
								notifyIcon.ShowBalloonTip(450, from, subject, ToolTipIcon.Info);
							}
						} else {
							notifyIcon.ShowBalloonTip(450, this.inbox.ThreadsUnread.ToString() + " " + (this.inbox.ThreadsUnread > 1 ? translation.unreadMessages : translation.unreadMessage), translation.newUnreadMessage, ToolTipIcon.Info);
						}

						// manages the balloon tip click event handler: we store the "notification tag" to allow the user to directly display the specified view (inbox/message/spam) in a browser
						notifyIcon.Tag = "#inbox" + (this.inbox.ThreadsUnread == 1 ? "/" + message.Id : "");
					}

					// displays the notification text
					notifyIcon.Text = this.inbox.ThreadsUnread.ToString() + " " + (this.inbox.ThreadsUnread > 1 ? translation.unreadMessages : translation.unreadMessage);

					// enables the mark as read menu item
					menuItemMarkAsRead.Enabled = true;
				} else {

					// restores the default systray icon and text
					notifyIcon.Icon = Resources.normal;
					notifyIcon.Text = translation.noMessage;

					// disables the mark as read menu item
					menuItemMarkAsRead.Enabled = false;
				}

				// saves the number of unread threads
				this.unreadthreads = this.inbox.ThreadsUnread;
			} catch(Exception exception) {

				// displays a balloon tip in the systray with the detailed error message
				notifyIcon.Icon = Resources.warning;
				notifyIcon.Text = translation.syncError;
				notifyIcon.ShowBalloonTip(1500, translation.error, translation.syncErrorOccured + exception.Message, ToolTipIcon.Warning);
			} finally {
				notifyIcon.Text = notifyIcon.Text.Split('\n')[0] + "\n" + translation.syncTime.Replace("{time}", this.synctime.ToLongTimeString());
			}
		}

		/// <summary>
		/// Asynchronous method used to mark as read the user inbox
		/// </summary>
		private async void AsyncMarkAsRead() {
			try {

				// displays the sync icon
				notifyIcon.Icon = Resources.sync;
				notifyIcon.Text = translation.sync;

				// gets all unread threads
				UsersResource.ThreadsResource.ListRequest threads = this.service.Users.Threads.List("me");
				threads.LabelIds = "UNREAD";
				ListThreadsResponse list = await threads.ExecuteAsync();
				IList<Google.Apis.Gmail.v1.Data.Thread> unread = list.Threads;

				// loops through all unread threads and removes the "unread" label for each one
				if (unread != null && unread.Count > 0) {
					foreach (Google.Apis.Gmail.v1.Data.Thread thread in unread) {
						ModifyThreadRequest request = new ModifyThreadRequest();
						request.RemoveLabelIds = new List<string>() { "UNREAD" };
						await this.service.Users.Threads.Modify(request, "me", thread.Id).ExecuteAsync();
					}

					// gets the "inbox" label
					this.inbox = await this.service.Users.Labels.Get("me", "INBOX").ExecuteAsync();

					// updates the statistics
					this.AsyncStatistics();
				}

				// restores the default systray icon and text
				notifyIcon.Icon = Resources.normal;
				notifyIcon.Text = translation.noMessage;

				// restores the default tag
				notifyIcon.Tag = null;

				// disables the mark as read menu item
				menuItemMarkAsRead.Enabled = false;
			} catch (Exception exception) {

				// enabled the mark as read menu item
				menuItemMarkAsRead.Enabled = true;

				// displays a balloon tip in the systray with the detailed error message
				notifyIcon.Icon = Resources.warning;
				notifyIcon.Text = translation.markAsReadError;
				notifyIcon.ShowBalloonTip(1500, translation.error, translation.markAsReadErrorOccured + exception.Message, ToolTipIcon.Warning);
			}
		}

		/// <summary>
		/// Manages the RunAtWindowsStartup user setting
		/// </summary>
		private void fieldRunAtWindowsStartup_CheckedChanged(object sender, EventArgs e) {
			if (fieldRunAtWindowsStartup.Checked) {
				using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true)) {
					key.SetValue("Gmail notifier", '"' + Application.ExecutablePath + '"');
				}
			} else {
				using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true)) {
					key.DeleteValue("Gmail notifier", false);
				}
			}
		}

		/// <summary>
		/// Manages the Language user setting
		/// </summary>
		private void fieldLanguage_SelectionChangeCommitted(object sender, EventArgs e) {

			// discard changes if the user select the current application language
			if (fieldLanguage.Text == Settings.Default.Language) {
				return;
			}

			// sets the new application language
			Settings.Default.Language = fieldLanguage.Text;

			// gets the current systemthreading culture
			string culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;

			// indicates to the user that to apply the new language on the interface, the application must be restarted
			bool changes = !((culture == "en-US" && fieldLanguage.Text == "English") || (culture == "fr-FR" && fieldLanguage.Text == "Français") || (culture == "de-DE" && fieldLanguage.Text == "Deutsch"));

			labelRestartToApply.Visible = changes;
			linkRestartToApply.Visible = changes;
		}

		/// <summary>
		/// Manages the SpamNotification user setting
		/// </summary>
		private void fieldSpamNotification_Click(object sender, EventArgs e) {
			this.AsyncSyncInbox();
		}

		/// <summary>
		/// Manages the NumericDelay user setting
		/// </summary>
		private void fieldNumericDelay_ValueChanged(object sender, EventArgs e) {
			Settings.Default.TimerInterval = 1000 * (fieldStepDelay.SelectedIndex == 0 ? 60 : 3600) * Convert.ToInt32(fieldNumericDelay.Value);
			Settings.Default.NumericDelay = fieldNumericDelay.Value;
			timer.Interval = Settings.Default.TimerInterval;
		}

		/// <summary>
		/// Manages the StepDelay user setting
		/// </summary>
		private void fieldStepDelay_SelectionChangeCommitted(object sender, EventArgs e) {
			Settings.Default.TimerInterval = 1000 * (fieldStepDelay.SelectedIndex == 0 ? 60 : 3600) * Convert.ToInt32(fieldNumericDelay.Value);
			Settings.Default.StepDelay = fieldStepDelay.SelectedIndex;
			timer.Interval = Settings.Default.TimerInterval;
		}

		/// <summary>
		/// Manages the NotificationBehavior user setting
		/// </summary>
		private void fieldNotificationBehavior_SelectionChangeCommitted(object sender, EventArgs e) {
			Settings.Default.NotificationBehavior = fieldNotificationBehavior.SelectedIndex;
		}

		/// <summary>
		/// Manages the PrivacyNotificationNone user setting
		/// </summary>
		private void fieldPrivacyNotificationNone_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.PrivacyNotification = (int)Privacy.None;
			pictureBoxPrivacyPreview.Image = Resources.privacy_none;
		}

		/// <summary>
		/// Manages the PrivacyNotificationShort user setting
		/// </summary>
		private void fieldPrivacyNotificationShort_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.PrivacyNotification = (int)Privacy.Short;
			pictureBoxPrivacyPreview.Image = Resources.privacy_short;
		}

		/// <summary>
		/// Manages the PrivacyNotificationAll user setting
		/// </summary>
		private void fieldPrivacyNotificationAll_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.PrivacyNotification = (int)Privacy.All;
			pictureBoxPrivacyPreview.Image = Resources.privacy_all;
		}

		/// <summary>
		/// Manages the UpdatePeriod user setting
		/// </summary>
		private void fieldUpdatePeriod_SelectedIndexChanged(object sender, EventArgs e) {
			Settings.Default.UpdatePeriod = fieldUpdatePeriod.SelectedIndex;
		}

		/// <summary>
		/// Opens the Github release section of the current build
		/// </summary>
		private void linkVersion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(GITHUB_REPOSITORY + "/releases/tag/" + this.version);
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
		/// Opens the Softpedia website
		/// </summary>
		private void linkSoftpedia_Click(object sender, EventArgs e) {
			Process.Start("http://www.softpedia.com/get/Internet/E-mail/Mail-Utilities/xavierfoucrier-Gmail-notifier.shtml#status");
		}

		/// <summary>
		/// Opens the Github license file
		/// </summary>
		private void linkLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(GITHUB_REPOSITORY + "/blob/master/LICENSE.md");
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
			Visible = false;
		}

		/// <summary>
		/// Closes the preferences when the Escape key is pressed
		/// </summary>
		private void Main_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape) {
				labelSettingsSaved.Visible = false;
				WindowState = FormWindowState.Minimized;
				ShowInTaskbar = false;
				Visible = false;
				return;
			}
		}

		/// <summary>
		/// Manages the context menu New message item
		/// </summary>
		private void menuItemNewMessage_Click(object sender, EventArgs e) {
			Process.Start(GMAIL_BASEURL + "/#inbox?compose=new");
		}

		/// <summary>
		/// Manages the context menu Synchronize item
		/// </summary>
		private void menuItemSynchronize_Click(object sender, EventArgs e) {
			this.AsyncSyncInbox();
		}

		/// <summary>
		/// Manages the context menu MarkAsRead item
		/// </summary>
		private void menuItemMarkAsRead_Click(object sender, EventArgs e) {
			this.AsyncMarkAsRead();
		}

		/// <summary>
		/// Delays the inbox sync during a certain time
		/// </summary>
		/// <param name="item">Item selected in the menu</param>
		/// <param name="delay">Delay until the next inbox sync, 0 means "infinite" timeout</param>
		private void Timeout(MenuItem item, int delay) {

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

			// disables the timer if the delay is set to "infinite"
			timer.Enabled = delay != 0;

			// applies "1" if the delay is set to "infinite" because the timer delay attribute does not support "0"
			timer.Interval = delay != 0 ? delay : 1;

			// restores the default tag
			notifyIcon.Tag = null;

			// updates the systray icon and text
			if (delay != Settings.Default.TimerInterval) {
				notifyIcon.Icon = Resources.timeout;
				notifyIcon.Text = translation.timeout + " - " + (delay != 0 ? DateTime.Now.AddMilliseconds(delay).ToShortTimeString() : "∞");
			} else {
				this.AsyncSyncInbox();
			}
		}

		/// <summary>
		/// Manages the context menu TimeoutDisabled item
		/// </summary>
		private void menuItemTimeoutDisabled_Click(object sender, EventArgs e) {
			Timeout((MenuItem)sender, Settings.Default.TimerInterval);
		}

		/// <summary>
		/// Manages the context menu Timeout30m item
		/// </summary>
		private void menuItemTimeout30m_Click(object sender, EventArgs e) {
			Timeout((MenuItem)sender, 1000 * 60 * 30);
		}

		/// <summary>
		/// Manages the context menu Timeout1h item
		/// </summary>
		private void menuItemTimeout1h_Click(object sender, EventArgs e) {
			Timeout((MenuItem)sender, 1000 * 60 * 60);
		}

		/// <summary>
		/// Manages the context menu Timeout2h item
		/// </summary>
		private void menuItemTimeout2h_Click(object sender, EventArgs e) {
			Timeout((MenuItem)sender, 1000 * 60 * 60 * 2);
		}

		/// <summary>
		/// Manages the context menu Timeout5h item
		/// </summary>
		private void menuItemTimeout5h_Click(object sender, EventArgs e) {
			Timeout((MenuItem)sender, 1000 * 60 * 60 * 5);
		}

		/// <summary>
		/// Manages the context menu TimeoutIndefinitely item
		/// </summary>
		private void menuItemTimeoutIndefinitely_Click(object sender, EventArgs e) {
			Timeout((MenuItem)sender, 0);
		}

		/// <summary>
		/// Manages the context menu Settings item
		/// </summary>
		private void menuItemSettings_Click(object sender, EventArgs e) {
			Visible = true;
			ShowInTaskbar = true;
			WindowState = FormWindowState.Normal;
			Focus();
		}

		/// <summary>
		/// Manages the context menu exit item
		/// </summary>
		private void menuItemExit_Click(object sender, EventArgs e) {
			this.Close();
		}

		/// <summary>
		/// Manages the systray icon double click
		/// </summary>
		private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {

				// by default, always open the gmail inbox in a browser
				if (notifyIcon.Tag == null) {
					Process.Start(GMAIL_BASEURL + "/#inbox");
				} else {
					notifyIconInteraction();
				}
			}
		}

		/// <summary>
		/// Manages the systray icon balloon click
		/// </summary>
		private void notifyIcon_BalloonTipClicked(object sender, EventArgs e) {
			if ((Control.MouseButtons & MouseButtons.Right) == MouseButtons.Right) {
				return;
			}

			notifyIconInteraction(true);
		}

		/// <summary>
		/// Do the gmail specified action (inbox/message/spam) in a browser
		/// </summary>
		/// <param name="systray">Defines if the interaction is provided by the balloon tip</param>
		private void notifyIconInteraction(bool balloon = false) {
			if (notifyIcon.Tag == null || (balloon && Settings.Default.NotificationBehavior == 0)) {
				return;
			}

			// opens a browser
			Process.Start(GMAIL_BASEURL + "/" + notifyIcon.Tag);
			notifyIcon.Tag = null;

			// restores the default systray icon and text: pretends that the user had read all his mail, except if the timeout option is activated
			if (timer.Interval == Settings.Default.TimerInterval) {
				notifyIcon.Icon = Resources.normal;
				notifyIcon.Text = translation.noMessage;

				// disables the mark as read menu item
				menuItemMarkAsRead.Enabled = false;
			}
		}

		/// <summary>
		/// Synchronizes the user mailbox on every timer tick
		/// </summary>
		private void timer_Tick(object sender, EventArgs e) {

			// restores the timer interval when the timeout time has elapsed
			if (timer.Interval != Settings.Default.TimerInterval) {
				Timeout(menuItemTimeoutDisabled, Settings.Default.TimerInterval);

				return;
			}

			// synchronizes the inbox
			this.AsyncSyncInbox(true);
		}

		/// <summary>
		/// Disconnects the Gmail account from the application
		/// </summary>
		private void buttonGmailDisconnect_Click(object sender, EventArgs e) {

			// asks the user for disconnect
			DialogResult dialog = MessageBox.Show(translation.gmailDisconnectQuestion.Replace("{account_name}", labelEmailAddress.Text), translation.gmailDisconnect, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

			if (dialog == DialogResult.No) {
				return;
			}

			// deletes the local application data folder and the client token file
			if (Directory.Exists(this.appdata)) {
				Directory.Delete(this.appdata, true);
			}

			// restarts the application
			this.restart();
		}

		/// <summary>
		/// Restarts the application to apply new user settings
		/// </summary>
		private void linkRestartToApply_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			this.restart();
		}

		/// <summary>
		/// Restarts the application
		/// </summary>
		private void restart() {

			// starts a new process
			ProcessStartInfo command = new ProcessStartInfo("cmd.exe", "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"");
			command.WindowStyle = ProcessWindowStyle.Hidden;
			command.CreateNoWindow = true;
			Process.Start(command);

			// exits the application
			Application.Exit();
		}

		// attempts to reconnect the user mailbox
		private void timerReconnect_Tick(object sender, EventArgs e) {

			// increases the number of reconnection attempt
			this.reconnect++;

			// bypass the first reconnection attempt because the last synchronization have already checked the internet connectivity
			if (this.reconnect == 1) {

				// sets the reconnection interval
				timerReconnect.Interval = INTERVAL_RECONNECT * 1000;

				// disables the menu items
				menuItemSynchronize.Enabled = false;
				menuItemTimout.Enabled = false;
				menuItemSettings.Enabled = false;

				// displays the reconnection attempt message on the icon
				notifyIcon.Icon = Resources.retry;
				notifyIcon.Text = translation.reconnectAttempt;

				return;
			}

			// if internet is down, waits for INTERVAL_RECONNECT seconds before next attempt
			if (!this.IsInternetAvailable()) {

				// after max unsuccessull reconnection attempts, the application waits for the next sync
				if (this.reconnect == MAX_AUTO_RECONNECT) {
					timerReconnect.Enabled = false;
					timerReconnect.Interval = 100;
					timer.Enabled = true;

					// activates the necessary menu items to allow the user to manually sync the inbox
					menuItemSynchronize.Enabled = true;

					// displays the last reconnection message on the icon
					notifyIcon.Icon = Resources.warning;
					notifyIcon.Text = translation.reconnectFailed;
				}
			} else {

				// restores default operation when internet is available
				timerReconnect.Enabled = false;
				timerReconnect.Interval = 100;
				timer.Enabled = true;

				// syncs the user mailbox
				this.AsyncSyncInbox();
			}
		}

		/// <summary>
		/// Check for update
		/// </summary>
		private void buttonCheckForUpdate_Click(object sender, EventArgs e) {
			buttonCheckForUpdate.Enabled = false;
			this.AsyncCheckForUpdate();
		}

		/// <summary>
		/// Check for update
		/// </summary>
		private void linkCheckForUpdate_Click(object sender, EventArgs e) {
			linkCheckForUpdate.Image = Resources.update_hourglass;
			linkCheckForUpdate.Enabled = false;
			Cursor.Current = DefaultCursor;
			this.AsyncCheckForUpdate();
		}

		/// <summary>
		/// Asynchronous method to connect to the repository and check if there is an update available
		/// </summary>
		/// <param name="verbose">Indicates if the process displays a message when a new update package is available</param>
		/// <param name="startup">Indicates if the update check process has been started at startup</param>
		private async void AsyncCheckForUpdate(bool verbose = true, bool startup = false) {
			try {

				// gets the list of tags in the Github repository tags webpage
				HttpResponseMessage response = await this.http.GetAsync(GITHUB_REPOSITORY + "/tags");

				var document = new HtmlAgilityPack.HtmlDocument();
				document.LoadHtml(await response.Content.ReadAsStringAsync());

				HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//span[@class='tag-name']");

				if (collection == null || collection.Count == 0) {
					return;
				}

				List<string> tags = collection.Select(p => p.InnerText).ToList();
				string release = tags.First();

				// the current version tag is not at the top of the list
				if (release != this.version) {
					
					// downloads the update package automatically or asks the user, depending on the user setting and verbosity
					if (verbose) {
						DialogResult dialog = MessageBox.Show(translation.newVersion.Replace("{version}", tags[0]), "Gmail Notifier Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

						if (dialog == DialogResult.Yes) {
							this.downloadUpdate(release);
						}
					} else if (Settings.Default.UpdateDownload) {
						this.downloadUpdate(release);
					}
				} else if (verbose && !startup) {
					MessageBox.Show(translation.latestVersion, "Gmail Notifier Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			} catch (Exception) {

				// indicates to the user that the update service is not reachable for the moment
				if (verbose) {
					MessageBox.Show(translation.updateServiceUnreachable, "Gmail Notifier Update", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			} finally {

				// restores default check icon and check for update button state
				linkCheckForUpdate.Enabled = true;
				linkCheckForUpdate.Image = Resources.update_check;
				buttonCheckForUpdate.Enabled = true;

				// stores the latest update datetime control
				Settings.Default.UpdateControl = DateTime.Now;

				// updates the update control label
				labelUpdateControl.Text = Settings.Default.UpdateControl.ToString();

				// synchronizes the inbox if the updates has been checked at startup after asynchronous authentication
				if (startup) {
					this.AsyncSyncInbox();
				}
			}
		}

		/// <summary>
		/// Downloads and launch the setup installer
		/// </summary>
		/// <param name="release">Version number package to download</param>
		private void downloadUpdate(string release) {

			// defines that the application is currently updating
			this.updating = true;

			// defines the new number version and temp path
			string newversion = release.Split('-')[0].Substring(1);
			string updatepath = this.appdata + "/gmnupdate-" + newversion + ".exe";
			string package = GITHUB_REPOSITORY + "/releases/download/" + release + "/Gmail.Notifier." + newversion + ".exe";

			try {

				// disables the context menu and displays the update icon in the systray
				notifyIcon.ContextMenu = null;
				notifyIcon.Icon = Resources.updating;
				notifyIcon.Text = translation.updating;

				// creates a new web client instance
				WebClient client = new WebClient();

				// displays the download progression on the systray icon, and prevents the application from restoring the context menu and systray icon at startup
				client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((object o, DownloadProgressChangedEventArgs target) => {
					notifyIcon.ContextMenu = null;
					notifyIcon.Icon = Resources.updating;
					notifyIcon.Text = translation.updating + " " + target.ProgressPercentage.ToString() + "%";
				});

				// starts the setup installer when the download has complete and exits the current application
				client.DownloadFileCompleted += new AsyncCompletedEventHandler((object o, AsyncCompletedEventArgs target) => {
					Process.Start(new ProcessStartInfo(updatepath, Settings.Default.UpdateQuiet ? "/verysilent" : ""));
					Application.Exit();
				});

				// ensures that the Github package URI is callable
				client.OpenRead(package).Close();

				// starts the download of the new version from the Github repository
				client.DownloadFileAsync(new Uri(package), updatepath);
			} catch (Exception) {

				// indicates to the user that the update service is not reachable for the moment
				MessageBox.Show(translation.updateServiceUnreachable, "Gmail Notifier Update", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				// defines that the application has exited the updating state
				this.updating = false;

				// restores the context menu to the systray icon and start a synchronization
				notifyIcon.ContextMenu = contextMenu;
				this.AsyncSyncInbox();
			}
		}
	}
}
