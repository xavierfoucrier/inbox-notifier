using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using notifier.Languages;
using notifier.Properties;

namespace notifier {
	public partial class Main : Form {

		/// <summary>
		/// Update service class
		/// </summary>
		internal Update UpdateService;

		/// <summary>
		/// Computer service class
		/// </summary>
		internal Computer ComputerService;

		/// <summary>
		/// Gmail service class
		/// </summary>
		internal Gmail GmailService;

		/// <summary>
		/// Gmail service class
		/// </summary>
		internal Notification NotificationService;

		/// <summary>
		/// Scheduler service class
		/// </summary>
		internal Scheduler SchedulerService;

		/// <summary>
		/// Global UI tooltip
		/// </summary>
		internal ToolTip tip = new ToolTip();

		/// <summary>
		/// Initialize the class
		/// </summary>
		public Main() {
			InitializeComponent();

			// main application instance
			Main ui = this;

			// initialize services
			UpdateService = new Update(ref ui);
			ComputerService = new Computer(ref ui);
			GmailService = new Gmail(ref ui);
			NotificationService = new Notification(ref ui);
			SchedulerService = new Scheduler(ref ui);
		}

		/// <summary>
		/// Load the form
		/// </summary>
		private void Main_Load(object sender, EventArgs e) {

			// play a pop sound at application startup
			if (Settings.Default.AudioPop) {
				using (SoundPlayer player = new SoundPlayer(Resources.pop_open)) {
					player.Play();
				}
			}

			// hide the form by default
			Visible = false;

			// upgrade the user configuration if necessary
			if (Settings.Default.UpdateRequired) {
				Settings.Default.Upgrade();

				// switch the update required state
				Settings.Default.UpdateRequired = false;
				Settings.Default.Save();

				// delete the setup installer package from the previous upgrade
				UpdateService.DeleteSetupPackage();
			}

			// display a systray notification on first load
			if (Settings.Default.FirstLoad && !Directory.Exists(Core.ApplicationDataFolder)) {
				NotificationService.Tip(Translation.welcome, Translation.firstLoad, Notification.Type.Info, 7000);

				// switch the first load state
				Settings.Default.FirstLoad = false;
				Settings.Default.Save();

				// wait for 7 seconds to complete the thread
				System.Threading.Thread.Sleep(7000);
			}

			// configure the help provider
			HelpProvider help = new HelpProvider();
			help.SetHelpString(fieldRunAtWindowsStartup, Translation.helpRunAtWindowsStartup);
			help.SetHelpString(fieldAskonExit, Translation.helpAskonExit);
			help.SetHelpString(fieldLanguage, Translation.helpLanguage);
			help.SetHelpString(labelEmailAddress, Translation.helpEmailAddress);
			help.SetHelpString(labelTokenDelivery, Translation.helpTokenDelivery);
			help.SetHelpString(buttonGmailDisconnect, Translation.helpGmailDisconnect);
			help.SetHelpString(chartUnreadMails, Translation.helpStatistics);
			help.SetHelpString(chartTotalMails, Translation.helpStatistics);
			help.SetHelpString(chartInbox, Translation.helpStatistics);
			help.SetHelpString(labelTotalDrafts, Translation.helpStatistics);
			help.SetHelpString(labelTotalLabels, Translation.helpStatistics);
			help.SetHelpString(fieldMessageNotification, Translation.helpMessageNotification);
			help.SetHelpString(fieldAudioNotification, Translation.helpAudioNotification);
			help.SetHelpString(fieldSpamNotification, Translation.helpSpamNotification);
			help.SetHelpString(fieldNumericDelay, Translation.helpNumericDelay);
			help.SetHelpString(fieldStepDelay, Translation.helpStepDelay);
			help.SetHelpString(fieldNotificationBehavior, Translation.helpNotificationBehavior);
			help.SetHelpString(fieldPrivacyNotificationNone, Translation.helpPrivacyNotificationNone);
			help.SetHelpString(fieldPrivacyNotificationShort, Translation.helpPrivacyNotificationShort);
			help.SetHelpString(fieldPrivacyNotificationAll, Translation.helpPrivacyNotificationAll);
			help.SetHelpString(fieldScheduler, Translation.helpScheduleService);
			help.SetHelpString(fieldDayOfWeek, Translation.helpScheduleDayOfWeek);
			help.SetHelpString(labelDuration, Translation.helpScheduleDuration);
			help.SetHelpString(fieldStartTime, Translation.helpScheduleStartTime);
			help.SetHelpString(fieldEndTime, Translation.helpScheduleEndTime);
			help.SetHelpString(fieldUpdateService, Translation.helpUpdateService);
			help.SetHelpString(fieldUpdateDownload, Translation.helpUpdateDownload);
			help.SetHelpString(fieldUpdateQuiet, Translation.helpUpdateQuiet);
			help.SetHelpString(fieldUpdatePeriod, Translation.helpUpdatePeriod);
			help.SetHelpString(labelUpdateControl, Translation.helpUpdateControl);
			help.SetHelpString(buttonCheckForUpdate, Translation.helpCheckForUpdate);

			// authenticate the user
			GmailService.Authentication();

			// attach the context menu to the systray icon
			notifyIcon.ContextMenu = notifyMenu;

			// bind the "PropertyChanged" event of the settings to automatically save the user settings and display the setting label
			Settings.Default.PropertyChanged += new PropertyChangedEventHandler((object source, PropertyChangedEventArgs target) => {
				Settings.Default.Save();

				if (target.PropertyName != "UpdateControl") {
					labelSettingsSaved.Visible = true;
				}
			});

			// bind all computer services
			ComputerService.BindNetwork();
			ComputerService.BindPowerMode();
			ComputerService.BindSessionSwitch();

			// display the notification labels
			labelNotificationOpenMessage.Visible = Settings.Default.NotificationBehavior == 1;
			labelNotificationMarkMessageAsRead.Visible = Settings.Default.NotificationBehavior == 2;

			// display the step delay setting
			fieldStepDelay.SelectedIndex = (int)Settings.Default.StepDelay;

			// display the notification behavior setting
			fieldNotificationBehavior.SelectedIndex = (int)Settings.Default.NotificationBehavior;

			// display the privacy notification setting
			switch (Settings.Default.PrivacyNotification) {
				case (int)Notification.Privacy.None:
					fieldPrivacyNotificationNone.Checked = true;
					pictureBoxPrivacyPreview.Image = Resources.privacy_none;
					break;
				default:
				case (int)Notification.Privacy.Short:
					fieldPrivacyNotificationShort.Checked = true;
					pictureBoxPrivacyPreview.Image = Resources.privacy_short;
					break;
				case (int)Notification.Privacy.All:
					fieldPrivacyNotificationAll.Checked = true;
					pictureBoxPrivacyPreview.Image = Resources.privacy_all;
					break;
			}

			// display the update period setting
			fieldUpdatePeriod.SelectedIndex = (int)Settings.Default.UpdatePeriod;

			// display the update control setting
			labelUpdateControl.Text = Settings.Default.UpdateControl.ToString();

			// display the product version
			linkVersion.Text = Core.Version.Substring(1);

			// display a tooltip for the product version
			tip.SetToolTip(linkVersion, Settings.Default.GITHUB_REPOSITORY + "/releases/tag/" + Core.Version);

			// display a tooltip for the license link
			tip.SetToolTip(linkPrivacy, Settings.Default.GITHUB_REPOSITORY + "/blob/master/PRIVACY.md");

			// display a tooltip for the website link
			tip.SetToolTip(linkWebsiteYusuke, Settings.Default.SITE_YUSUKE);

			// display a tooltip for the website link
			tip.SetToolTip(linkWebsiteXavier, Settings.Default.SITE_AUTHOR);

			// display a tooltip for the license link
			tip.SetToolTip(linkLicense, Settings.Default.GITHUB_REPOSITORY + "/blob/master/LICENSE.md");
		}

