using System;

namespace hrvster.Models.Harvester
{
	public class BlacklistedSweepUser
	{
		public Guid SweepUid { get; set; }
		public long TweetUserId { get; set; }
	}
}
