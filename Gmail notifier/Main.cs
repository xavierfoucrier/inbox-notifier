using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace notifier {
	public partial class Main : Form {

		// gmail api service
		private GmailService service;

		// user credential for the gmail authentication
		private UserCredential credential;

		/// <summary>
		/// Initializes the class
		/// </summary>
		public Main() {
			InitializeComponent();
		}

		/// <summary>
		/// Loads the form
		/// </summary>
		private void Main_Load(object sender, EventArgs e) {

			// authenticates the user
			AsyncAuthentication();

			// displays the product version
			string[] version = Application.ProductVersion.Split('.');
			labelVersion.Text = version[0] + "." + version[1] + "-" + (version[2] == "0" ? "alpha" : version[2] == "1" ? "beta" : version[2] == "2" ? "rc" : version[2] == "3" ? "release" : "") + (version[3] != "0" ? " " + version[3] : "");
		}

		/// <summary>
		/// Prompts the user before closing the form
		/// </summary>
		private void Main_FormClosing(object sender, FormClosingEventArgs e) {

			// asks the user for exit, depending on the application settings
			if (e.CloseReason != CloseReason.ApplicationExitCall && Properties.Settings.Default.AskonExit) {
				DialogResult dialog = MessageBox.Show("Vous êtes sur le point de quitter l'application.\n\nVoulez-vous vraiment quitter l'application ?", "Fermeture de l'application", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

				if (dialog == DialogResult.No) {
					e.Cancel = true;
				}
			}
		}

		/// <summary>
		/// Asynchronous method used to get user credential
		/// </summary>
		private async void AsyncAuthentication() {
			try {
				this.credential = await AsyncAuthorizationBroker();
			} catch(Exception) {
				MessageBox.Show("Vous avez refusé que l'application accède à votre compte Gmail. Cette étape est nécessaire et vous sera demandée à nouveau lors du prochain démarrage.\n\nL'application va désormais quitter.", "Erreur d'authentification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				Application.Exit();
			}
		}

		/// <summary>
		/// Asynchronous task used to get the user authorization
		/// </summary>
		/// <returns>OAuth 2.0 user credential</returns>
		private async Task<UserCredential> AsyncAuthorizationBroker() {

			// uses the client secret file for the context
			using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) {

				// waits for the user validation, only if the user has not already authorized the application
				return await GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					new string[] { GmailService.Scope.GmailReadonly },
					"user",
					CancellationToken.None,
					new FileDataStore(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), true)
				);
			}
		}

		/// <summary>
		/// Manages the AskonExit user setting
		/// </summary>
		private void fieldAskonExit_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.AskonExit = fieldAskonExit.Checked;
			Properties.Settings.Default.Save();
			labelSettingsSaved.Visible = true;
		}
	}
}