		/// <summary>
		/// Prompt the user before closing the form
		/// </summary>
		private void Main_FormClosing(object sender, FormClosingEventArgs e) {

			// hide the form to the systray if closed by the user
			if (e.CloseReason == CloseReason.UserClosing) {
				labelSettingsSaved.Visible = false;
				WindowState = FormWindowState.Minimized;
				ShowInTaskbar = false;
				Visible = false;
				e.Cancel = true;

				return;
			}

			// dispose the gmail service
			GmailService.Dispose();

			// play a pop sound at application exit
			if (Settings.Default.AudioPop) {
				using (SoundPlayer player = new SoundPlayer(Resources.pop_exit)) {
					player.PlaySync();
				}
			}
		}

		/// <summary>
		/// Manage the RunAtWindowsStartup user setting
		/// </summary>
		private void fieldRunAtWindowsStartup_CheckedChanged(object sender, EventArgs e) {
			ComputerService.SetApplicationStartup(fieldRunAtWindowsStartup.Checked ? Computer.Registration.On : Computer.Registration.Off);
		}

		/// <summary>
		/// Manage the Language user setting
		/// </summary>
		private void fieldLanguage_SelectionChangeCommitted(object sender, EventArgs e) {

			// discard changes if the user select the current application language
			if (fieldLanguage.Text == Settings.Default.Language) {
				return;
			}

			// set the new application language
			Settings.Default.Language = fieldLanguage.Text;

			// get the current system threading culture
			string culture = Thread.CurrentThread.CurrentUICulture.Name;

			// indicate to the user that to apply the new language on the interface, the application must be restarted
			bool changes = !((culture == "en-US" && fieldLanguage.Text == "English") || (culture == "fr-FR" && fieldLanguage.Text == "Français") || (culture == "de-DE" && fieldLanguage.Text == "Deutsch"));

			labelRestartToApply.Visible = changes;
			linkRestartToApply.Visible = changes;
		}

