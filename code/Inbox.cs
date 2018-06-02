using System;
using System.Linq;
using System.Collections.Generic;
using System.Media;
using System.Net;
using System.Text.RegularExpressions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using notifier.Languages;
using notifier.Properties;

namespace notifier {
	class Inbox {

		#region #attributes

		/// <summary>
		/// Gmail api service
		/// </summary>
		private GmailService Api;

		/// <summary>
		/// Main inbox label
		/// </summary>
		private Google.Apis.Gmail.v1.Data.Label Box;

		/// <summary>
		/// Unread threads
		/// </summary>
		private int? UnreadThreads = 0;

		/// <summary>
		/// Reference to the main interface
		/// </summary>
		private Main UI;

		#endregion

		#region #methods

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
		/// <param name="manual">Indicates if the synchronization come's from the timer tick or has been manually triggered</param>
		/// <param name="token">Indicates if the Gmail token need to be refreshed</param>
		public async void Sync(bool manual = true, bool token = false) {

			// prevents the application from syncing the inbox when updating
			if (UI.UpdateService.Updating) {
				return;
			}

			// updates the synchronization time
			Time = DateTime.Now;

			// resets reconnection count and prevents the application from displaying continuous warning icon when a timertick synchronization occurs after a reconnection attempt
			if (ReconnectionAttempts != 0) {
				manual = true;
				ReconnectionAttempts = 0;
			}

			// disables the timeout when the user do a manual synchronization
			if (manual && UI.NotificationService.Paused) {
				UI.NotificationService.Resume();

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

			// displays the sync icon, but only on manual synchronization
			if (manual) {
				UI.notifyIcon.Icon = Resources.sync;
				UI.notifyIcon.Text = Translation.sync;
			}

			// do a small ping on the update service
			UI.UpdateService.Ping();

			try {

				// manages the spam notification
				if (Settings.Default.SpamNotification) {

					// exits if a spam is already detected
					if (!manual && UI.NotificationService.Tag == "#spam") {
						return;
					}

					// gets the "spam" label
					Google.Apis.Gmail.v1.Data.Label spam = await Api.Users.Labels.Get("me", "SPAM").ExecuteAsync();

					// manages unread spams
					if (spam.ThreadsUnread > 0) {

						// plays a sound on unread spams
						if (Settings.Default.AudioNotification) {
							SystemSounds.Exclamation.Play();
						}

						// displays a balloon tip in the systray with the total of unread threads
						UI.NotificationService.Tip(spam.ThreadsUnread.ToString() + " " + (spam.ThreadsUnread > 1 ? Translation.unreadSpams : Translation.unreadSpam), Translation.newUnreadSpam, Notification.Type.Error);

						// sets the notification icon and text
						UI.notifyIcon.Icon = Resources.spam;
						UI.notifyIcon.Text = spam.ThreadsUnread.ToString() + " " + (spam.ThreadsUnread > 1 ? Translation.unreadSpams : Translation.unreadSpam);

						// updates the tag
						UI.NotificationService.Tag = "#spam";

						return;
					}
				}

				// gets the "inbox" label
				Box = await Api.Users.Labels.Get("me", "INBOX").ExecuteAsync();

				// updates the statistics
				UpdateStatistics();

				// exits the sync if the number of unread threads is the same as before
				if (!manual && (Box.ThreadsUnread == UnreadThreads)) {
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
								UI.NotificationService.Tip(from, message.Snippet != "" ? WebUtility.HtmlDecode(message.Snippet) : Translation.newUnreadMessage);

							} else if (Settings.Default.PrivacyNotification == (int)Notification.Privacy.Short) {
								UI.NotificationService.Tip(from, subject);
							}
						} else {
							UI.NotificationService.Tip(Box.ThreadsUnread.ToString() + " " + (Box.ThreadsUnread > 1 ? Translation.unreadMessages : Translation.unreadMessage), Translation.newUnreadMessage);
						}

						// updates the notification tag to allow the user to directly display the specified view (inbox/message/spam) in a browser
						UI.NotificationService.Tag = "#inbox" + (Box.ThreadsUnread == 1 ? "/" + message.Id : "");
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
				UI.NotificationService.Tip(Translation.error, Translation.syncErrorOccured + exception.Message, Notification.Type.Warning, 1500);
			} finally {
				UI.notifyIcon.Text = UI.notifyIcon.Text.Split('\n')[0] + "\n" + Translation.syncTime.Replace("{time}", Time.ToLongTimeString());
			}
		}

		/// <summary>
		/// Asynchronous method used to mark as read the user inbox
		/// </summary>
		public async void MarkAsRead() {
			try {

				// updates the synchronization time
				Time = DateTime.Now;

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

				// cleans the tag
				UI.NotificationService.Tag = null;

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
				UI.NotificationService.Tip(Translation.error, Translation.markAsReadErrorOccured + exception.Message, Notification.Type.Warning, 1500);
			} finally {
				UI.notifyIcon.Text = UI.notifyIcon.Text.Split('\n')[0] + "\n" + Translation.syncTime.Replace("{time}", Time.ToLongTimeString());
			}
		}

		/// <summary>
		/// Retries to reconnect the inbox
		/// </summary>
		public void Retry() {

			// increases the number of reconnection attempt
			ReconnectionAttempts++;

			// bypass the first reconnection attempt because the last synchronization have already checked the internet connectivity
			if (ReconnectionAttempts == 1) {

				// sets the reconnection interval
				UI.timerReconnect.Interval = (int)Settings.Default.INTERVAL_RECONNECT * 1000;

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
				if (ReconnectionAttempts == Settings.Default.MAX_AUTO_RECONNECT) {
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
		/// Asynchronous method used to get account statistics
		/// </summary>
		private async void UpdateStatistics() {
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
		}

		#endregion

		#region #accessors

		/// <summary>
		/// Last synchronization time
		/// </summary>
		public DateTime Time {
			get; set;
		} = DateTime.Now;

		/// <summary>
		/// Gmail email address
		/// </summary>
		public string EmailAddress {
			get; set;
		}

		/// <summary>
		/// Number of automatic reconnection attempts
		/// </summary>
		public uint ReconnectionAttempts {
			get; set;
		} = 0;

		#endregion
	}
}
