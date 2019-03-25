using System;
using System.Collections.Generic;
using System.Linq;
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
		/// Gets a specific day of week by index using Monday as reference for the starting day of week
		/// </summary>
		/// <param name="index">Index of the day in the list</param>
		/// <returns>The day of week at the specific index</returns>
		public DayOfWeek GetDayOfWeek(int index) {
			return Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().OrderBy(day => {
				return (day - DayOfWeek.Monday + 7) % 7;
			}).ElementAt(index);
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

		/// <summary>
		/// Adds or updates a time slot to the scheduler
		/// </summary>
		/// <param name="Day">Day of week</param>
		/// <param name="Start">Start time of the time slot</param>
		/// <param name="End">End time of the time slot</param>
		public void SetTimeSlot(DayOfWeek Day, TimeSpan Start, TimeSpan End) {
			int index = Slots.FindIndex((match) => {
				return match.Day == Day;
			});

			if (index != -1) {
				Slots[index].Start = Start;
				Slots[index].End = End;
			} else {
				Slots.Add(new TimeSlot(Day, Start, End));
			}

			// saves all slots
			Settings.Default.SchedulerTimeSlot = JsonConvert.SerializeObject(Slots);
		}

		#endregion

		#region #accessors

		#endregion

	}
}
