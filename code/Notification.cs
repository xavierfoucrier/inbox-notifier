using System;
using System.Diagnostics;
using System.Windows.Forms;
using notifier.Languages;
using notifier.Properties;

namespace notifier {
	class Notification {

		#region #attributes

		/// <summary>
		/// Privacy possibilities
		/// </summary>
		public enum Privacy : uint {
			None = 0,
			Short = 1,
			All = 2
		}

		/// <summary>
		/// Type possibilities
		/// </summary>
		public enum Type : uint {
			None = 0,
			Info = 1,
			Warning = 2,
			Error = 3
		}

		/// <summary>
		/// Behavior possibilities
		/// </summary>
		public enum Behavior : uint {
			DoNothing = 0,
			Open = 1
		}

		/// <summary>
		/// Reference to the main interface
		/// </summary>
		private Main UI;

		#endregion

		#region #methods

		/// <summary>
		/// Class constructor
		/// </summary>
		public Notification(ref Main form) {
			UI = form;
		}

		/// <summary>
		/// Displays a notification balloon tip
		/// </summary>
		/// <param name="title">Title of the tip</param>
		/// <param name="text">Text contained in the balloon tip</param>
		/// <param name="icon">Icon of the tip</param>
		/// <param name="duration">How long the notification is displayed</param>
		public void Tip(string title, string text, Type icon = Type.Info, int duration = 450) {
			UI.notifyIcon.ShowBalloonTip(duration, title, text, (ToolTipIcon)icon);
		}

		/// <summary>
		/// Do the gmail specified action (inbox/message/spam) in a browser
		/// </summary>
		/// <param name="systray">Defines if the interaction is provided by the balloon tip</param>
		public void Interaction(bool balloon = false) {

			// by default, always open the gmail inbox in a browser if the interaction is provided by a double click on the systray icon
			if (Tag == null) {

				if (!balloon) {
					Process.Start(Settings.Default.GMAIL_BASEURL + "/#inbox");
				}

				return;
			}

			// do nothing if there is no tag or if the notification behavior is set to "do nothing"
			if (balloon && Settings.Default.NotificationBehavior == (int)Behavior.DoNothing) {
				return;
			}

			// opens a browser
			Process.Start(Settings.Default.GMAIL_BASEURL + "/" + Tag);

			// cleans the tag
			Tag = null;

			// restores the default systray icon and text: pretends that the user had read all his mail, except if the timeout option is activated
			if (!Paused) {

				// updates the synchronization time
				UI.GmailService.Inbox.Time = DateTime.Now;

				// restores the default systray icon and text
				UI.notifyIcon.Icon = Resources.normal;
				UI.notifyIcon.Text = Translation.noMessage + "\n" + Translation.syncTime.Replace("{time}", DateTime.Now.ToLongTimeString());

				// disables the mark as read menu item
				UI.menuItemMarkAsRead.Text = Translation.markAsRead;
				UI.menuItemMarkAsRead.Enabled = false;
			}
		}

		/// <summary>
		/// Pauses the notifications during a certain time
		/// </summary>
		/// <param name="item">Selected item in the timeout menu</param>
		/// <param name="delay">Delay until the next inbox sync, 0 means "infinite" timeout</param>
		public void Pause(MenuItem item, int delay) {

			// exits if the selected menu item is already checked
			if (item.Checked) {
				return;
			}

			// disables notifications
			Paused = true;

			// cleans the tag
			Tag = null;

			// unchecks all menu items
			foreach (MenuItem i in UI.menuItemTimout.MenuItems) {
				i.Checked = false;
			}

			// displays the user choice
			item.Checked = true;

			// infinite variable
			bool infinite = delay == 0;

			// disables the timer if the delay is set to "infinite"
			UI.timer.Enabled = !infinite;

			// applies "1" if the delay is set to "infinite" because the timer interval attribute does not support "0"
			UI.timer.Interval = infinite ? 1 : delay;

			// updates the systray icon and text
			UI.notifyIcon.Icon = Resources.timeout;
			UI.notifyIcon.Text = Translation.timeout + " - " + (infinite ? "∞" : DateTime.Now.AddMilliseconds(delay).ToShortTimeString());
		}

		/// <summary>
		/// Resumes notifications
		/// </summary>
		public void Resume() {

			// exits if the selected menu item is already checked
			if (UI.menuItemTimeoutDisabled.Checked) {
				return;
			}

			// enables notifications
			Paused = false;

			// unchecks all menu items
			foreach (MenuItem i in UI.menuItemTimout.MenuItems) {
				i.Checked = false;
			}

			// displays the user choice
			UI.menuItemTimeoutDisabled.Checked = true;

			// restores the timer interval
			UI.timer.Interval = Settings.Default.TimerInterval;

			// enables the timer: this will automatically trigger the inbox synchronization in the timer tick
			UI.timer.Enabled = true;

			// synchronizes the inbox
			UI.GmailService.Inbox.Sync();
		}

		#endregion

		#region #accessors

		/// <summary>
		/// Notification tag to allow the user to directly display the specified view (inbox/message/spam) in a browser
		/// </summary>
		/// <returns>The current notification tag</returns>
		public string Tag
		{
			get; set;
		}

		/// <summary>
		/// Timeout mode
		/// </summary>
		/// <returns>Indicates if the notification service is paused</returns>
		public bool Paused
		{
			get; set;
		}

		#endregion
	}
}
