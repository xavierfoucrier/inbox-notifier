using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
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

		// update service class
		private Update UpdateService;

		/// <summary>
		/// Initializes the class
		/// </summary>
		public Main() {
			InitializeComponent();

			// main application instance
			var Instance = this;

			// initializes services
			UpdateService = new Update(ref Instance);
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

				// deletes the setup installer package from the previous upgrade
				UpdateService.DeleteSetupPackage();
			}

			// displays a systray notification on first load
			if (Settings.Default.FirstLoad && !Directory.Exists(Core.GetApplicationDataFolder())) {
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

					// do nothing if the timeout mode is set to infinite
					if (timer.Interval != Settings.Default.TimerInterval && menuItemTimeoutIndefinitely.Checked) {
						return;
					}

					this.AsyncSyncInbox(false, true);
				}
			});

			// bins the "SessionSwitch" event to automatically sync the inbox on session unlocking
			SystemEvents.SessionSwitch += new SessionSwitchEventHandler((object o, SessionSwitchEventArgs target) => {

				// syncs the inbox when the user is unlocking the Windows session
				if (target.Reason == SessionSwitchReason.SessionUnlock) {

					// do nothing if the timeout mode is set to infinite
					if (timer.Interval != Settings.Default.TimerInterval && menuItemTimeoutIndefinitely.Checked) {
						return;
					}

					AsyncSyncInbox(false, true);
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
			linkVersion.Text = Core.GetVersion().Substring(1);

			// positioning the check for update link
			linkCheckForUpdate.Left = linkVersion.Right + 2;

			// displays a tooltip for the product version
			ToolTip tipTag = new ToolTip();
			tipTag.SetToolTip(linkVersion, Settings.Default.GITHUB_REPOSITORY + "/releases/tag/" + Core.GetVersion());
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
			tipLicense.SetToolTip(linkLicense, Settings.Default.GITHUB_REPOSITORY + "/blob/master/LICENSE.md");
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
				labelTokenDelivery.Text = this.credential.Token.IssuedUtc.ToLocalTime().ToString();
			} catch(Exception) {

				// exits the application if the google api token file doesn't exists
				if (!Directory.Exists(Core.GetApplicationDataFolder()) || !Directory.EnumerateFiles(Core.GetApplicationDataFolder()).Any()) {

					// displays the authentication icon and title
					notifyIcon.Icon = Resources.authentication;
					notifyIcon.Text = translation.authenticationFailed;

					// exits the application
					MessageBox.Show(translation.authenticationWithGmailRefused, translation.authenticationFailed, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					Application.Exit();
				}
			} finally {

				// synchronizes the user mailbox, after checking for update depending on the user settings, or by default after the asynchronous authentication
				if (Settings.Default.UpdateService && UpdateService.IsPeriodSetToStartup()) {
					UpdateService.Check(!Settings.Default.UpdateDownload, true);
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

		/// <summary>
		/// Asynchronous method used to refresh the authentication token
		/// </summary>
		/// <returns></returns>
		private async Task<bool> AsyncRefreshToken() {

			// refreshes the token and updates the token delivery date and time on the interface
			if (await this.credential.RefreshTokenAsync(new CancellationToken())) {
				labelTokenDelivery.Text = this.credential.Token.IssuedUtc.ToLocalTime().ToString();
			}

			return true;
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
		public async void AsyncSyncInbox(bool timertick = false, bool token = false) {
			
			// prevents the application from syncing the inbox when updating
			if (UpdateService.IsUpdating()) {
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

			// refreshes the authentication token if needed
			if (token) {
				await this.AsyncRefreshToken();
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

			// do a small ping on the update service
			UpdateService.Ping();

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
					notifyIcon.Icon = this.inbox.ThreadsUnread <= Settings.Default.UNSTACK_BOUNDARY ? Resources.mails : Resources.stack;

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
					menuItemMarkAsRead.Text = translation.markAsRead + " (" + this.inbox.ThreadsUnread + ")";
					menuItemMarkAsRead.Enabled = true;
				} else {

					// restores the default systray icon and text
					notifyIcon.Icon = Resources.normal;
					notifyIcon.Text = translation.noMessage;

					// disables the mark as read menu item
					menuItemMarkAsRead.Text = translation.markAsRead;
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

				// updates the synchronization time
				this.synctime = DateTime.Now;

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
						ModifyThreadRequest request = new ModifyThreadRequest() {
							RemoveLabelIds = new List<string>() { "UNREAD" }
						};

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
				menuItemMarkAsRead.Text = translation.markAsRead;
				menuItemMarkAsRead.Enabled = false;
			} catch (Exception exception) {

				// enabled the mark as read menu item
				menuItemMarkAsRead.Text = translation.markAsRead + " (" + this.inbox.ThreadsUnread + ")";
				menuItemMarkAsRead.Enabled = true;

				// displays a balloon tip in the systray with the detailed error message
				notifyIcon.Icon = Resources.warning;
				notifyIcon.Text = translation.markAsReadError;
				notifyIcon.ShowBalloonTip(1500, translation.error, translation.markAsReadErrorOccured + exception.Message, ToolTipIcon.Warning);
			} finally {
				notifyIcon.Text = notifyIcon.Text.Split('\n')[0] + "\n" + translation.syncTime.Replace("{time}", this.synctime.ToLongTimeString());
			}
		}

		/// <summary>
		/// Manages the RunAtWindowsStartup user setting
		/// </summary>
		private void FieldRunAtWindowsStartup_CheckedChanged(object sender, EventArgs e) {
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
		private void FieldLanguage_SelectionChangeCommitted(object sender, EventArgs e) {

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
		private void FieldSpamNotification_Click(object sender, EventArgs e) {
			this.AsyncSyncInbox();
		}

		/// <summary>
		/// Manages the NumericDelay user setting
		/// </summary>
		private void FieldNumericDelay_ValueChanged(object sender, EventArgs e) {
			Settings.Default.TimerInterval = 1000 * (fieldStepDelay.SelectedIndex == 0 ? 60 : 3600) * Convert.ToInt32(fieldNumericDelay.Value);
			Settings.Default.NumericDelay = fieldNumericDelay.Value;
			timer.Interval = Settings.Default.TimerInterval;
		}

		/// <summary>
		/// Manages the StepDelay user setting
		/// </summary>
		private void FieldStepDelay_SelectionChangeCommitted(object sender, EventArgs e) {
			Settings.Default.TimerInterval = 1000 * (fieldStepDelay.SelectedIndex == 0 ? 60 : 3600) * Convert.ToInt32(fieldNumericDelay.Value);
			Settings.Default.StepDelay = fieldStepDelay.SelectedIndex;
			timer.Interval = Settings.Default.TimerInterval;
		}

		/// <summary>
		/// Manages the NotificationBehavior user setting
		/// </summary>
		private void FieldNotificationBehavior_SelectionChangeCommitted(object sender, EventArgs e) {
			Settings.Default.NotificationBehavior = fieldNotificationBehavior.SelectedIndex;
		}

		/// <summary>
		/// Manages the PrivacyNotificationNone user setting
		/// </summary>
		private void FieldPrivacyNotificationNone_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.PrivacyNotification = (int)Privacy.None;
			pictureBoxPrivacyPreview.Image = Resources.privacy_none;
		}

		/// <summary>
		/// Manages the PrivacyNotificationShort user setting
		/// </summary>
		private void FieldPrivacyNotificationShort_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.PrivacyNotification = (int)Privacy.Short;
			pictureBoxPrivacyPreview.Image = Resources.privacy_short;
		}

		/// <summary>
		/// Manages the PrivacyNotificationAll user setting
		/// </summary>
		private void FieldPrivacyNotificationAll_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.PrivacyNotification = (int)Privacy.All;
			pictureBoxPrivacyPreview.Image = Resources.privacy_all;
		}

		/// <summary>
		/// Manages the UpdatePeriod user setting
		/// </summary>
		private void FieldUpdatePeriod_SelectedIndexChanged(object sender, EventArgs e) {
			Settings.Default.UpdatePeriod = fieldUpdatePeriod.SelectedIndex;
		}

		/// <summary>
		/// Opens the Github release section of the current build
		/// </summary>
		private void LinkVersion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(Settings.Default.GITHUB_REPOSITORY + "/releases/tag/" + Core.GetVersion());
		}

		/// <summary>
		/// Opens the Yusuke website
		/// </summary>
		private void LinkWebsiteYusuke_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("http://p.yusukekamiyamane.com");
		}

		/// <summary>
		/// Opens the Xavier website
		/// </summary>
		private void LinkWebsiteXavier_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("http://www.xavierfoucrier.fr");
		}

		/// <summary>
		/// Opens the Softpedia website
		/// </summary>
		private void LinkSoftpedia_Click(object sender, EventArgs e) {
			Process.Start("http://www.softpedia.com/get/Internet/E-mail/Mail-Utilities/xavierfoucrier-Gmail-notifier.shtml#status");
		}

		/// <summary>
		/// Opens the Github license file
		/// </summary>
		private void LinkLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(Settings.Default.GITHUB_REPOSITORY + "/blob/master/LICENSE.md");
		}

		/// <summary>
		/// Hides the settings saved label
		/// </summary>
		private void TabControl_Selecting(object sender, TabControlCancelEventArgs e) {
			labelSettingsSaved.Visible = false;
		}

		/// <summary>
		/// Closes the preferences when the OK button is clicked
		/// </summary>
		private void ButtonOK_Click(object sender, EventArgs e) {
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
		private void MenuItemNewMessage_Click(object sender, EventArgs e) {
			Process.Start(Settings.Default.GMAIL_BASEURL + "/#inbox?compose=new");
		}

		/// <summary>
		/// Manages the context menu Synchronize item
		/// </summary>
		private void MenuItemSynchronize_Click(object sender, EventArgs e) {
			this.AsyncSyncInbox();
		}

		/// <summary>
		/// Manages the context menu MarkAsRead item
		/// </summary>
		private void MenuItemMarkAsRead_Click(object sender, EventArgs e) {
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
		private void MenuItemTimeoutDisabled_Click(object sender, EventArgs e) {
			Timeout((MenuItem)sender, Settings.Default.TimerInterval);
		}

		/// <summary>
		/// Manages the context menu Timeout30m item
		/// </summary>
		private void MenuItemTimeout30m_Click(object sender, EventArgs e) {
			Timeout((MenuItem)sender, 1000 * 60 * 30);
		}

		/// <summary>
		/// Manages the context menu Timeout1h item
		/// </summary>
		private void MenuItemTimeout1h_Click(object sender, EventArgs e) {
			Timeout((MenuItem)sender, 1000 * 60 * 60);
		}

		/// <summary>
		/// Manages the context menu Timeout2h item
		/// </summary>
		private void MenuItemTimeout2h_Click(object sender, EventArgs e) {
			Timeout((MenuItem)sender, 1000 * 60 * 60 * 2);
		}

		/// <summary>
		/// Manages the context menu Timeout5h item
		/// </summary>
		private void MenuItemTimeout5h_Click(object sender, EventArgs e) {
			Timeout((MenuItem)sender, 1000 * 60 * 60 * 5);
		}

		/// <summary>
		/// Manages the context menu TimeoutIndefinitely item
		/// </summary>
		private void MenuItemTimeoutIndefinitely_Click(object sender, EventArgs e) {
			Timeout((MenuItem)sender, 0);
		}

		/// <summary>
		/// Manages the context menu Settings item
		/// </summary>
		private void MenuItemSettings_Click(object sender, EventArgs e) {

			// resets the settings label visibility
			labelSettingsSaved.Visible = false;

			// displays the form
			Visible = true;
			ShowInTaskbar = true;
			WindowState = FormWindowState.Normal;
			Focus();
		}

		/// <summary>
		/// Manages the context menu exit item
		/// </summary>
		private void MenuItemExit_Click(object sender, EventArgs e) {
			this.Close();
		}

		/// <summary>
		/// Manages the systray icon double click
		/// </summary>
		private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {

				// by default, always open the gmail inbox in a browser
				if (notifyIcon.Tag == null) {
					Process.Start(Settings.Default.GMAIL_BASEURL + "/#inbox");
				} else {
					NotifyIconInteraction();
				}
			}
		}

		/// <summary>
		/// Manages the systray icon balloon click
		/// </summary>
		private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e) {
			if ((Control.MouseButtons & MouseButtons.Right) == MouseButtons.Right) {
				return;
			}

			NotifyIconInteraction(true);
		}

		/// <summary>
		/// Do the gmail specified action (inbox/message/spam) in a browser
		/// </summary>
		/// <param name="systray">Defines if the interaction is provided by the balloon tip</param>
		private void NotifyIconInteraction(bool balloon = false) {

			// do nothing if there is no tag or if the notification behavior is set to "do nothing"
			if (notifyIcon.Tag == null || (balloon && Settings.Default.NotificationBehavior == 0)) {
				return;
			}

			// opens a browser
			Process.Start(Settings.Default.GMAIL_BASEURL + "/" + notifyIcon.Tag);
			notifyIcon.Tag = null;

			// restores the default systray icon and text: pretends that the user had read all his mail, except if the timeout option is activated
			if (timer.Interval == Settings.Default.TimerInterval) {

				// updates the synchronization time
				this.synctime = DateTime.Now;

				// restores the default systray icon and text
				notifyIcon.Icon = Resources.normal;
				notifyIcon.Text = translation.noMessage + "\n" + translation.syncTime.Replace("{time}", this.synctime.ToLongTimeString());

				// disables the mark as read menu item
				menuItemMarkAsRead.Text = translation.markAsRead;
				menuItemMarkAsRead.Enabled = false;
			}
		}

		/// <summary>
		/// Synchronizes the user mailbox on every timer tick
		/// </summary>
		private void Timer_Tick(object sender, EventArgs e) {

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
		private void ButtonGmailDisconnect_Click(object sender, EventArgs e) {

			// asks the user for disconnect
			DialogResult dialog = MessageBox.Show(translation.gmailDisconnectQuestion.Replace("{account_name}", labelEmailAddress.Text), translation.gmailDisconnect, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

			if (dialog == DialogResult.No) {
				return;
			}

			// deletes the local application data folder and the client token file
			if (Directory.Exists(Core.GetApplicationDataFolder())) {
				Directory.Delete(Core.GetApplicationDataFolder(), true);
			}

			// restarts the application
			this.Restart();
		}

		/// <summary>
		/// Restarts the application to apply new user settings
		/// </summary>
		private void LinkRestartToApply_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			this.Restart();
		}

		/// <summary>
		/// Restarts the application
		/// </summary>
		private void Restart() {

			// starts a new process
			ProcessStartInfo command = new ProcessStartInfo("cmd.exe", "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"") {
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true
			};

			Process.Start(command);

			// exits the application
			Application.Exit();
		}

		// attempts to reconnect the user mailbox
		private void TimerReconnect_Tick(object sender, EventArgs e) {

			// increases the number of reconnection attempt
			this.reconnect++;

			// bypass the first reconnection attempt because the last synchronization have already checked the internet connectivity
			if (this.reconnect == 1) {

				// sets the reconnection interval
				timerReconnect.Interval = Settings.Default.INTERVAL_RECONNECT * 1000;

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
				if (this.reconnect == Settings.Default.MAX_AUTO_RECONNECT) {
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
		private void ButtonCheckForUpdate_Click(object sender, EventArgs e) {
			buttonCheckForUpdate.Enabled = false;
			UpdateService.Check();
		}

		/// <summary>
		/// Check for update
		/// </summary>
		private void LinkCheckForUpdate_Click(object sender, EventArgs e) {
			linkCheckForUpdate.Image = Resources.update_hourglass;
			linkCheckForUpdate.Enabled = false;
			Cursor.Current = DefaultCursor;
			UpdateService.Check();
		}
	}
}
