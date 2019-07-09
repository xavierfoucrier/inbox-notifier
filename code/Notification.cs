using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
			Open = 1,
			MarkAsRead = 2,
			OpenSimplifiedHTML = 3
		}

		/// <summary>
		/// Reference to the main interface
		/// </summary>
		private readonly Main UI;

		#endregion

		#region #methods

		/// <summary>
		/// Class constructor
		/// </summary>
		public Notification(ref Main form) {
			UI = form;
		}

		/// <summary>
		/// Display a notification balloon tip
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
		/// <param name="balloon">Define if the interaction is provided by the balloon tip</param>
		public async Task Interaction(bool balloon = false) {

			// by default, always open the gmail inbox in a browser if the interaction is provided by a double click on the systray icon
			if (Tag == null) {

				if (!balloon) {
					Process.Start(GetBaseURL() + "/#inbox");
				}

				return;
			}

			// display the form and focus the update tab
			if (balloon && Tag == "update") {
				UI.Visible = true;
				UI.ShowInTaskbar = true;
				UI.WindowState = FormWindowState.Normal;
				UI.Focus();
				UI.tabControl.SelectTab("tabPageUpdate");
				Tag = null;

				return;
			}

			// do nothing if the notification behavior is set to "do nothing"
			if (balloon && Settings.Default.NotificationBehavior == (int)Behavior.DoNothing) {
				return;
			}

			// mark the message as read if the notification behavior is set to "mark as read"
			if (balloon && Settings.Default.NotificationBehavior == (int)Behavior.MarkAsRead) {
				await UI.GmailService.Inbox.MarkAsRead();
			} else {
				Process.Start(GetBaseURL() + "/" + Tag);
			}

			// clean the tag
			Tag = null;

			// restore the default systray icon and text: pretend that the user had read all his mail, except if the timeout option is activated
			if (!Paused) {

				// reset the number of unread threads
				UI.GmailService.Inbox.UnreadThreads = 0;

				// update the synchronization time
				UI.GmailService.Inbox.Time = DateTime.Now;

				// restore the default systray icon and text
				UI.notifyIcon.Icon = Resources.normal;
				UI.notifyIcon.Text = Translation.noMessage + "\n" + Translation.syncTime.Replace("{time}", DateTime.Now.ToLongTimeString());

				// disable the mark as read menu item
				UI.menuItemMarkAsRead.Text = Translation.markAsRead;
				UI.menuItemMarkAsRead.Enabled = false;
			}
		}

		/// <summary>
		/// Pause the notifications during a certain time
		/// </summary>
		/// <param name="item">Selected item in the timeout menu</param>
		/// <param name="delay">Delay until the next inbox sync, 0 means "infinite" timeout</param>
		public void Pause(MenuItem item, int delay) {

			// exit if the selected menu item is already checked
			if (item.Checked) {
				return;
			}

			// disable notifications
			Paused = true;

			// clean the tag
			Tag = null;

			// uncheck all menu items
			foreach (MenuItem i in UI.menuItemTimout.MenuItems) {
				i.Checked = false;
			}

			// display the user choice
			item.Checked = true;

			// infinite variable
			bool infinite = delay == 0;

			// disable the timer if the delay is set to "infinite"
			UI.timer.Enabled = !infinite;

			// applie "1" if the delay is set to "infinite" because the timer interval attribute does not support "0"
			UI.timer.Interval = infinite ? 1 : delay;

			// update the systray icon and text
			UI.notifyIcon.Icon = Resources.timeout;
			UI.notifyIcon.Text = Translation.timeout + " - " + (infinite ? "∞" : DateTime.Now.AddMilliseconds(delay).ToShortTimeString());
		}

		/// <summary>
		/// Resume notifications
		/// </summary>
		public async Task Resume() {

			// exit if the selected menu item is already checked
			if (UI.menuItemTimeoutDisabled.Checked) {
				return;
			}

			// enable notifications
			Paused = false;

			// uncheck all menu items
			foreach (MenuItem i in UI.menuItemTimout.MenuItems) {
				i.Checked = false;
			}

			// display the user choice
			UI.menuItemTimeoutDisabled.Checked = true;

			// restore the timer interval
			UI.timer.Interval = (int)Settings.Default.TimerInterval;

			// enable the timer: this will automatically trigger the inbox synchronization in the timer tick
			UI.timer.Enabled = true;

			// synchronize the inbox
			await UI.GmailService.Inbox.Sync();
		}

		/// <summary>
		/// Return the Gmail base URL depending on the notification behavior
		/// </summary>
		/// <returns></returns>
		public static string GetBaseURL() {
			return Settings.Default.NotificationBehavior == (int)Behavior.OpenSimplifiedHTML ? Settings.Default.GMAIL_BASEURL + "/h" : Settings.Default.GMAIL_BASEURL;
		}

		#endregion

		#region #accessors

		/// <summary>
		/// Notification tag to allow the user to directly display the specified view (inbox/message/spam) in a browser
		/// </summary>
		public string Tag {
			get; set;
		}

		/// <summary>
		/// Timeout mode that indicates if the notification service is paused
		/// </summary>
		public bool Paused {
			get; set;
		}

		#endregion
	}
}