using System;

namespace hrvster.Models.Core
{
	public class UserActivity
	{
		public int ActivityId { get; set; }
		public Guid ActivityUid { get; set; }
		public Account Account { get; set; }
		public int LinkedId { get; set; }
		public Guid LinkedUid { get; set; }
		public ActivityType ActivityType { get; set; }
		public string ActivityTableBaseName { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime CreatedDateUTC { get; set; }
	}
}
