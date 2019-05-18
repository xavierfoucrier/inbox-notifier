using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using notifier.Properties;

namespace notifier {
	static class Core {

		#region #attributes

		/// <summary>
		/// Major version number
		/// </summary>
		private static readonly string VersionMajor = "";

		/// <summary>
		/// Minor version number
		/// </summary>
		private static readonly string VersionMinor = "";

		/// <summary>
		/// Release version number
		/// </summary>
		private static readonly string VersionRelease = "";

		/// <summary>
		/// Build version number
		/// </summary>
		private static readonly string VersionBuild = "";

		#endregion

		#region #methods

		/// <summary>
		/// Class constructor
		/// </summary>
		static Core() {

			// initialize the application version number, based on scheme Major.Minor.Build-Release
			string[] ProductVersion = Application.ProductVersion.Split('.');

			VersionMajor = ProductVersion[0];
			VersionMinor = ProductVersion[1];
			VersionRelease = ProductVersion[2];
			VersionBuild = ProductVersion[3];

			Version = "v" + VersionMajor + "." + VersionMinor + (VersionBuild != "0" ? "." + VersionBuild : "") + "-" + (VersionRelease == "0" ? "alpha" : VersionRelease == "1" ? "beta" : VersionRelease == "2" ? "rc" : VersionRelease == "3" ? "release" : "");
		}

		/// <summary>
		/// Restart the application
		/// </summary>
		public static void RestartApplication() {

			// start a new process
			Process.Start(new ProcessStartInfo("cmd.exe", "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"") {
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
			using (StreamWriter writer = new StreamWriter(ApplicationDataFolder + "/" + Settings.Default.LOG_FILE, true)) {
				writer.Write(DateTime.Now + " - " + message + Environment.NewLine);
			}
		}

		#endregion

		#region #accessors

		/// <summary>
		/// Local application data folder name
		/// </summary>
		public static string ApplicationDataFolder {
			get; set;
		} = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Gmail Notifier";

		/// <summary>
		/// Full application version number
		/// </summary>
		public static string Version {
			get; set;
		} = "";

		#endregion
	}
}