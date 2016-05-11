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

			// authenticates the application
			GmailAuthentication();

			// displays the product version
			string[] version = Application.ProductVersion.Split('.');
			labelVersion.Text = version[0] + "." + version[1] + "-" + (version[2] == "0" ? "alpha" : version[2] == "1" ? "beta" : version[2] == "2" ? "rc" : version[2] == "3" ? "release" : "") + (version[3] != "0" ? " " + version[3] : "");
		}

		/// <summary>
		/// Authenticates the application through the gmail api
		/// </summary>
		private void GmailAuthentication() {

			// creates web authorization broker and waits for the user validation
			using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) {
				this.credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)).Secrets,
					new string[] { GmailService.Scope.GmailReadonly },
					"user",
					CancellationToken.None,
					new FileDataStore(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), true)
				).Result;
			}
		}
	}
}
