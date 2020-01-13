using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using notifier.Languages;
using notifier.Properties;

namespace notifier {
	class Inbox : IDisposable {

		#region #attributes

		/// <summary>
		/// Gmail api service
		/// </summary>
		private GmailService Api;

		/// <summary>
		/// Main inbox label
		/// </summary>
		private Label Box;

		/// <summary>
		/// Reference to the main interface
		/// </summary>
		private readonly Main UI;

		#endregion

		#region #methods

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="form">Reference to the application main window</param>
		public Inbox(ref Main form) {
			UI = form;

			// initialize the gmail service base client api
			InitGmailServiceApi();
		}

		/// <summary>
		/// Asynchronous method used to synchronize the user inbox
		/// </summary>
		/// <param name="manual">Indicate if the synchronization come's from the timer tick or has been manually triggered</param>
		/// <param name="token">Indicate if the Gmail token need to be refreshed</param>
		public async Task Sync(bool manual = true, bool token = false) {

			// temp variable
			bool userAction = manual;

			// prevent the application from syncing the inbox when the scheduler is enabled and the sync is not scheduled
			if (Settings.Default.Scheduler && !UI.SchedulerService.ScheduledSync()) {
				UI.SchedulerService.PauseSync();

				return;
			}

			// prevent the application from syncing the inbox when updating
			if (UI.UpdateService.Updating) {
				return;
			}

			// update the synchronization time
			Time = DateTime.Now;

			// reset reconnection count and prevent the application from displaying continuous warning icon when a timertick synchronization occurs after a reconnection attempt
			if (ReconnectionAttempts != 0) {
				userAction = true;
				ReconnectionAttempts = 0;
			}

			// disable the timeout when the user do a manual synchronization
			if (userAction && UI.NotificationService.Paused) {
				await UI.NotificationService.Resume();

				return;
			}

			// if internet is down, attempt to reconnect the user mailbox
			if (!Computer.IsInternetAvailable()) {
				UI.timerReconnect.Enabled = true;
				UI.timer.Enabled = false;

				return;
			}

			// refresh the token if needed
			if (token) {
				await UI.GmailService.RefreshToken();
			}

			// activate the necessary menu items
			UI.menuItemSynchronize.Enabled = true;
			UI.menuItemTimout.Enabled = true;
			UI.menuItemSettings.Enabled = true;

			// display the sync icon, but only on manual synchronization
			if (userAction) {
				UI.notifyIcon.Icon = Resources.sync;
				UI.notifyIcon.Text = Translation.sync;
			}

			// do a small ping on the update service
			await UI.UpdateService.Ping();

			try {

				// initialize the gmail service base client api
				InitGmailServiceApi();

				// get the "inbox" label
				Box = await Api.Users.Labels.Get("me", "INBOX").ExecuteAsync();

				// update the statistics
				await UpdateStatistics().ConfigureAwait(false);

				// manage the spam notification
				if (Settings.Default.SpamNotification) {

					// exit if a spam is already detected
					if (!userAction && UI.NotificationService.Tag == "#spam") {
						return;
					}

					// get the "spam" label
					Label spam = await Api.Users.Labels.Get("me", "SPAM").ExecuteAsync();

					// manage unread spams
					if (spam.ThreadsUnread > 0) {

						// play a sound on unread spams
						if (Settings.Default.AudioNotification) {
							SystemSounds.Exclamation.Play();
						}

						// display a balloon tip in the systray with the total of unread threads
						UI.NotificationService.Tip(spam.ThreadsUnread.ToString() + " " + (spam.ThreadsUnread > 1 ? Translation.unreadSpams : Translation.unreadSpam), Translation.newUnreadSpam, Notification.Type.Error);

						// set the notification icon and text
						UI.notifyIcon.Icon = Resources.spam;
						UI.notifyIcon.Text = spam.ThreadsUnread.ToString() + " " + (spam.ThreadsUnread > 1 ? Translation.unreadSpams : Translation.unreadSpam);

						// enable the mark as read menu item
						UI.menuItemMarkAsRead.Text = Translation.markAsRead + " (" + spam.ThreadsUnread + ")";
						UI.menuItemMarkAsRead.Enabled = true;

						// update the tag
						UI.NotificationService.Tag = "#spam";

						return;
					}
				}

				// exit the sync if the number of unread threads is the same as before
				if (!userAction && (Box.ThreadsUnread == UnreadThreads)) {
					return;
				}

				// manage unread threads
				if (Box.ThreadsUnread > 0) {

					// set the notification icon
					UI.notifyIcon.Icon = Box.ThreadsUnread <= Settings.Default.UNSTACK_BOUNDARY ? Resources.mails : Resources.stack;

					// manage message notification
					if (Settings.Default.MessageNotification) {

						// play a sound on unread threads
						if (Settings.Default.AudioNotification) {
							SystemSounds.Asterisk.Play();
						}

						// get the message details
						UsersResource.MessagesResource.ListRequest messages = Api.Users.Messages.List("me");
						messages.LabelIds = "UNREAD";
						messages.MaxResults = 1;
						Google.Apis.Gmail.v1.Data.Message message = await Api.Users.Messages.Get("me", await messages.ExecuteAsync().ContinueWith(m => {
							return m.Result.Messages.First().Id;
						})).ExecuteAsync();

						//  display a balloon tip in the systray with the total of unread threads and message details, depending on the user privacy setting
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
								subject = message.Snippet != "" ? WebUtility.HtmlDecode(message.Snippet) : Translation.newUnreadMessage;
							}

							// detect if the message contains attachments
							if (message.Payload.Parts != null && message.Payload.MimeType == "multipart/mixed") {
								int attachments = message.Payload.Parts.Where(part => !String.IsNullOrEmpty(part.Filename)).Count();

								if (attachments > 0) {
									from = (from.Length > 48 ? from.Substring(0, 48) : from) + " – " + attachments.ToString() + " " + (attachments > 1 ? Translation.attachments : Translation.attachment);
								}
							}

							UI.NotificationService.Tip(from, subject);
						} else {
							UI.NotificationService.Tip(Box.ThreadsUnread.ToString() + " " + (Box.ThreadsUnread > 1 ? Translation.unreadMessages : Translation.unreadMessage), Translation.newUnreadMessage);
						}

						// update the notification tag to allow the user to directly display the specified view (inbox/message/spam) in a browser
						UI.NotificationService.Tag = "#inbox" + (Box.ThreadsUnread == 1 ? "/" + message.Id : "");
					}

					// display the notification text
					UI.notifyIcon.Text = Box.ThreadsUnread.ToString() + " " + (Box.ThreadsUnread > 1 ? Translation.unreadMessages : Translation.unreadMessage);

					// enable the mark as read menu item
					UI.menuItemMarkAsRead.Text = Translation.markAsRead + " (" + Box.ThreadsUnread + ")";
					UI.menuItemMarkAsRead.Enabled = true;
				} else {

					// restore the default systray icon and text
					UI.notifyIcon.Icon = Resources.normal;
					UI.notifyIcon.Text = Translation.noMessage;

					// disable the mark as read menu item
					UI.menuItemMarkAsRead.Text = Translation.markAsRead;
					UI.menuItemMarkAsRead.Enabled = false;
				}

