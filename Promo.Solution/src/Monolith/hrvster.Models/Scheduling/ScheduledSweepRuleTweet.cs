using System;

namespace hrvster.Models.Scheduling
{
	public class ScheduledSweepRuleTweet
	{
		public int ScheduledId { get; set; }
		public Guid ScheduledUid { get; set; }
		public string Text { get; set; }
		public DateTime ScheduledTime { get; set; }
		public DateTime ScheduledTimeUtc { get; set; }
		public ScheduledStatus Status { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
		public Rule Rule { get; set; }
	}
}