		/// <summary>
		/// Manage the SpamNotification user setting
		/// </summary>
		private void fieldSpamNotification_Click(object sender, EventArgs e) {
			GmailService.Inbox.Sync();
		}

		/// <summary>
		/// Manage the NumericDelay user setting
		/// </summary>
		private void fieldNumericDelay_ValueChanged(object sender, EventArgs e) {
			Settings.Default.TimerInterval = 1000 * (fieldStepDelay.SelectedIndex == 0 ? 60 : 3600) * (int)fieldNumericDelay.Value;
			Settings.Default.NumericDelay = fieldNumericDelay.Value;
			timer.Interval = (int)Settings.Default.TimerInterval;
		}

		/// <summary>
		/// Manage the StepDelay user setting
		/// </summary>
		private void fieldStepDelay_SelectionChangeCommitted(object sender, EventArgs e) {
			Settings.Default.TimerInterval = 1000 * (fieldStepDelay.SelectedIndex == 0 ? 60 : 3600) * (int)fieldNumericDelay.Value;
			Settings.Default.StepDelay = (uint)fieldStepDelay.SelectedIndex;
			timer.Interval = (int)Settings.Default.TimerInterval;
		}

		/// <summary>
		/// Manage the NotificationBehavior user setting
		/// </summary>
		private void fieldNotificationBehavior_SelectionChangeCommitted(object sender, EventArgs e) {
			Settings.Default.NotificationBehavior = (uint)fieldNotificationBehavior.SelectedIndex;
			labelNotificationOpenMessage.Visible = Settings.Default.NotificationBehavior == 1;
			labelNotificationMarkMessageAsRead.Visible = Settings.Default.NotificationBehavior == 2;
		}

		/// <summary>
		/// Manage the PrivacyNotificationNone user setting
		/// </summary>
		private void fieldPrivacyNotificationNone_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.PrivacyNotification = (int)Notification.Privacy.None;
			pictureBoxPrivacyPreview.Image = Resources.privacy_none;
		}

