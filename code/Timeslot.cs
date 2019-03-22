using System;

namespace notifier {
	class Timeslot {

		#region #attributes

		#endregion

		#region #methods

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="Day">Day of the week</param>
		/// <param name="Start">Start time of the timeslot</param>
		/// <param name="End">End time of the timeslot</param>
		public Timeslot(DayOfWeek Day, TimeSpan Start, TimeSpan End) {
			this.Day = Day;
			this.Start = Start;
			this.End = End;
		}

		#endregion

		#region #accessors

		/// <summary>
		/// Day of the timeslot
		/// </summary>
		public DayOfWeek Day {
			get; set;
		}

		/// <summary>
		/// Starting time of the timeslot
		/// </summary>
		public TimeSpan Start {
			get; set;
		}

		/// <summary>
		/// Ending time of the timeslot
		/// </summary>
		public TimeSpan End {
			get; set;
		}

		#endregion
	}
}
