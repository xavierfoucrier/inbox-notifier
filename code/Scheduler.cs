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
		/// Days list using Monday as first day of week
		/// </summary>
		public readonly List<DayOfWeek> Days = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().OrderBy(day => { return (day - DayOfWeek.Monday + 7) % 7; }).ToList();

		/// <summary>
		/// List of slots for the scheduler
		/// </summary>
		private List<TimeSlot> Slots;

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

			// inits the slots depending on the user settings
			Slots = Settings.Default.SchedulerTimeSlot != "" ? JsonConvert.DeserializeObject<List<TimeSlot>>(Settings.Default.SchedulerTimeSlot) : new List<TimeSlot>();

			// displays the default day of week based on today
			UI.fieldDayOfWeek.SelectedIndex = Days.IndexOf(DateTime.Now.DayOfWeek);

			// displays the start time and end time for today
			TimeSlot slot = GetTimeSlot();

			if (slot != null) {
				UI.fieldStartTime.Text = slot.Start.ToString(@"h\:mm");
				UI.fieldEndTime.Text = slot.End.ToString(@"h\:mm");
				UI.labelDuration.Text = slot.Start.Subtract(slot.End).Duration().Hours.ToString() + " " + Translation.hours;
			} else {
				UI.fieldStartTime.Text = "-";
				UI.fieldEndTime.Text = "-";
				UI.labelDuration.Text = "0 " + Translation.hours;
			}
		}

		/// <summary>
		/// Gets a specific day of week by index using Monday as reference for the starting day of week
		/// </summary>
		/// <param name="index">Index of the day in the list</param>
		/// <returns>The day of week at the specific index</returns>
		public DayOfWeek GetDayOfWeek(int index) {
			return Days.ElementAt(index);
		}

		/// <summary>
		/// Searches a time slot for today
		/// </summary>
		/// <returns>The time slot of today</returns>
		public TimeSlot GetTimeSlot() {
			return Slots.Find((match) => {
				return match.Day == DateTime.Now.DayOfWeek;
			});
		}

		/// <summary>
		/// Searches a time slot for a specific day
		/// </summary>
		/// <param name="day">The day for which to find a time slot</param>
		/// <returns>The time slot of the day</returns>
		public TimeSlot GetTimeSlot(DayOfWeek day) {
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

		/// <summary>
		/// Removes a time slot from the list
		/// </summary>
		/// <param name="Day"></param>
		public void RemoveTimeSlot(DayOfWeek Day) {
			TimeSlot slot = GetTimeSlot(Day);

			if (slot == null) {
				return;
			}

			Slots.Remove(slot);

			// saves all slots
			Settings.Default.SchedulerTimeSlot = JsonConvert.SerializeObject(Slots);
		}

		/// <summary>
		/// Detects if the synchronization is scheduled
		/// </summary>
		/// <returns>A flag that tells if the inbox can be synched</returns>
		public bool ScheduledSync() {

			// gets the time slot of today
			TimeSlot slot = GetTimeSlot();

			// allows inbox syncing if there is no slot defined for today
			if (slot == null) {
				return true;
			}

			// checks if the current time is inside the time slot
			DateTime now = DateTime.Now;
			bool end = now.Hour <= slot.End.Hours;

			if (now.Hour == slot.End.Hours) {
				end = end && now.Minute == 0;
			}

			return now.Hour >= slot.Start.Hours && end;
		}

		#endregion

		#region #accessors

		#endregion

	}
}
