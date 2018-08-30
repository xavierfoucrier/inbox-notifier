using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;
using HtmlAgilityPack;
using notifier.Languages;
using notifier.Properties;

namespace notifier {
	class Update {

		#region #attributes

		/// <summary>
		/// Update period possibilities
		/// </summary>
		private enum Period : uint {
			Startup = 0,
			Day = 1,
			Week = 2,
			Month = 3
		}

		/// <summary>
		/// Http client used to check for updates
		/// </summary>
		private HttpClient Http = new HttpClient();

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
		public Update(ref Main form) {
			UI = form;
		}

		/// <summary>
		/// Checks the update period user setting
		/// </summary>
		/// <returns>Indicates if the update period is currently set to startup</returns>
		public bool IsPeriodSetToStartup() {
			return Settings.Default.UpdatePeriod == (int)Period.Startup;
		}

		/// <summary>
		/// Deletes the setup installer package from the local application data folder
		/// </summary>
		public void DeleteSetupPackage() {
			if (!Directory.Exists(Core.ApplicationDataFolder)) {
				return;
			}

			IEnumerable<string> executables = Directory.EnumerateFiles(Core.ApplicationDataFolder, "*.exe", SearchOption.TopDirectoryOnly);

			foreach (string executable in executables) {
				try {
					File.Delete(executable);
				} catch (Exception) {
					// nothing to catch: executable is currently locked
					// setup package will be removed next time
				}
			}
		}

		/// <summary>
		/// Checks for update depending on the user settings
		/// </summary>
		public void Ping() {
			if (!Settings.Default.UpdateService) {
				return;
			}

			switch (Settings.Default.UpdatePeriod) {
				case (int)Period.Day:
					if (DateTime.Now >= Settings.Default.UpdateControl.AddDays(1)) {
						Check(false);
					}

					break;
				case (int)Period.Week:
					if (DateTime.Now >= Settings.Default.UpdateControl.AddDays(7)) {
						Check(false);
					}

					break;
				case (int)Period.Month:
					if (DateTime.Now >= Settings.Default.UpdateControl.AddMonths(1)) {
						Check(false);
					}

					break;
			}
		}

