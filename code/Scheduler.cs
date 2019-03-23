using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using notifier.Languages;
using notifier.Properties;

namespace notifier {
	class Scheduler {

		#region #attributes

		/// <summary>
		/// List of slots for the scheduler
		/// </summary>
		private List<TimeSlot> Slots = JsonConvert.DeserializeObject<List<TimeSlot>>(Settings.Default.SchedulerTimeSlot);

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
			if (Slots != null) {
				TimeSlot Monday = GetTimeSlot(DayOfWeek.Monday);

				if (Monday != null) {
					UI.fieldStartTime.Text = Monday.Start.Hours.ToString() + ":00";
					UI.fieldEndTime.Text = Monday.End.Hours.ToString() + ":00";
					UI.labelDuration.Text = Monday.Start.Subtract(Monday.End).Duration().Hours.ToString() + " " + Translation.hours;
				}
			}
		}

		/// <summary>
		/// Searches for a time slot for a specific day
		/// </summary>
		/// <param name="day">The day for which to find a time slot</param>
		/// <returns>The time slot of the day</returns>
		public TimeSlot GetTimeSlot(DayOfWeek day) {
			if (Slots == null) {
				return null;
			}

			return Slots.Find((match) => {
				return match.Day == day;
			});
		}

		#endregion

		#region #accessors

		#endregion

	}
}
