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

			MajorVersion = int.Parse(ProductVersion[0]);
			MinorVersion = int.Parse(ProductVersion[1]);
			PatchVersion = int.Parse(ProductVersion[2]);

			Version = $"v{MajorVersion}.{MinorVersion}.{PatchVersion}";
		}

		/// <summary>
		/// Restart the application
		/// </summary>
		public static void RestartApplication() {

			// start a new process
			Process.Start(new ProcessStartInfo {
				FileName = Application.ExecutablePath,
				UseShellExecute = true,
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true
			});

			// exit the environment
			Environment.Exit(0);
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

		/// <summary>
		/// Major application version number
		/// </summary>
		public static int MajorVersion {
			get;
		} = 0;

		/// <summary>
		/// Minor application version number
		/// </summary>
		public static int MinorVersion {
			get;
		} = 0;

		/// <summary>
		/// Patch application version number
		/// </summary>
		public static int PatchVersion {
			get;
		} = 0;

		#endregion
	}
}