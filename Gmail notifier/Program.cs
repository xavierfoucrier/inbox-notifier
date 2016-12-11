using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using notifier.Languages;
using notifier.Properties;

namespace notifier {
	static class Program {

		/// <summary>
		/// Mutex associated to the application instance
		/// </summary>
		static Mutex mutex = new Mutex(true, "gmailnotifier-115e363ecbfefd771e55c6874680bc0a");

		[STAThread]
		static void Main() {

			// initializes the interface with the specified culture, depending on the user settings
			switch (Settings.Default.Language) {
				default:
				case "English":
					Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
					break;
				case "Français":
					Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");
					break;
				case "Deutsch":
					Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
					break;
			}

			// check if there is an instance running
			if (!mutex.WaitOne(TimeSpan.Zero, true)) {
				MessageBox.Show(translation.mutexError, translation.multipleInstances, MessageBoxButtons.OK, MessageBoxIcon.Warning);

				return;
			}

			// sets some default properties
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// run the main window
			Application.Run(new Main());

			mutex.ReleaseMutex();
		}
	}
}