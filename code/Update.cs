using System;
using System.Collections.Generic;
using System.IO;
using notifier.Properties;

namespace notifier {
	class Update : Main {

		/// <summary>
		/// Class constructor
		/// </summary>
		public Update() {
		}

		/// <summary>
		/// Cleans all temporary update files present in the use local application data folder
		/// </summary>
		public void CleanTemporaryUpdateFiles() {
			if (!Directory.Exists(GetAppData())) {
				return;
			}

			IEnumerable<string> executables = Directory.EnumerateFiles(GetAppData(), "*.exe", SearchOption.TopDirectoryOnly);

			foreach (string executable in executables) {
				try {
					File.Delete(executable);
				} catch (Exception) {
					// nothing to catch: executable is currently locked
					// setup package will be removed next time
				}
			}
		}
	}
}
