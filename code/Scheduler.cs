using System.Collections.Generic;
using Newtonsoft.Json;

namespace notifier {
	class Scheduler {

		#region #attributes

		/// <summary>
		/// List of slots for the scheduler
		/// </summary>
		private List<Timeslot> Slots;

		/// <summary>
		/// Reference to the main interface
		/// </summary>
		private Main UI;

		#endregion

		#region #methods

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="form">Reference to the application main window</param>
		public Scheduler(ref Main form) {
			UI = form;
		}

		#endregion

		#region #accessors

		#endregion

	}
}