		/// <summary>
		/// Asynchronous method to connect to the repository and check if there is an update available
		/// </summary>
		/// <param name="verbose">Indicates if the process displays a message when a new update package is available</param>
		/// <param name="startup">Indicates if the update check process has been started at startup</param>
		public async void Check(bool verbose = true, bool startup = false) {
			try {

				// using tls 1.2 as security protocol to contact Github.com
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

				// gets the list of tags in the Github repository tags webpage
				HttpResponseMessage response = await Http.GetAsync(Settings.Default.GITHUB_REPOSITORY + "/tags");

				HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
				document.LoadHtml(await response.Content.ReadAsStringAsync());

				HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//span[@class='tag-name']");

				if (collection == null || collection.Count == 0) {

					// indicates to the user that the update service is not reachable for the moment
					if (verbose) {
						UI.NotificationService.Tip(Settings.Default.UPDATE_SERVICE_NAME, Translation.updateServiceUnreachable, Notification.Type.Warning, 1500);
					}

					return;
				}

				List<string> tags = collection.Select(p => p.InnerText).ToList();
				string release = tags.First();

				// stores the latest update datetime control
				Settings.Default.UpdateControl = DateTime.Now;

				// updates the update control label
				UI.labelUpdateControl.Text = Settings.Default.UpdateControl.ToString();

				// the current version tag is not at the top of the list
				if (release != Core.Version) {

					// stores the update state
					UpdateAvailable = true;
					ReleaseAvailable = release;

					// updates the notification service tag
					UI.NotificationService.Tag = "update";

					// updates the check for update button text
					UI.buttonCheckForUpdate.Text = Translation.updateNow;

					// downloads the update package automatically or asks the user, depending on the user setting and verbosity
					if (verbose) {
						UI.NotificationService.Tip(Settings.Default.UPDATE_SERVICE_NAME, Translation.newVersion.Replace("{version}", ReleaseAvailable), Notification.Type.Info, 1500);
					} else if (Settings.Default.UpdateDownload) {
						Download();
					}
				} else if (verbose && !startup) {
					MessageBox.Show(Translation.latestVersion, Settings.Default.UPDATE_SERVICE_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			} catch (Exception) {

				// indicates to the user that the update service is not reachable for the moment
				if (verbose) {
					UI.NotificationService.Tip(Settings.Default.UPDATE_SERVICE_NAME, Translation.updateServiceUnreachable, Notification.Type.Warning, 1500);
				}
			} finally {

				// restores default update button state
				UI.buttonCheckForUpdate.Enabled = true;

				// synchronizes the inbox if the updates has been checked at startup after asynchronous authentication
				if (startup) {
					UI.GmailService.Inbox.Sync();
				}
			}
		}

		/// <summary>
		/// Downloads and launch the setup installer
		/// </summary>
		public void Download() {

			// defines that the application is currently updating
			Updating = true;

			// defines the new number version and temp path
			string newversion = ReleaseAvailable.Split('-')[0].Substring(1);
			string updatepath = Core.ApplicationDataFolder + "/gmnupdate-" + newversion + ".exe";
			string package = Settings.Default.GITHUB_REPOSITORY + "/releases/download/" + ReleaseAvailable + "/Gmail.Notifier." + newversion + ".exe";

			try {

				// disables the context menu and displays the update icon in the systray
				UI.notifyIcon.ContextMenu = null;
				UI.notifyIcon.Icon = Resources.updating;
				UI.notifyIcon.Text = Translation.updating;

				// creates a new web client instance
				WebClient client = new WebClient();

				// displays the download progression on the systray icon, and prevents the application from restoring the context menu and systray icon at startup
				client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((object source, DownloadProgressChangedEventArgs target) => {
					UI.notifyIcon.ContextMenu = null;
					UI.notifyIcon.Icon = Resources.updating;
					UI.notifyIcon.Text = Translation.updating + " " + target.ProgressPercentage.ToString() + "%";
				});

				// starts the setup installer when the download has complete and exits the current application
				client.DownloadFileCompleted += new AsyncCompletedEventHandler((object source, AsyncCompletedEventArgs target) => {
					Process.Start(new ProcessStartInfo("cmd.exe", "/C ping 127.0.0.1 -n 2 && \"" + updatepath + "\" " + (Settings.Default.UpdateQuiet ? "/verysilent" : "")) {
						WindowStyle = ProcessWindowStyle.Hidden,
						CreateNoWindow = true
					});

					Application.Exit();
				});

				// ensures that the Github package URI is callable
				client.OpenRead(package).Close();

				// starts the download of the new version from the Github repository
				client.DownloadFileAsync(new Uri(package), updatepath);
			} catch (Exception) {

				// indicates to the user that the update service is not reachable for the moment
				UI.NotificationService.Tip(Settings.Default.UPDATE_SERVICE_NAME, Translation.updateServiceUnreachable, Notification.Type.Warning, 1500);

				// defines that the application has exited the updating state
				Updating = false;

				// restores default update button state
				UI.buttonCheckForUpdate.Enabled = true;

				// restores the context menu to the systray icon and start a synchronization
				UI.notifyIcon.ContextMenu = UI.notifyMenu;
				UI.GmailService.Inbox.Sync();
			}
		}

		#endregion

		#region #accessors

		/// <summary>
		/// Flag defining if an update is available
		/// </summary>
		public bool UpdateAvailable {
			get; set;
		} = false;

		/// <summary>
		/// Latest release version available
		/// </summary>
		public string ReleaseAvailable {
			get; set;
		} = "";

		/// <summary>
		/// Flag defining if the update service is currently updating
		/// </summary>
		public bool Updating {
			get; set;
		} = false;

		#endregion
	}
}