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
		/// Day list using Monday as first day of week
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

			// init the slots depending on the user settings
			Slots = Settings.Default.SchedulerTimeSlot != "" ? JsonConvert.DeserializeObject<List<TimeSlot>>(Settings.Default.SchedulerTimeSlot) : new List<TimeSlot>();

			// display the default day of week based on today
			UI.fieldDayOfWeek.SelectedIndex = Days.IndexOf(DateTime.Now.DayOfWeek);

			// display the start time and end time for today
			DisplayTimeSlotProperties(GetTimeSlot());
		}

		/// <summary>
		/// Display the time slot properties on the interface
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
		/// Update the scheduler properties depending on the type of time
		/// </summary>
		/// <param name="type">Type of time</param>
		public void Update(TimeType type) {

			// get the selected day of week
			DayOfWeek day = GetDayOfWeek(UI.fieldDayOfWeek.SelectedIndex);

			// remove the time slot for the selected day
			if ((type == TimeType.Start && UI.fieldStartTime.SelectedIndex == 0) || (type == TimeType.End && UI.fieldEndTime.SelectedIndex == 0)) {

				// remove the time slot from the scheduler
				RemoveTimeSlot(day);

				// udpate the interface
				UI.fieldStartTime.SelectedIndex = 0;
				UI.fieldEndTime.SelectedIndex = 0;
				UI.labelDuration.Text = Translation.theday;

				// synchronize the inbox if the selected day of week is today
				if (GetDayOfWeek(UI.fieldDayOfWeek.SelectedIndex) == DateTime.Now.DayOfWeek) {
					UI.GmailService.Inbox.Sync();
				}

				return;
			}

			// update the end time depending on the start time
			if (type == TimeType.Start) {
				if (UI.fieldStartTime.SelectedIndex > UI.fieldEndTime.SelectedIndex || UI.fieldEndTime.SelectedIndex == 0) {
					UI.fieldEndTime.SelectedIndex = UI.fieldStartTime.SelectedIndex;
				}
			}

			// update the start time depending on the end time
			if (type == TimeType.End) {
				if (UI.fieldEndTime.SelectedIndex < UI.fieldStartTime.SelectedIndex || UI.fieldStartTime.SelectedIndex == 0) {
					UI.fieldStartTime.SelectedIndex = UI.fieldEndTime.SelectedIndex;
				}
			}

			// define the start and end time of the time slot
			TimeSpan start = TimeSpan.Parse(UI.fieldStartTime.Text);
			TimeSpan end = TimeSpan.Parse(UI.fieldEndTime.Text);

			// add or update the time slot
			SetTimeSlot(day, start, end);

			// update the duration label
			UI.labelDuration.Text = start.Subtract(end).Duration().TotalHours.ToString() + " " + Translation.hours;

			// synchronize the inbox if the selected day of week is today
			if (GetDayOfWeek(UI.fieldDayOfWeek.SelectedIndex) == DateTime.Now.DayOfWeek) {
				UI.GmailService.Inbox.Sync();
			}
		}

		/// <summary>
		/// Pause the synchronization
		/// </summary>
		public void PauseSync() {

			// get the time slot of today
			TimeSlot slot = GetTimeSlot();

			// display the timeout icon
			UI.notifyIcon.Icon = Resources.timeout;
			UI.notifyIcon.Text = Translation.syncScheduled.Replace("{day}", CultureInfo.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek)).Replace("{start}", slot.Start.ToString(@"h\:mm")).Replace("{end}", slot.End.ToString(@"h\:mm"));

			// disable some menu items
			UI.menuItemSynchronize.Enabled = false;
			UI.menuItemMarkAsRead.Enabled = false;
			UI.menuItemTimout.Enabled = false;
			UI.menuItemSettings.Enabled = true;

			// update some text items
			UI.menuItemMarkAsRead.Text = Translation.markAsRead;
		}

		/// <summary>
		/// Get a specific day of week by index using Monday as reference for the starting day of week
		/// </summary>
		/// <param name="index">Index of the day in the list</param>
		/// <returns>The day of week at the specific index</returns>
		public DayOfWeek GetDayOfWeek(int index) {
			return Days.ElementAt(index);
		}

		/// <summary>
		/// Search a time slot for today
		/// </summary>
		/// <returns>The time slot of today</returns>
		public TimeSlot GetTimeSlot() {
			return Slots.Find((match) => {
				return match.Day == DateTime.Now.DayOfWeek;
			});
		}

		/// <summary>
		/// Search a time slot for a specific day
		/// </summary>
		/// <param name="day">The day for which to find a time slot</param>
		/// <returns>The time slot of the day</returns>
		public TimeSlot GetTimeSlot(DayOfWeek day) {
			return Slots.Find((match) => {
				return match.Day == day;
			});
		}

		/// <summary>
		/// Add or update a time slot to the scheduler
		/// </summary>
		/// <param name="day">Day of week</param>
		/// <param name="start">Start time of the time slot</param>
		/// <param name="end">End time of the time slot</param>
		public void SetTimeSlot(DayOfWeek day, TimeSpan start, TimeSpan end) {
			int index = Slots.FindIndex((match) => {
				return match.Day == day;
			});

			if (index != -1) {
				Slots[index].Start = start;
				Slots[index].End = end;
			} else {
				Slots.Add(new TimeSlot(day, start, end));
			}

			// save all slots
			Settings.Default.SchedulerTimeSlot = JsonConvert.SerializeObject(Slots);
		}

		/// <summary>
		/// Remove a time slot from the list
		/// </summary>
		/// <param name="day">The day for which the time slot is defined</param>
		public void RemoveTimeSlot(DayOfWeek day) {
			TimeSlot slot = GetTimeSlot(day);

			if (slot == null) {
				return;
			}

			Slots.Remove(slot);

			// save all slots
			Settings.Default.SchedulerTimeSlot = JsonConvert.SerializeObject(Slots);
		}

		/// <summary>
		/// Detect if the synchronization is scheduled
		/// </summary>
		/// <returns>A flag that tells if the inbox can be synced</returns>
		public bool ScheduledSync() {

			// get the time slot of today
			TimeSlot slot = GetTimeSlot();

			// allow inbox syncing if there is no slot defined for today
			if (slot == null) {
				return true;
			}

			// check if the current time is inside the time slot
			return DateTime.Now >= Convert.ToDateTime(slot.Start.ToString()) && DateTime.Now <= Convert.ToDateTime(slot.End.ToString());
		}

		#endregion

		#region #accessors

		#endregion

	}
}
