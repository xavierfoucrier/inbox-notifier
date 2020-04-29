using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using notifier.Languages;
using notifier.Properties;

namespace notifier {
	static class Program {

		#region #attributes

		/// <summary>
		/// Mutex associated to the application instance
		/// </summary>
		static Mutex Mutex = new Mutex(true, "inboxnotifier-115e363ecbfefd771e55c6874680bc0a");

		#endregion

		#region #methods

		[STAThread]
		static void Main(string[] args) {

			// initialize the configuration file with setup installer settings
			if (args.Length == 3 && args[0] == "install") {

				// language application setting
				switch (args[1]) {
					default:
						Settings.Default.Language = "English";
						break;
					case "fr":
						Settings.Default.Language = "Français";
						break;
					case "de":
						Settings.Default.Language = "Deutsch";
						break;
					case "ru":
						Settings.Default.Language = "Русский";
						break;
				}

				// start with Windows setting
				Settings.Default.RunAtWindowsStartup = args[2] == "auto";

				// commit changes to the configuration file
				Settings.Default.Save();

				return;
			}

			// initialize the interface with the specified culture, depending on the user settings
			switch (Settings.Default.Language) {
				default:
					CultureInfo.CurrentUICulture = new CultureInfo("en-US");
					break;
				case "Français":
					CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
					break;
				case "Deutsch":
					CultureInfo.CurrentUICulture = new CultureInfo("de-DE");
					break;
				case "Русский":
					CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");
					break;
			}

			// check if there is an instance running
			if (!Mutex.WaitOne(TimeSpan.Zero, true)) {
				MessageBox.Show(Translation.mutexError, Translation.multipleInstances, MessageBoxButtons.OK, MessageBoxIcon.Warning);

				return;
			}

			// set some default properties
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// set the process priority to "low"
			Process process = Process.GetCurrentProcess();
			process.PriorityClass = ProcessPriorityClass.BelowNormal;

			// run the main window
			Application.Run(new Main());

			// release the mutex instance
			Mutex.ReleaseMutex();
		}

		#endregion

		#region #accessors

		#endregion
	}
}