				// save the number of unread threads
				UnreadThreads = Box.ThreadsUnread;
			} catch (IOException exception) {

				// log the exception from mscorlib: sometimes the process can not access the token response file because it is used by another process
				Core.Log("IOException: " + exception.Message);
			} catch (Exception exception) {

				// display a balloon tip in the systray with the detailed error message
				UI.notifyIcon.Icon = Resources.warning;
				UI.notifyIcon.Text = Translation.syncError;
				UI.NotificationService.Tip(Translation.error, Translation.syncErrorOccured + exception.Message, Notification.Type.Warning, 1500);

				// log the error
				Core.Log("Sync: " + exception.Message);
			} finally {
				UI.notifyIcon.Text = UI.notifyIcon.Text.Split('\n')[0] + "\n" + Translation.syncTime.Replace("{time}", Time.ToLongTimeString());
			}
		}

		/// <summary>
		/// Asynchronous method used to mark as read the user inbox
		/// </summary>
		public async Task MarkAsRead() {
			try {

				// update the synchronization time
				Time = DateTime.Now;

				// display the sync icon
				UI.notifyIcon.Icon = Resources.sync;
				UI.notifyIcon.Text = Translation.sync;

				// create the request filter
				List<string> filter = new List<string> {
					"UNREAD"
				};

				// check for unread spams
				bool unreadSpams = UI.NotificationService.Tag == "#spam";

				if (unreadSpams) {
					filter.Add("SPAM");
				}

				// get all unread messages
				UsersResource.MessagesResource.ListRequest messages = Api.Users.Messages.List("me");
				messages.LabelIds = filter;
				ListMessagesResponse list = await messages.ExecuteAsync();
				IList<Message> unread = list.Messages;

				// loop through all unread threads and remove the "unread" label for each one
				if (unread != null && unread.Count > 0) {

					// batch all mail ids to modify
					IEnumerable<string> batch = unread.Select(
						thread => thread.Id
					);

					// create the batch request
					BatchModifyMessagesRequest request = new BatchModifyMessagesRequest {
						Ids = batch.ToList(),
						RemoveLabelIds = new List<string> { "UNREAD" }
					};

					// execute the batch request to mark all mails as read
					await Api.Users.Messages.BatchModify(request, "me").ExecuteAsync();

					// get the "inbox" label
					Box = await Api.Users.Labels.Get("me", "INBOX").ExecuteAsync();

					// update the statistics only when there is no unread spams
					if (!unreadSpams) {
						await UpdateStatistics().ConfigureAwait(false);
					}
				}

				// sync the inbox again if the user has just mark spams as read
				if (unreadSpams) {
					await Sync().ConfigureAwait(false);
				} else {

					// restore the default systray icon and text
					UI.notifyIcon.Icon = Resources.normal;
					UI.notifyIcon.Text = Translation.noMessage;

					// clean the tag
					UI.NotificationService.Tag = null;

					// reset the number of unread threads
					UnreadThreads = 0;

					// disable the mark as read menu item
					UI.menuItemMarkAsRead.Text = Translation.markAsRead;
					UI.menuItemMarkAsRead.Enabled = false;
				}
			} catch (Exception exception) {

				// enabled the mark as read menu item
				UI.menuItemMarkAsRead.Text = Translation.markAsRead + " (" + Box.ThreadsUnread + ")";
				UI.menuItemMarkAsRead.Enabled = true;

				// display a balloon tip in the systray with the detailed error message
				UI.notifyIcon.Icon = Resources.warning;
				UI.notifyIcon.Text = Translation.operationError;
				UI.NotificationService.Tip(Translation.error, Translation.markAsReadError + exception.Message, Notification.Type.Warning, 1500);

				// log the error
				Core.Log("MarkAsRead: " + exception.Message);
			} finally {
				UI.notifyIcon.Text = UI.notifyIcon.Text.Split('\n')[0] + "\n" + Translation.syncTime.Replace("{time}", Time.ToLongTimeString());
			}
		}

		/// <summary>
		/// Retry to reconnect the inbox
		/// </summary>
		public async Task Retry() {

			// increase the number of reconnection attempt
			ReconnectionAttempts++;

			// bypass the first reconnection attempt because the last synchronization have already checked the internet connectivity
			if (ReconnectionAttempts == 1) {

				// set the reconnection interval
				UI.timerReconnect.Interval = (int)Settings.Default.INTERVAL_RECONNECT * 1000;

				// disable the menu items
				UI.menuItemSynchronize.Enabled = false;
				UI.menuItemTimout.Enabled = false;
				UI.menuItemSettings.Enabled = false;

				// display the reconnection attempt message on the icon
				UI.notifyIcon.Icon = Resources.retry;
				UI.notifyIcon.Text = Translation.reconnectAttempt;

				return;
			}

			// if internet is down, wait for INTERVAL_RECONNECT seconds before next attempt
			if (!Computer.IsInternetAvailable()) {

				// after max unsuccessull reconnection attempts, the application waits for the next sync
				if (ReconnectionAttempts == Settings.Default.MAX_AUTO_RECONNECT) {
					UI.timerReconnect.Enabled = false;
					UI.timerReconnect.Interval = 100;
					UI.timer.Enabled = true;

					// activate the necessary menu items to allow the user to manually sync the inbox
					UI.menuItemSynchronize.Enabled = true;

					// display the last reconnection message on the icon
					UI.notifyIcon.Icon = Resources.warning;
					UI.notifyIcon.Text = Translation.reconnectFailed + "\n" + Translation.syncTime.Replace("{time}", Time.ToLongTimeString());
				}
			} else {

				// restore default operation when internet is available
				UI.timerReconnect.Enabled = false;
				UI.timerReconnect.Interval = 100;
				UI.timer.Enabled = true;

				// sync the user mailbox
				await Sync().ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Dispose the gmail api
		/// </summary>
		public void Dispose() {
			if (Api != null) {
				Api.Dispose();
			}
		}

		/// <summary>
		/// Initialize the gmail service base client api
		/// </summary>
		private void InitGmailServiceApi() {
			if (Api != null) {
				return;
			}

			// initialize the gmail service base client api
			Api = new GmailService(new BaseClientService.Initializer {
				HttpClientInitializer = UI.GmailService.Credential,
				ApplicationName = Settings.Default.APPLICATION_NAME
			});

			// retrieve the gmail address and store it in an application cache setting
			if (Settings.Default.EmailAddress == "-") {
				EmailAddress = Api.Users.GetProfile("me").Execute().EmailAddress;
				UI.labelEmailAddress.Text = EmailAddress;
				Settings.Default.EmailAddress = EmailAddress;
			}
		}

		/// <summary>
		/// Asynchronous method used to get account statistics
		/// </summary>
		private async Task UpdateStatistics() {

			// get inbox message count
			int unread = (int)Box.ThreadsUnread;
			int total = (int)Box.ThreadsTotal;

			// build the chart
			if (total == 0) {
				UI.chartUnreadMails.Width = 0;
				UI.chartTotalMails.Width = 0;
			} else {
				const int MAXIMUM_SCALE = 100;
				bool INBOX_FULL = total > MAXIMUM_SCALE;
				int scale = INBOX_FULL ? total : MAXIMUM_SCALE;

				UI.chartUnreadMails.Width = INBOX_FULL && unread == 1 ? 1 : (unread * UI.chartInbox.Width) / scale;
				UI.chartTotalMails.Width = (total * UI.chartInbox.Width) / scale;
			}

			// update the tooltip informations
			UI.tip.SetToolTip(UI.chartUnreadMails, unread + " " + (unread > 1 ? Translation.unreadMessages : Translation.unreadMessage));
			UI.tip.SetToolTip(UI.chartTotalMails, total + " " + (total > 1 ? Translation.messages : Translation.message));

			// update the draft informations
			ListDraftsResponse drafts = await Api.Users.Drafts.List("me").ExecuteAsync();
			UI.labelTotalDrafts.Enabled = true;
			UI.labelTotalDrafts.Text = drafts.Drafts != null ? drafts.Drafts.Count.ToString() : "0";

			// update the label informations
			ListLabelsResponse labels = await Api.Users.Labels.List("me").ExecuteAsync();
			UI.labelTotalLabels.Enabled = true;
			UI.labelTotalLabels.Text = labels.Labels != null ? labels.Labels.Count.ToString() : "0";
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
		/// Unread threads
		/// </summary>
		public int? UnreadThreads {
			get; set;
		} = 0;

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
		}

		#endregion
	}
}