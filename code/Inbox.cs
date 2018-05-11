﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Media;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using notifier.Languages;
using notifier.Properties;

namespace notifier {
	class Inbox {

		/// <summary>
		/// Gmail api service
		/// </summary>
		private GmailService Api;

		/// <summary>
		/// Gmail address
		/// </summary>
		private string EmailAddress;

		/// <summary>
		/// Main inbox label
		/// </summary>
		private Google.Apis.Gmail.v1.Data.Label Box;

		/// <summary>
		/// Number of automatic reconnection
		/// </summary>
		private uint Reconnect = 0;

		/// <summary>
		/// Unread threads
		/// </summary>
		private int? UnreadThreads = 0;

		/// <summary>
		/// Last synchronization time
		/// </summary>
		private DateTime SyncTime = DateTime.Now;

		/// <summary>
		/// Reference to the main interface
		/// </summary>
		private Main UI;

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="form">Reference to the application main window</param>
		/// <param name="credential">User credential given by the authorization broker</param>
		public Inbox(ref Main form, ref UserCredential credential) {
			UI = form;

			// initializes the gmail service base client api
			Api = new GmailService(new BaseClientService.Initializer() {
				HttpClientInitializer = credential,
				ApplicationName = "Gmail notifier for Windows"
			});

			// retrieves the gmail address
			EmailAddress = Api.Users.GetProfile("me").Execute().EmailAddress;
		}

