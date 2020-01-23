using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using notifier.Properties;

namespace notifier {
	class Computer {

		#region #attributes

		/// <summary>
		/// Registration possibilities
		/// </summary>
		public enum Registration : uint {
			Off = 0,
			On = 1
		}

		/// <summary>
		/// Power resume state of the computer
		/// </summary>
		private bool PowerResume;

		/// <summary>
		/// Reference to the main interface
		/// </summary>
		private readonly Main UI;

		#endregion

		#region #methods

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="form">Reference to the application main window</param>
		public Computer(ref Main form) {
			UI = form;
		}

		/// <summary>
		/// Bind the "NetworkAvailabilityChanged" event to automatically sync the inbox when a network is available
		/// </summary>
		public void BindNetwork() {
			NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(async (object source, NetworkAvailabilityEventArgs target) => {

				// stop the reconnect process if it is running
				if (UI.GmailService.Inbox.ReconnectionAttempts != 0) {
					UI.timerReconnect.Enabled = false;
					UI.timerReconnect.Interval = 100;
					UI.GmailService.Inbox.ReconnectionAttempts = 0;
				}

				// loop through all network interface to check network connectivity
				foreach (NetworkInterface network in NetworkInterface.GetAllNetworkInterfaces()) {

					// discard "non-up" status, modem, serial, loopback and tunnel
					if (network.OperationalStatus != OperationalStatus.Up || network.Speed < 0 || network.NetworkInterfaceType == NetworkInterfaceType.Loopback || network.NetworkInterfaceType == NetworkInterfaceType.Tunnel) {
						continue;
					}

					// discard virtual cards (like virtual box, virtual pc, etc.) and microsoft loopback adapter (showing as ethernet card)
					if (network.Name.ToLower().Contains("virtual") || network.Description.ToLower().Contains("virtual") || network.Description.ToLower() == ("microsoft loopback adapter")) {
						continue;
					}

					// sync the inbox when a network interface is available and the timeout mode is disabled
					if (!UI.NotificationService.Paused) {
						await UI.GmailService.Inbox.Sync();
					}

					break;
				}
			});
		}

		/// <summary>
		/// Bind the "PowerModeChanged" event to automatically pause/resume the application synchronization
		/// </summary>
		public void BindPowerMode() {
			SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(async (object source, PowerModeChangedEventArgs target) => {
				if (target.Mode == PowerModes.Suspend) {

					// suspend the main timer
					UI.timer.Enabled = false;
				} else if (target.Mode == PowerModes.Resume) {

					// store the power resume state
					PowerResume = true;

					// do nothing if the timeout mode is set to infinite
					if (UI.NotificationService.Paused && UI.menuItemTimeoutIndefinitely.Checked) {
						return;
					}

					// synchronize the inbox and renew the token
					await UI.GmailService.Inbox.Sync(true, true);

					// enable the timer properly
					UI.timer.Enabled = true;
				}
			});
		}

		/// <summary>
		/// Bind the "SessionSwitch" event to automatically sync the inbox on session unlocking
		/// </summary>
		public void BindSessionSwitch() {
			SystemEvents.SessionSwitch += new SessionSwitchEventHandler(async (object source, SessionSwitchEventArgs target) => {

				// sync the inbox when the user is unlocking the Windows session
				if (target.Reason == SessionSwitchReason.SessionUnlock) {

					// do nothing if the timeout mode is set to infinite
					if (UI.NotificationService.Paused && UI.menuItemTimeoutIndefinitely.Checked) {
						return;
					}

					// do nothing if the computer power mode is resumed
					if (PowerResume) {
						PowerResume = false;
						return;
					}

					// synchronize the inbox and renew the token
					await UI.GmailService.Inbox.Sync(true, true);
				}
			});
		}

		/// <summary>
		/// Asynchronous method to check the internet connectivity
		/// </summary>
		/// <returns>Indicate if the user is connected to the internet, false means that the request to the DNS_REGISTRY_IP and Google server has failed</returns>
		public static async Task<bool> IsInternetAvailable() {
			try {

				// send a ping to the DNS registry
				PingReply reply = await new Ping().SendPingAsync(Settings.Default.DNS_REGISTRY_IP, 1000, new byte[32]);

				if (reply.Status == IPStatus.Success) {
					return true;
				} else {

					// use Google secured homepage as alternative to the DNS ping
					using (WebClient client = new WebClient()) {
						using (Stream stream = await client.OpenReadTaskAsync("https://www.google.com")) {
							return true;
						}
					}
				}
			} catch (Exception) {
				return false;
			}
		}

		/// <summary>
		/// Regulate the start with Windows setting against the registry to prevent bad registry reflection
		/// </summary>
		public static void RegulatesRegistry() {
			using (RegistryKey key = Registry.CurrentUser.OpenSubKey(Settings.Default.REGISTRY_KEY, true)) {
				if (key.GetValue("Inbox Notifier") != null) {
					if (!Settings.Default.RunAtWindowsStartup) {
						Settings.Default.RunAtWindowsStartup = true;
					}
				} else {
					if (Settings.Default.RunAtWindowsStartup) {
						Settings.Default.RunAtWindowsStartup = false;
					}
				}
			}
		}

		/// <summary>
		/// Register or unregister the application from Windows startup program list
		/// </summary>
		/// <param name="mode">The registration mode for the application, Off means that the application will no longer be started at Windows startup</param>
		public static void SetApplicationStartup(Registration mode) {
			using (RegistryKey key = Registry.CurrentUser.OpenSubKey(Settings.Default.REGISTRY_KEY, true)) {
				if (mode == Registration.On) {
					key.SetValue("Inbox Notifier", '"' + Application.ExecutablePath + '"');
				} else {
					key.DeleteValue("Inbox Notifier", false);
				}
			}
		}

		#endregion

		#region #accessors

		#endregion
	}
}