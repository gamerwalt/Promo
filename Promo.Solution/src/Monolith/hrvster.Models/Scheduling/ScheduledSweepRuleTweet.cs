using System;
using System.Collections.Generic;

namespace hrvster.Models.Scheduling
{
	public class ScheduledSweepRuleTweet
	{
		public ScheduledSweepRuleTweet()
		{
			Images = new List<ScheduledSweepRuleTweetImage>();
		}

		public int ScheduledId { get; set; }
		public Guid ScheduledUid { get; set; }
		public string Text { get; set; }
		public DateTime ScheduledTime { get; set; }
		public DateTime ScheduledTimeUtc { get; set; }
		public ScheduledStatus Status { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
		public Rule Rule { get; set; }
		public List<ScheduledSweepRuleTweetImage> Images { get; set; }
	}
}
