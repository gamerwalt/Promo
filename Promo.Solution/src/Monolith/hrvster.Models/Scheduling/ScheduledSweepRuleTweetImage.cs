using System;

namespace hrvster.Models.Scheduling
{
	public class ScheduledSweepRuleTweetImage
	{
		public int ScheduledImageId { get; set; }
		public int ScheduledSweepRuleTweetId { get; set; }
		public string ImageLocation { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