		/// <summary>
		/// Asynchronous method used to synchronize the user inbox
		/// </summary>
		/// <param name="timertick">Indicates if the synchronization come's from the timer tick or has been manually triggered</param>
		public async void Sync(bool timertick = false, bool token = false) {

			// prevents the application from syncing the inbox when updating
			if (UI.UpdateService.IsUpdating()) {
				return;
			}

			// updates the synchronization time
			SyncTime = DateTime.Now;

			// resets reconnection count and prevents the application from displaying continuous warning icon when a timertick synchronization occurs after a reconnection attempt
			if (Reconnect != 0) {
				timertick = false;
				Reconnect = 0;
			}

			// disables the timeout when the user do a manual synchronization
			if (UI.timer.Interval != Settings.Default.TimerInterval) {
				Timeout(UI.menuItemTimeoutDisabled, Settings.Default.TimerInterval);

				// exits the method because the timeout function automatically restarts a synchronization once it has been disabled
				return;
			}

			// if internet is down, attempts to reconnect the user mailbox
			if (!UI.ComputerService.IsInternetAvailable()) {
				UI.timerReconnect.Enabled = true;
				UI.timer.Enabled = false;

				return;
			}

			// refreshes the token if needed
			if (token) {
				await UI.GmailService.RefreshToken();
			}

			// activates the necessary menu items
			UI.menuItemSynchronize.Enabled = true;
			UI.menuItemTimout.Enabled = true;
			UI.menuItemSettings.Enabled = true;

			// displays the sync icon, but not on every tick of the timer
			if (!timertick) {
				UI.notifyIcon.Icon = Resources.sync;
				UI.notifyIcon.Text = Translation.sync;
			}

			// do a small ping on the update service
			UI.UpdateService.Ping();

			try {

				// manages the spam notification
				if (Settings.Default.SpamNotification) {

					// exits if a spam is already detected
					if (timertick && UI.notifyIcon.Tag != null && UI.notifyIcon.Tag.ToString() == "#spam") {
						return;
					}

					// gets the "spam" label
					Google.Apis.Gmail.v1.Data.Label spam = await Api.Users.Labels.Get("me", "SPAM").ExecuteAsync();

					// manages unread spams
					if (spam.ThreadsUnread > 0) {

						// sets the notification icon and text
						UI.notifyIcon.Icon = Resources.spam;

						// plays a sound on unread spams
						if (Settings.Default.AudioNotification) {
							SystemSounds.Exclamation.Play();
						}

						// displays a balloon tip in the systray with the total of unread threads
						UI.notifyIcon.ShowBalloonTip(450, spam.ThreadsUnread.ToString() + " " + (spam.ThreadsUnread > 1 ? Translation.unreadSpams : Translation.unreadSpam), Translation.newUnreadSpam, ToolTipIcon.Error);
						UI.notifyIcon.Text = spam.ThreadsUnread.ToString() + " " + (spam.ThreadsUnread > 1 ? Translation.unreadSpams : Translation.unreadSpam);
						UI.notifyIcon.Tag = "#spam";

						return;
					}
				}

				// gets the "inbox" label
				Box = await Api.Users.Labels.Get("me", "INBOX").ExecuteAsync();

				// updates the statistics
				UpdateStatistics();

				// exits the sync if the number of unread threads is the same as before
				if (timertick && (Box.ThreadsUnread == UnreadThreads)) {
					return;
				}

				// manages unread threads
				if (Box.ThreadsUnread > 0) {

					// sets the notification icon
					UI.notifyIcon.Icon = Box.ThreadsUnread <= Settings.Default.UNSTACK_BOUNDARY ? Resources.mails : Resources.stack;

					// manages message notification
					if (Settings.Default.MessageNotification) {

						// plays a sound on unread threads
						if (Settings.Default.AudioNotification) {
							SystemSounds.Asterisk.Play();
						}

						// gets the message details
						UsersResource.MessagesResource.ListRequest messages = Api.Users.Messages.List("me");
						messages.LabelIds = "UNREAD";
						messages.MaxResults = 1;
						Google.Apis.Gmail.v1.Data.Message message = await Api.Users.Messages.Get("me", await messages.ExecuteAsync().ContinueWith(m => {
							return m.Result.Messages.First().Id;
						})).ExecuteAsync();

						//  displays a balloon tip in the systray with the total of unread threads and message details, depending on the user privacy setting
						if (Box.ThreadsUnread == 1 && Settings.Default.PrivacyNotification != (int)Notification.Privacy.All) {
							string subject = "";
							string from = "";

							foreach (MessagePartHeader header in message.Payload.Headers) {
								if (header.Name == "Subject") {
									subject = header.Value != "" ? header.Value : Translation.newUnreadMessage;
								} else if (header.Name == "From") {
									Match match = Regex.Match(header.Value, ".* <");

									if (match.Length != 0) {
										from = match.Captures[0].Value.Replace(" <", "").Replace("\"", "");
									} else {
										match = Regex.Match(header.Value, "<?.*>?");
										from = match.Length != 0 ? match.Value.ToLower().Replace("<", "").Replace(">", "") : header.Value.Replace(match.Value, Box.ThreadsUnread.ToString() + " " + Translation.unreadMessage);
									}
								}
							}

							if (Settings.Default.PrivacyNotification == (int)Notification.Privacy.None) {
								UI.notifyIcon.ShowBalloonTip(450, from, message.Snippet != "" ? WebUtility.HtmlDecode(message.Snippet) : Translation.newUnreadMessage, ToolTipIcon.Info);
							} else if (Settings.Default.PrivacyNotification == (int)Notification.Privacy.Short) {
								UI.notifyIcon.ShowBalloonTip(450, from, subject, ToolTipIcon.Info);
							}
						} else {
							UI.notifyIcon.ShowBalloonTip(450, Box.ThreadsUnread.ToString() + " " + (Box.ThreadsUnread > 1 ? Translation.unreadMessages : Translation.unreadMessage), Translation.newUnreadMessage, ToolTipIcon.Info);
						}

						// manages the balloon tip click event handler: we store the "notification tag" to allow the user to directly display the specified view (inbox/message/spam) in a browser
						UI.notifyIcon.Tag = "#inbox" + (Box.ThreadsUnread == 1 ? "/" + message.Id : "");
					}

					// displays the notification text
					UI.notifyIcon.Text = Box.ThreadsUnread.ToString() + " " + (Box.ThreadsUnread > 1 ? Translation.unreadMessages : Translation.unreadMessage);

					// enables the mark as read menu item
					UI.menuItemMarkAsRead.Text = Translation.markAsRead + " (" + Box.ThreadsUnread + ")";
					UI.menuItemMarkAsRead.Enabled = true;
				} else {

					// restores the default systray icon and text
					UI.notifyIcon.Icon = Resources.normal;
					UI.notifyIcon.Text = Translation.noMessage;

					// disables the mark as read menu item
					UI.menuItemMarkAsRead.Text = Translation.markAsRead;
					UI.menuItemMarkAsRead.Enabled = false;
				}

				// saves the number of unread threads
				UnreadThreads = Box.ThreadsUnread;
			} catch (Exception exception) {

				// displays a balloon tip in the systray with the detailed error message
				UI.notifyIcon.Icon = Resources.warning;
				UI.notifyIcon.Text = Translation.syncError;
				UI.notifyIcon.ShowBalloonTip(1500, Translation.error, Translation.syncErrorOccured + exception.Message, ToolTipIcon.Warning);
			} finally {
				UI.notifyIcon.Text = UI.notifyIcon.Text.Split('\n')[0] + "\n" + Translation.syncTime.Replace("{time}", SyncTime.ToLongTimeString());
			}
		}

