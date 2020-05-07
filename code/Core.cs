using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using notifier.Properties;

namespace notifier {
	static class Core {

		#region #attributes

		#endregion

		#region #methods

		/// <summary>
		/// Class constructor
		/// </summary>
		static Core() {

			// initialize the application version number, based on scheme Semantic Versioning - https://semver.org
			string[] ProductVersion = Application.ProductVersion.Split('.');

			string VersionMajor = ProductVersion[0];
			string VersionMinor = ProductVersion[1];
			string VersionPatch = ProductVersion[2];

			Version = $"v{VersionMajor}.{VersionMinor}.{VersionPatch}";
		}

		/// <summary>
		/// Restart the application
		/// </summary>
		public static void RestartApplication() {

			// start a new process
			Process.Start(new ProcessStartInfo("cmd.exe", $"/C ping 127.0.0.1 -n 2 && \"{Application.ExecutablePath}\"") {
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true
			});

			// exit the application
			Application.Exit();
		}

		/// <summary>
		/// Log a message to the application log file
		/// </summary>
		/// <param name="message">Message to log</param>
		public static void Log(string message) {
			using (StreamWriter writer = new StreamWriter($"{ApplicationDataFolder}/{Settings.Default.LOG_FILE}", true)) {
				writer.Write($"{DateTime.Now} - {message}{Environment.NewLine}");
			}
		}

		#endregion

		#region #accessors

		/// <summary>
		/// Local application data folder name
		/// </summary>
		public static string ApplicationDataFolder {
			get;
		} = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/Inbox Notifier";

		/// <summary>
		/// Full application version number
		/// </summary>
		public static string Version {
			get;
		} = "";

		#endregion
	}
}