using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using notifier.Properties;

namespace notifier {
	class Scheduler {

		#region #attributes

		/// <summary>
		/// List of slots for the scheduler
		/// </summary>
		private List<Timeslot> Slots = JsonConvert.DeserializeObject<List<Timeslot>>(Settings.Default.SchedulerTimeSlot);

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

			// displays the start time and end time for monday
			Timeslot Monday = Slots.Find((match) => {
				return match.Day == DayOfWeek.Monday;
			});

			if (Monday != null) {
				UI.fieldStartTime.Text = Monday.Start.Hours.ToString() + "h";
				UI.fieldEndTime.Text = Monday.End.Hours.ToString() + "h";
				UI.labelDuration.Text = Monday.Start.Subtract(Monday.End).Duration().Hours.ToString() + "h";
			}
		}

		#endregion

		#region #accessors

		#endregion

	}
}