		/// <summary>
		/// Asynchronous method used to mark as read the user inbox
		/// </summary>
		public async void MarkAsRead() {
			try {

				// updates the synchronization time
				SyncTime = DateTime.Now;

				// displays the sync icon
				UI.notifyIcon.Icon = Resources.sync;
				UI.notifyIcon.Text = Translation.sync;

				// gets all unread threads
				UsersResource.ThreadsResource.ListRequest threads = Api.Users.Threads.List("me");
				threads.LabelIds = "UNREAD";
				ListThreadsResponse list = await threads.ExecuteAsync();
				IList<Thread> unread = list.Threads;

				// loops through all unread threads and removes the "unread" label for each one
				if (unread != null && unread.Count > 0) {
					foreach (Thread thread in unread) {
						ModifyThreadRequest request = new ModifyThreadRequest() {
							RemoveLabelIds = new List<string>() { "UNREAD" }
						};

						await Api.Users.Threads.Modify(request, "me", thread.Id).ExecuteAsync();
					}

					// gets the "inbox" label
					Box = await Api.Users.Labels.Get("me", "INBOX").ExecuteAsync();

					// updates the statistics
					UpdateStatistics();
				}

				// restores the default systray icon and text
				UI.notifyIcon.Icon = Resources.normal;
				UI.notifyIcon.Text = Translation.noMessage;

				// restores the default tag
				UI.notifyIcon.Tag = null;

				// disables the mark as read menu item
				UI.menuItemMarkAsRead.Text = Translation.markAsRead;
				UI.menuItemMarkAsRead.Enabled = false;
			} catch (Exception exception) {

				// enabled the mark as read menu item
				UI.menuItemMarkAsRead.Text = Translation.markAsRead + " (" + Box.ThreadsUnread + ")";
				UI.menuItemMarkAsRead.Enabled = true;

				// displays a balloon tip in the systray with the detailed error message
				UI.notifyIcon.Icon = Resources.warning;
				UI.notifyIcon.Text = Translation.markAsReadError;
				UI.notifyIcon.ShowBalloonTip(1500, Translation.error, Translation.markAsReadErrorOccured + exception.Message, ToolTipIcon.Warning);
			} finally {
				UI.notifyIcon.Text = UI.notifyIcon.Text.Split('\n')[0] + "\n" + Translation.syncTime.Replace("{time}", SyncTime.ToLongTimeString());
			}
		}

		/// <summary>
		/// Delays the inbox sync during a certain time
		/// </summary>
		/// <param name="item">Item selected in the menu</param>
		/// <param name="delay">Delay until the next inbox sync, 0 means "infinite" timeout</param>
		public void Timeout(MenuItem item, int delay) {

			// exits if the selected menu item is already checked
			if (item.Checked) {
				return;
			}

			// unchecks all menu items
			foreach (MenuItem i in UI.menuItemTimout.MenuItems) {
				i.Checked = false;
			}

			// displays the user choice
			item.Checked = true;

			// disables the timer if the delay is set to "infinite"
			UI.timer.Enabled = delay != 0;

			// applies "1" if the delay is set to "infinite" because the timer delay attribute does not support "0"
			UI.timer.Interval = delay != 0 ? delay : 1;

			// restores the default tag
			UI.notifyIcon.Tag = null;

			// updates the systray icon and text
			if (delay != Settings.Default.TimerInterval) {
				UI.notifyIcon.Icon = Resources.timeout;
				UI.notifyIcon.Text = Translation.timeout + " - " + (delay != 0 ? DateTime.Now.AddMilliseconds(delay).ToShortTimeString() : "∞");
			} else {
				Sync();
			}
		}

