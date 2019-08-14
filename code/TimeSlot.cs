using System;
using Newtonsoft.Json;

namespace notifier {
	class TimeSlot {

		#region #attributes

		#endregion

		#region #methods

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="day">Day of the week</param>
		/// <param name="start">Start time of the time slot</param>
		/// <param name="end">End time of the time slot</param>
		public TimeSlot(DayOfWeek day, TimeSpan start, TimeSpan end) {
			Day = day;
			Start = start;
			End = end;
		}

		#endregion

		#region #accessors

		/// <summary>
		/// Day of the time slot
		/// </summary>
		[JsonConverter(typeof(DayConverter))]
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

		/// <summary>
		/// Total hours of the time slot
		/// </summary>
		public double TotalHours {
			get {
				return Start.Subtract(End).Duration().TotalHours;
			}
		}

		#endregion
	}

	class DayConverter : JsonConverter {
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			writer.WriteValue(value.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), reader.Value.ToString());
		}

		public override bool CanConvert(Type objectType) {
			return objectType == typeof(DayOfWeek);
		}
	}
}
