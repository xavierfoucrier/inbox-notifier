using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
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
		private readonly HttpClient Http = new HttpClient();

		/// <summary>
		/// Update endpoint
		/// </summary>
		private readonly string EndPoint = "https://api.github.com/repos/xavierfoucrier/inbox-notifier/releases";

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
		public Update(ref Main form) {
			UI = form;
		}

		/// <summary>
		/// Check the update period user setting
		/// </summary>
		/// <returns>Indicate if the update period is currently set to startup</returns>
		public static bool IsPeriodSetToStartup() {
			return Settings.Default.UpdatePeriod == (uint)Period.Startup;
		}

		/// <summary>
		/// Delete the setup installer package from the local application data folder
		/// </summary>
		public static void DeleteSetupPackage() {
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
		/// Asynchronous method to check for update depending on the user settings
		/// </summary>
		public async Task Ping() {
			if (!Settings.Default.UpdateService) {
				return;
			}

			switch (Settings.Default.UpdatePeriod) {
				case (uint)Period.Day:
					if (DateTime.Now >= Settings.Default.UpdateControl.AddDays(1)) {
						await Check(false).ConfigureAwait(false);
					}

					break;
				default:
					if (DateTime.Now >= Settings.Default.UpdateControl.AddDays(7)) {
						await Check(false).ConfigureAwait(false);
					}

					break;
				case (uint)Period.Month:
					if (DateTime.Now >= Settings.Default.UpdateControl.AddMonths(1)) {
						await Check(false).ConfigureAwait(false);
					}

					break;
			}
		}

		/// <summary>
		/// Asynchronous method to connect to the repository and check if there is an update available
		/// </summary>
		/// <param name="verbose">Indicate if the process display a message when a new update package is available</param>
		/// <param name="startup">Indicate if the update check process has been started at startup</param>
		public async Task Check(bool verbose = true, bool startup = false) {
			try {

				// define user agent
				Http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("InboxNotifier", Core.Version));

				// request the open Github API
				HttpResponseMessage httpResponse = await Http.GetAsync(EndPoint);
				httpResponse.EnsureSuccessStatusCode();

				string responseBody = await httpResponse.Content.ReadAsStringAsync();
				JArray releases = JArray.Parse(responseBody);

				// filter by releases only (exclude pre-releases)
				string release = releases.Where(version => !(bool)version["prerelease"]).First()["tag_name"].ToString();

				// store the latest update datetime control
				Settings.Default.UpdateControl = DateTime.Now;

				// update the update control label
				UI.labelUpdateControl.Text = Settings.Default.UpdateControl.ToString();

				// the current version tag is not at the top of the list
				if (release != Core.Version) {

					// store the update state
					UpdateAvailable = true;
					ReleaseAvailable = release;

					// update the notification service tag
					UI.NotificationService.Tag = "update";

					// update the check for update button text
					UI.buttonCheckForUpdate.Text = Translation.updateNow;

					// check for major version changes
					int major = int.Parse(Regex.Match(release, @"v(\d+)").Groups[1].Value);

					if (major > Core.MajorVersion) {

						// store the major update state
						MajorUpdateAvailable = true;

						// notify the user about new major release
						if (verbose) {
							UI.NotificationService.Tip(Translation.updateServiceName, Translation.newMajorVersion.Replace("{version}", ReleaseAvailable), Notification.Type.Info, 1500);
						}
					} else {

						// download the update package automatically or ask the user, depending on the user setting and verbosity
						if (verbose) {
							UI.NotificationService.Tip(Translation.updateServiceName, Translation.newVersion.Replace("{version}", ReleaseAvailable), Notification.Type.Info, 1500);
						} else if (Settings.Default.UpdateDownload) {
							await Download().ConfigureAwait(false);
						}
					}
				} else if (verbose && !startup) {
					MessageBox.Show(Translation.latestVersion, Translation.updateServiceName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			} catch (Exception exception) {

				// indicate to the user that the update service is not reachable for the moment
				if (verbose) {
					UI.NotificationService.Tip(Translation.updateServiceName, Translation.updateServiceUnreachable, Notification.Type.Warning, 1500);
				}

				// log the error
				Core.Log($"UpdateCheck: {exception.Message}");
			} finally {

				// restore default update button state
				UI.buttonCheckForUpdate.Enabled = true;
			}
		}

		/// <summary>
		/// Download and launch the setup installer
		/// </summary>
		public async Task Download() {

			// define that the application is currently updating
			Updating = true;

			// define the new number version and temp path
			string newversion = ReleaseAvailable.Split('-')[0].Substring(1);
			string updatepath = $"{Core.ApplicationDataFolder}/gmnupdate-{newversion}.exe";
			string package = $"{Settings.Default.GITHUB_REPOSITORY}/releases/download/{ReleaseAvailable}/Inbox.Notifier.{newversion}.exe";

			try {

				// disable the context menu and display the update icon in the systray
				UI.notifyIcon.ContextMenu = null;
				UI.notifyIcon.Icon = Resources.updating;
				UI.notifyIcon.Text = Translation.updating;

				// create a new web client instance
				WebClient client = new WebClient();

				// display the download progression on the systray icon, and prevent the application from restoring the context menu and systray icon at startup
				client.DownloadProgressChanged += (object source, DownloadProgressChangedEventArgs target) => {
					UI.notifyIcon.ContextMenu = null;
					UI.notifyIcon.Icon = Resources.updating;
					UI.notifyIcon.Text = $"{Translation.updating} {target.ProgressPercentage}%";
				};

				// start the setup installer when the download has complete and exit the current application
				client.DownloadFileCompleted += (object source, AsyncCompletedEventArgs target) => {

					// start a new process
					Process.Start(new ProcessStartInfo {
						FileName = updatepath,
						UseShellExecute = true,
						WindowStyle = ProcessWindowStyle.Hidden,
						CreateNoWindow = true,
						Arguments = Settings.Default.UpdateQuiet ? "/verysilent" : ""
					});

					// exit the environment
					Environment.Exit(0);
				};

				// ensure that the Github package URI is callable
				client.OpenRead(package).Close();

				// start the download of the new version from the Github repository
				client.DownloadFileAsync(new Uri(package), updatepath);
			} catch (Exception exception) {

				// indicate to the user that the update service is not reachable for the moment
				UI.NotificationService.Tip("UPDATE_SERVICE_NAME", Translation.updateServiceUnreachable, Notification.Type.Warning, 1500);

				// define that the application has exited the updating state
				Updating = false;

				// restore default update button state
				UI.buttonCheckForUpdate.Enabled = true;

				// restore the context menu to the systray icon and start a synchronization
				UI.notifyIcon.ContextMenu = UI.notifyMenu;
				await UI.GmailService.Inbox.Sync();

				// log the error
				Core.Log($"UpdateDownload: {exception.Message}");
			}
		}

		/// <summary>
		/// Show the Github release page in a browser
		/// </summary>
		public void ShowGithubRelease() {

			// open the release link in a browser
			Process.Start($"{Settings.Default.GITHUB_REPOSITORY}/releases/tag/{ReleaseAvailable}");

			// restore default update button state
			UI.buttonCheckForUpdate.Enabled = true;
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
		/// Flag defining if a major update is available
		/// </summary>
		public bool MajorUpdateAvailable {
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