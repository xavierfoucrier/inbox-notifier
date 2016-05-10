using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace notifier {
	static class Program {

		/// <summary>
		/// Mutex associated to the application instance
		/// </summary>
		static Mutex mutex = new Mutex(true, "gmailnotifier-115e363ecbfefd771e55c6874680bc0a");

		[STAThread]
		static void Main() {

			// check if there is an instance running
			if (!mutex.WaitOne(TimeSpan.Zero, true)) {
				MessageBox.Show("L'application \"Gmail notifier\" est déjà en cours d'exécution : vous ne pouvez pas lancer plusieurs instances de l'application sur un même ordinateur.\n\nCette option n'est pas activée sur ce type d'application.", "Instances multiples", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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