		/// <summary>
		/// Manage the PrivacyNotificationShort user setting
		/// </summary>
		private void fieldPrivacyNotificationShort_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.PrivacyNotification = (int)Notification.Privacy.Short;
			pictureBoxPrivacyPreview.Image = Resources.privacy_short;
		}

		/// <summary>
		/// Manage the PrivacyNotificationAll user setting
		/// </summary>
		private void fieldPrivacyNotificationAll_CheckedChanged(object sender, EventArgs e) {
			Settings.Default.PrivacyNotification = (int)Notification.Privacy.All;
			pictureBoxPrivacyPreview.Image = Resources.privacy_all;
		}

		/// <summary>
		/// Manage the UpdatePeriod user setting
		/// </summary>
		private void fieldUpdatePeriod_SelectedIndexChanged(object sender, EventArgs e) {
			Settings.Default.UpdatePeriod = (uint)fieldUpdatePeriod.SelectedIndex;
		}

		/// <summary>
		/// Open the Github release section of the current build
		/// </summary>
		private void linkVersion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(Settings.Default.GITHUB_REPOSITORY + "/releases/tag/" + Core.Version);
		}

		/// <summary>
		/// Open the Github privacy notice file
		/// </summary>
		private void linkPrivacy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(Settings.Default.GITHUB_REPOSITORY + "/blob/master/PRIVACY.md");
		}

		/// <summary>
		/// Open the Yusuke website
		/// </summary>
		private void linkWebsiteYusuke_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(Settings.Default.SITE_YUSUKE);
		}

		/// <summary>
		/// Open the Xavier website
		/// </summary>
		private void linkWebsiteXavier_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(Settings.Default.SITE_AUTHOR);
		}

		/// <summary>
		/// Open the Github license file
		/// </summary>
		private void linkLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(Settings.Default.GITHUB_REPOSITORY + "/blob/master/LICENSE.md");
		}
		
		/// <summary>
		/// Hide the settings saved label
		/// </summary>
		private void tabControl_Selecting(object sender, TabControlCancelEventArgs e) {
			labelSettingsSaved.Visible = false;
		}

		/// <summary>
		/// Close the preferences when the OK button is clicked
		/// </summary>
		private void buttonOK_Click(object sender, EventArgs e) {
			labelSettingsSaved.Visible = false;
			WindowState = FormWindowState.Minimized;
			ShowInTaskbar = false;
			Visible = false;
		}

		/// <summary>
		/// Close the preferences when the Escape key is pressed
		/// </summary>
		private void main_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode != Keys.Escape) {
				return;
			}

			labelSettingsSaved.Visible = false;
			WindowState = FormWindowState.Minimized;
			ShowInTaskbar = false;
			Visible = false;
		}

		/// <summary>
		/// Manage the context menu New message item
		/// </summary>
		private void menuItemNewMessage_Click(object sender, EventArgs e) {
			Process.Start(Settings.Default.GMAIL_BASEURL + "/#inbox?compose=new");
		}

		/// <summary>
		/// Manage the context menu Synchronize item
		/// </summary>
		private void menuItemSynchronize_Click(object sender, EventArgs e) {
			GmailService.Inbox.Sync();
		}

		/// <summary>
		/// Manage the context menu MarkAsRead item
		/// </summary>
		private void menuItemMarkAsRead_Click(object sender, EventArgs e) {
			GmailService.Inbox.MarkAsRead();
		}

		/// <summary>
		/// Manage the context menu TimeoutDisabled item
		/// </summary>
		private void menuItemTimeoutDisabled_Click(object sender, EventArgs e) {
			NotificationService.Resume();
		}

		/// <summary>
		/// Manage the context menu Timeout30m item
		/// </summary>
		private void menuItemTimeout30m_Click(object sender, EventArgs e) {
			NotificationService.Pause((MenuItem)sender, 1000 * 60 * 30);
		}

		/// <summary>
		/// Manage the context menu Timeout1h item
		/// </summary>
		private void menuItemTimeout1h_Click(object sender, EventArgs e) {
			NotificationService.Pause((MenuItem)sender, 1000 * 60 * 60);
		}

		/// <summary>
		/// Manage the context menu Timeout2h item
		/// </summary>
		private void menuItemTimeout2h_Click(object sender, EventArgs e) {
			NotificationService.Pause((MenuItem)sender, 1000 * 60 * 60 * 2);
		}

		/// <summary>
		/// Manage the context menu Timeout5h item
		/// </summary>
		private void menuItemTimeout5h_Click(object sender, EventArgs e) {
			NotificationService.Pause((MenuItem)sender, 1000 * 60 * 60 * 5);
		}

		/// <summary>
		/// Manage the context menu TimeoutIndefinitely item
		/// </summary>
		private void menuItemTimeoutIndefinitely_Click(object sender, EventArgs e) {
			NotificationService.Pause((MenuItem)sender, 0);
		}

		/// <summary>
		/// Manage the context menu Settings item
		/// </summary>
		private void menuItemSettings_Click(object sender, EventArgs e) {

			// reset the settings label visibility
			labelSettingsSaved.Visible = false;

			// check the start with Windows setting against the registry
			if (tabControl.SelectedTab == tabPageGeneral) {
				ComputerService.RegulatesRegistry();
			}

			// display the form
			Visible = true;
			ShowInTaskbar = true;
			WindowState = FormWindowState.Normal;
			Focus();
		}

		/// <summary>
		/// Manage the context menu exit item
		/// </summary>
		private void menuItemExit_Click(object sender, EventArgs e) {

			// ask the user for exit, depending on the application settings
			if (Settings.Default.AskonExit) {
				DialogResult dialog = MessageBox.Show(Translation.applicationExitQuestion, Translation.applicationExit, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

				if (dialog == DialogResult.No) {
					return;
				}
			}

			Application.Exit();
		}

		/// <summary>
		/// Manage the systray icon double click
		/// </summary>
		private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				NotificationService.Interaction();
			}
		}

		/// <summary>
		/// Manage the systray icon balloon click
		/// </summary>
		private void notifyIcon_BalloonTipClicked(object sender, EventArgs e) {
			if ((Control.MouseButtons & MouseButtons.Right) == MouseButtons.Right) {
				return;
			}

			NotificationService.Interaction(true);
		}

		/// <summary>
		/// Synchronize the user mailbox on every timer tick
		/// </summary>
		private void timer_Tick(object sender, EventArgs e) {

			// restore the timer interval when the timeout time has elapsed
			if (NotificationService.Paused) {
				NotificationService.Resume();

				return;
			}

			// synchronize the inbox
			GmailService.Inbox.Sync(false);
		}

		/// <summary>
		/// Disconnect the Gmail account from the application
		/// </summary>
		private void buttonGmailDisconnect_Click(object sender, EventArgs e) {

			// ask the user for disconnect
			DialogResult dialog = MessageBox.Show(Translation.gmailDisconnectQuestion.Replace("{account_name}", labelEmailAddress.Text), Translation.gmailDisconnect, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

			if (dialog == DialogResult.No) {
				return;
			}

			// delete the local application data folder and the client token file
			if (Directory.Exists(Core.ApplicationDataFolder)) {
				Directory.Delete(Core.ApplicationDataFolder, true);
			}

			// restart the application
			Core.RestartApplication();
		}

		/// <summary>
		/// Restart the application to apply new user settings
		/// </summary>
		private void linkRestartToApply_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Core.RestartApplication();
		}

		// attempt to reconnect the user mailbox
		private void timerReconnect_Tick(object sender, EventArgs e) {
			GmailService.Inbox.Retry();
		}

		/// <summary>
		/// Check for update
		/// </summary>
		private void buttonCheckForUpdate_Click(object sender, EventArgs e) {
			buttonCheckForUpdate.Enabled = false;

			if (UpdateService.UpdateAvailable) {
				WindowState = FormWindowState.Minimized;
				ShowInTaskbar = false;
				Visible = false;
				UpdateService.Download();
			} else {
				UpdateService.Check();
			}
		}

		/// <summary>
		/// Check the start with Windows setting against the registry when entering the general tab page
		/// </summary>
		private void tabPageGeneral_Enter(object sender, EventArgs e) {
			ComputerService.RegulatesRegistry();
		}

		/// <summary>
		/// Manage the DayOfWeek user setting
		/// </summary>
		private void fieldDayOfWeek_SelectionChangeCommitted(object sender, EventArgs e) {
			SchedulerService.DisplayTimeSlotProperties(SchedulerService.GetTimeSlot(SchedulerService.GetDayOfWeek(fieldDayOfWeek.SelectedIndex)));
		}

		/// <summary>
		/// Hide the settings saved label
		/// </summary>
		private void fieldDayOfWeek_SelectedIndexChanged(object sender, EventArgs e) {
			labelSettingsSaved.Visible = false;
		}

		/// <summary>
		/// Manage the fieldStartTime user setting
		/// </summary>
		private void fieldStartTime_SelectionChangeCommitted(object sender, EventArgs e) {
			SchedulerService.Update(Scheduler.TimeType.Start);
		}

		/// <summary>
		/// Manage the fieldEndTime user setting
		/// </summary>
		private void fieldEndTime_SelectionChangeCommitted(object sender, EventArgs e) {
			SchedulerService.Update(Scheduler.TimeType.End);
		}

		/// <summary>
		/// Synchronize the inbox if the scheduler is enable or disable and if the selected day of week is today
		/// </summary>
		private void fieldScheduler_Click(object sender, EventArgs e) {
			if (SchedulerService.GetDayOfWeek(fieldDayOfWeek.SelectedIndex) == DateTime.Now.DayOfWeek) {
				GmailService.Inbox.Sync();
			}
		}
	}
}