		/// <summary>
		/// Retries to reconnect the inbox
		/// </summary>
		public void Retry() {

			// increases the number of reconnection attempt
			Reconnect++;

			// bypass the first reconnection attempt because the last synchronization have already checked the internet connectivity
			if (Reconnect == 1) {

				// sets the reconnection interval
				UI.timerReconnect.Interval = Settings.Default.INTERVAL_RECONNECT * 1000;

				// disables the menu items
				UI.menuItemSynchronize.Enabled = false;
				UI.menuItemTimout.Enabled = false;
				UI.menuItemSettings.Enabled = false;

				// displays the reconnection attempt message on the icon
				UI.notifyIcon.Icon = Resources.retry;
				UI.notifyIcon.Text = Translation.reconnectAttempt;

				return;
			}

			// if internet is down, waits for INTERVAL_RECONNECT seconds before next attempt
			if (!UI.ComputerService.IsInternetAvailable()) {

				// after max unsuccessull reconnection attempts, the application waits for the next sync
				if (Reconnect == Settings.Default.MAX_AUTO_RECONNECT) {
					UI.timerReconnect.Enabled = false;
					UI.timerReconnect.Interval = 100;
					UI.timer.Enabled = true;

					// activates the necessary menu items to allow the user to manually sync the inbox
					UI.menuItemSynchronize.Enabled = true;

					// displays the last reconnection message on the icon
					UI.notifyIcon.Icon = Resources.warning;
					UI.notifyIcon.Text = Translation.reconnectFailed;
				}
			} else {

				// restores default operation when internet is available
				UI.timerReconnect.Enabled = false;
				UI.timerReconnect.Interval = 100;
				UI.timer.Enabled = true;

				// syncs the user mailbox
				Sync();
			}
		}

		/// <summary>
		/// Disposes the gmail api
		/// </summary>
		public void Dispose() {
			if (Api != null) {
				Api.Dispose();
			}
		}

		/// <summary>
		/// Gets the gmail email address
		/// </summary>
		/// <returns>Gmail email address</returns>
		public string GetEmailAddress() {
			return EmailAddress;
		}

		/// <summary>
		/// Gets the number of automatic reconnection to the network
		/// </summary>
		/// <returns>The number of automatic reconnection to the network</returns>
		public uint GetReconnect() {
			return Reconnect;
		}

		/// <summary>
		/// Sets the number of automatic reconnection to the network
		/// </summary>
		/// <param name="reconnection">Number of automatic reconnection</param>
		public void SetReconnect(uint reconnection) {
			Reconnect = reconnection;
		}
		
		/// <summary>
		/// Gets the last synchronization time
		/// </summary>
		/// <returns>Last synchronization time</returns>
		public DateTime GetSyncTime() {
			return SyncTime;
		}
		
		/// <summary>
		/// Sets the last synchronization time
		/// </summary>
		/// <param name="time">Last synchronization time</param>
		public void SetSyncTime(DateTime time) {
			SyncTime = time;
		}

		/// <summary>
		/// Asynchronous method used to get account statistics
		/// </summary>
		private async void UpdateStatistics() {
			try {
				UI.labelTotalUnreadMails.Text = Box.ThreadsUnread.ToString();
				UI.labelTotalMails.Text = Box.ThreadsTotal.ToString();

				ListDraftsResponse drafts = await Api.Users.Drafts.List("me").ExecuteAsync();
				ListLabelsResponse labels = await Api.Users.Labels.List("me").ExecuteAsync();

				if (drafts.Drafts != null) {
					UI.labelTotalDrafts.Text = drafts.Drafts.Count.ToString();
				}

				if (labels.Labels != null) {
					UI.labelTotalLabels.Text = labels.Labels.Count.ToString();
				}
			} catch (Exception) {
				// nothing to catch
			}
		}
	}
}
