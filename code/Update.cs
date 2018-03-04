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

		// update period possibilities
		private enum Period : uint {
			Startup = 0,
			Day = 1,
			Week = 2,
			Month = 3
		}

		// flag defining the update state
		private bool Updating = false;

		// http client used to check for updates
		private HttpClient Http = new HttpClient();

		// Reference to the main interface
		private Main Interface;

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="Form">Reference to the application main window</param>
		public Update(ref Main Form) {
			Interface = Form;
		}

		/// <summary>
		/// Indicates if the update service is currently updating
		/// </summary>
		public bool IsUpdating() {
			return Updating;
		}

		/// <summary>
		/// Indicates if the update period is currently set to startup
		/// </summary>
		public bool IsPeriodSetToStartup() {
			return Settings.Default.UpdatePeriod == (int)Period.Startup;
		}

		/// <summary>
		/// Deletes the setup installer package from the local application data folder
		/// </summary>
		public void DeleteSetupPackage() {
			if (!Directory.Exists(Core.GetApplicationDataFolder())) {
				return;
			}

			IEnumerable<string> executables = Directory.EnumerateFiles(Core.GetApplicationDataFolder(), "*.exe", SearchOption.TopDirectoryOnly);

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

				var document = new HtmlAgilityPack.HtmlDocument();
				document.LoadHtml(await response.Content.ReadAsStringAsync());

				HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//span[@class='tag-name']");

				if (collection == null || collection.Count == 0) {
					return;
				}

				List<string> tags = collection.Select(p => p.InnerText).ToList();
				string release = tags.First();

				// the current version tag is not at the top of the list
				if (release != Core.GetVersion()) {

					// downloads the update package automatically or asks the user, depending on the user setting and verbosity
					if (verbose) {
						DialogResult dialog = MessageBox.Show(translation.newVersion.Replace("{version}", tags[0]), "Gmail Notifier Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

						if (dialog == DialogResult.Yes) {
							Download(release);
						}
					} else if (Settings.Default.UpdateDownload) {
						Download(release);
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
				Interface.linkCheckForUpdate.Enabled = true;
				Interface.linkCheckForUpdate.Image = Resources.update_check;
				Interface.buttonCheckForUpdate.Enabled = true;

				// stores the latest update datetime control
				Settings.Default.UpdateControl = DateTime.Now;

				// updates the update control label
				Interface.labelUpdateControl.Text = Settings.Default.UpdateControl.ToString();

				// synchronizes the inbox if the updates has been checked at startup after asynchronous authentication
				if (startup) {
					Interface.GmailService.GetInbox().Sync();
				}
			}
		}

		/// <summary>
		/// Downloads and launch the setup installer
		/// </summary>
		/// <param name="release">Version number package to download</param>
		private void Download(string release) {

			// defines that the application is currently updating
			Updating = true;

			// defines the new number version and temp path
			string newversion = release.Split('-')[0].Substring(1);
			string updatepath = Core.GetApplicationDataFolder() + "/gmnupdate-" + newversion + ".exe";
			string package = Settings.Default.GITHUB_REPOSITORY + "/releases/download/" + release + "/Gmail.Notifier." + newversion + ".exe";

			try {

				// disables the context menu and displays the update icon in the systray
				Interface.notifyIcon.ContextMenu = null;
				Interface.notifyIcon.Icon = Resources.updating;
				Interface.notifyIcon.Text = translation.updating;

				// creates a new web client instance
				WebClient client = new WebClient();

				// displays the download progression on the systray icon, and prevents the application from restoring the context menu and systray icon at startup
				client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((object o, DownloadProgressChangedEventArgs target) => {
					Interface.notifyIcon.ContextMenu = null;
					Interface.notifyIcon.Icon = Resources.updating;
					Interface.notifyIcon.Text = translation.updating + " " + target.ProgressPercentage.ToString() + "%";
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
				Updating = false;

				// restores the context menu to the systray icon and start a synchronization
				Interface.notifyIcon.ContextMenu = Interface.contextMenu;
				Interface.GmailService.GetInbox().Sync();
			}
		}
	}
}
