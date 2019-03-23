using System;

namespace notifier {
	class TimeSlot {

		#region #attributes

		#endregion

		#region #methods

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="Day">Day of the week</param>
		/// <param name="Start">Start time of the time slot</param>
		/// <param name="End">End time of the time slot</param>
		public TimeSlot(DayOfWeek Day, TimeSpan Start, TimeSpan End) {
			this.Day = Day;
			this.Start = Start;
			this.End = End;
		}

		#endregion

		#region #accessors

		/// <summary>
		/// Day of the time slot
		/// </summary>
		public DayOfWeek Day {
			get; set;
		}

		/// <summary>
		/// Starting time of the time slot
		/// </summary>
		public TimeSpan Start {
			get; set;
		}

		/// <summary>
		/// Ending time of the time slot
		/// </summary>
		public TimeSpan End {
			get; set;
		}

		#endregion
	}
}
