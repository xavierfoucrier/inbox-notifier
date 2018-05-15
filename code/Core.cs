using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace notifier {
	static class Core {

		#region #attributes

		/// <summary>
		/// Local application data folder name
		/// </summary>
		private static string ApplicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Gmail Notifier";

		/// <summary>
		/// Version number
		/// </summary>
		private static string Version = "";

		/// <summary>
		/// Major version number
		/// </summary>
		private static string VersionMajor = "";

		/// <summary>
		/// Minor version number
		/// </summary>
		private static string VersionMinor = "";

		/// <summary>
		/// Release version number
		/// </summary>
		private static string VersionRelease = "";

		/// <summary>
		/// Build version number
		/// </summary>
		private static string VersionBuild = "";

		#endregion

		#region #methods

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

		#endregion

		#region #accessors

		#endregion
	}
}
