using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notifier {
	class Timeslot {

		#region #attributes

		#endregion

		#region #methods

		/// <summary>
		/// Class constructor
		/// </summary>
		public Timeslot() {
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
