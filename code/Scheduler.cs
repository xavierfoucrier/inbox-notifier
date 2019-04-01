using System;
using System.Collections.Generic;
using System.Globalization;
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
		/// Time type possibilities
		/// </summary>
		public enum TimeType : uint {
			Start = 0,
			End = 1
		}

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
			DisplayTimeSlotProperties(GetTimeSlot());
		}

		/// <summary>
		/// Displays the time slot properties on the interface
		/// </summary>
		public void DisplayTimeSlotProperties(TimeSlot slot) {
			if (slot == null) {
				UI.fieldStartTime.SelectedIndex = 0;
				UI.fieldEndTime.SelectedIndex = 0;
				UI.labelDuration.Text = Translation.theday;

				return;
			}

			UI.fieldStartTime.Text = slot.Start.ToString(@"h\:mm");
			UI.fieldEndTime.Text = slot.End.ToString(@"h\:mm");
			UI.labelDuration.Text = slot.Start.Subtract(slot.End).Duration().TotalHours.ToString() + " " + Translation.hours;
		}

		/// <summary>
		/// Updates the scheduler properties depending on the type of time
		/// </summary>
		/// <param name="type">Type of time</param>
		public void Update(TimeType type) {

			// gets the selected day of week
			DayOfWeek day = GetDayOfWeek(UI.fieldDayOfWeek.SelectedIndex);

			// removes the time slot for the selected day
			if ((type == TimeType.Start && UI.fieldStartTime.SelectedIndex == 0) || (type == TimeType.End && UI.fieldEndTime.SelectedIndex == 0)) {

				// removes the time slot from the scheduler
				RemoveTimeSlot(day);

				// udpates the interface
				UI.fieldStartTime.SelectedIndex = 0;
				UI.fieldEndTime.SelectedIndex = 0;
				UI.labelDuration.Text = Translation.theday;

				// synchronizes the inbox if the selected day of week is today
				if (GetDayOfWeek(UI.fieldDayOfWeek.SelectedIndex) == DateTime.Now.DayOfWeek) {
					UI.GmailService.Inbox.Sync();
				}

				return;
			}

			// updates the end time depending on the start time
			if (type == TimeType.Start) {
				if (UI.fieldStartTime.SelectedIndex > UI.fieldEndTime.SelectedIndex || UI.fieldEndTime.SelectedIndex == 0) {
					UI.fieldEndTime.SelectedIndex = UI.fieldStartTime.SelectedIndex;
				}
			}

			// updates the start time depending on the end time
			if (type == TimeType.End) {
				if (UI.fieldEndTime.SelectedIndex < UI.fieldStartTime.SelectedIndex || UI.fieldStartTime.SelectedIndex == 0) {
					UI.fieldStartTime.SelectedIndex = UI.fieldEndTime.SelectedIndex;
				}
			}

			// defines the start and end time of the time slot
			TimeSpan start = TimeSpan.Parse(UI.fieldStartTime.Text);
			TimeSpan end = TimeSpan.Parse(UI.fieldEndTime.Text);

			// adds or updates the time slot
			SetTimeSlot(day, start, end);

			// updates the duration label
			UI.labelDuration.Text = start.Subtract(end).Duration().TotalHours.ToString() + " " + Translation.hours;

			// synchronizes the inbox if the selected day of week is today
			if (GetDayOfWeek(UI.fieldDayOfWeek.SelectedIndex) == DateTime.Now.DayOfWeek) {
				UI.GmailService.Inbox.Sync();
			}
		}

		/// <summary>
		/// Pause the synchronization
		/// </summary>
		public void PauseSync() {

			// gets the time slot of today
			TimeSlot slot = GetTimeSlot();

			// displays the timeout icon
			UI.notifyIcon.Icon = Resources.timeout;
			UI.notifyIcon.Text = Translation.syncScheduled.Replace("{day}", CultureInfo.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek)).Replace("{start}", slot.Start.ToString(@"h\:mm")).Replace("{end}", slot.End.ToString(@"h\:mm"));

			// disables some menu items
			UI.menuItemSynchronize.Enabled = false;
			UI.menuItemMarkAsRead.Enabled = false;
			UI.menuItemTimout.Enabled = false;
			UI.menuItemSettings.Enabled = true;

			// updates some text items
			UI.menuItemMarkAsRead.Text = Translation.markAsRead;
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
		/// <param name="Day">The day for which the time slot is defined</param>
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
		/// <returns>A flag that tells if the inbox can be synced</returns>
		public bool ScheduledSync() {

			// gets the time slot of today
			TimeSlot slot = GetTimeSlot();

			// allows inbox syncing if there is no slot defined for today
			if (slot == null) {
				return true;
			}

			// checks if the current time is inside the time slot
			return DateTime.Now >= Convert.ToDateTime(slot.Start.ToString()) && DateTime.Now <= Convert.ToDateTime(slot.End.ToString());
		}

		#endregion

		#region #accessors

		#endregion

	}
}
