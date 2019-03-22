using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notifier {
	class Timeslot {

		#region #attributes

		/// <summary>
		/// Day of the timeslot
		/// </summary>
		private DayOfWeek Day;

		/// <summary>
		/// Starting time of the timeslot
		/// </summary>
		private TimeSpan Start;

		/// <summary>
		/// Ending time of the timeslot
		/// </summary>
		private TimeSpan End;

		#endregion

		#region #methods

		/// <summary>
		/// Class constructor
		/// </summary>
		public Timeslot() {
		}

		#endregion

		#region #accessors

		#endregion
	}
}
