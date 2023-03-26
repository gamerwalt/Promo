using System;

namespace hrvster.Models.Harvester
{
	public class BlacklistedSweepUser : BlacklistedUser
	{
		public Guid SweepUid { get; set; }
		public long TweetUserId { get; set; }
	}

	public class BlacklistedUser
	{
		public Guid GameUid { get; set; }
		public string UserId { get; set; }
	}
}
