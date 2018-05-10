using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace notifier {
	static class Core {

		// local application data folder name
		private static string ApplicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Gmail Notifier";

		// version number
		private static string Version = "";

		// major version number
		private static string VersionMajor = "";

		// minor version number
		private static string VersionMinor = "";

		// release version number
		private static string VersionRelease = "";

		// build version number
		private static string VersionBuild = "";

		/// <summary>
		/// Class constructor
		/// </summary>
		static Core() {

			// initializes the application version number, based on scheme Major.Minor.Build-Release
			string[] ProductVersion = Application.ProductVersion.Split('.');

			VersionMajor = ProductVersion[0];
			VersionMinor = ProductVersion[1];
			VersionRelease = ProductVersion[2];
			VersionBuild = ProductVersion[3];

			Version = "v" + VersionMajor + "." + VersionMinor + (VersionBuild != "0" ? "." + VersionBuild : "") + "-" + (VersionRelease == "0" ? "alpha" : VersionRelease == "1" ? "beta" : VersionRelease == "2" ? "rc" : VersionRelease == "3" ? "release" : "");
		}

		/// <summary>
		/// Restarts the application
		/// </summary>
		public static void RestartApplication() {

			// starts a new process
			ProcessStartInfo command = new ProcessStartInfo("cmd.exe", "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"") {
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true
			};

			Process.Start(command);

			// exits the application
			Application.Exit();
		}

		/// <summary>
		/// Gets the application data folder path
		/// </summary>
		/// <returns>The application data folder path</returns>
		public static string GetApplicationDataFolder() {
			return ApplicationDataFolder;
		}

		/// <summary>
		/// Gets the full application version number
		/// </summary>
		/// <returns>The full application version number, like v1.6.0.0</returns>
		public static string GetVersion() {
			return Version;
		}
	}
}
