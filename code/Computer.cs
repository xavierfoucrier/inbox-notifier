using System;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using notifier.Properties;

namespace notifier {
	class Computer {

		// Reference to the main interface
		private Main Interface;

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="Form">Reference to the application main window</param>
		public Computer(ref Main Form) {
			Interface = Form;
		}

		/// <summary>
		/// Binds the "NetworkAvailabilityChanged" event to automatically sync the inbox when a network is available
		/// </summary>
		public void BindNetwork() {
			NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler((object source, NetworkAvailabilityEventArgs target) => {

				// stops the reconnect process if it is running
				if (Interface.GmailService.Inbox.GetReconnect() != 0) {
					Interface.timerReconnect.Enabled = false;
					Interface.timerReconnect.Interval = 100;
					Interface.GmailService.Inbox.SetReconnect(0);
				}

				// loops through all network interface to check network connectivity
				foreach (NetworkInterface network in NetworkInterface.GetAllNetworkInterfaces()) {

					// discards "non-up" status, modem, serial, loopback and tunnel
					if (network.OperationalStatus != OperationalStatus.Up || network.Speed < 0 || network.NetworkInterfaceType == NetworkInterfaceType.Loopback || network.NetworkInterfaceType == NetworkInterfaceType.Tunnel) {
						continue;
					}

					// discards virtual cards (like virtual box, virtual pc, etc.) and microsoft loopback adapter (showing as ethernet card)
					if (network.Name.ToLower().Contains("virtual") || network.Description.ToLower().Contains("virtual") || network.Description.ToLower() == ("microsoft loopback adapter")) {
						continue;
					}

					// syncs the inbox when a network interface is available and the timeout mode is disabled
					if (Interface.timer.Interval == Settings.Default.TimerInterval) {
						Interface.GmailService.Inbox.Sync();
					}

					break;
				}
			});
		}

		/// <summary>
		/// Binds the "PowerModeChanged" event to automatically pause/resume the application synchronization
		/// </summary>
		public void BindPowerMode() {
			SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler((object source, PowerModeChangedEventArgs target) => {
				if (target.Mode == PowerModes.Suspend) {
					Interface.timer.Enabled = false;
				} else if (target.Mode == PowerModes.Resume) {

					// do nothing if the timeout mode is set to infinite
					if (Interface.timer.Interval != Settings.Default.TimerInterval && Interface.menuItemTimeoutIndefinitely.Checked) {
						return;
					}
					
					Interface.GmailService.Inbox.Sync(false, true);
				}
			});
		}

		/// <summary>
		/// Binds the "SessionSwitch" event to automatically sync the inbox on session unlocking
		/// </summary>
		public void BindSessionSwitch() {
			SystemEvents.SessionSwitch += new SessionSwitchEventHandler((object source, SessionSwitchEventArgs target) => {

				// syncs the inbox when the user is unlocking the Windows session
				if (target.Reason == SessionSwitchReason.SessionUnlock) {

					// do nothing if the timeout mode is set to infinite
					if (Interface.timer.Interval != Settings.Default.TimerInterval && Interface.menuItemTimeoutIndefinitely.Checked) {
						return;
					}

					Interface.GmailService.Inbox.Sync(false, true);
				}
			});
		}

		/// <summary>
		/// Opens the Google website to checks the internet connectivity
		/// </summary>
		/// <returns>Indicates if the user is connected to the internet, false means that the request to the Google server has failed</returns>
		public bool IsInternetAvailable() {
			try {
				using (var client = new WebClient()) {
					using (var stream = client.OpenRead("http://www.google.com")) {
						return true;
					}
				}
			} catch (Exception) {
				return false;
			}
		}
	}
}
