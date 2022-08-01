using System;

namespace hrvster.Models.Harvester
{
	public class Harvested
	{
		public int HarvestedId { get; set; }
		public Guid RuleUid { get; set; }
		public Guid HarvestedUid { get; set; }
		public Guid TweetUid { get; set; }
		public string ExternalTweetId { get; set; }
		public string TweetText { get; set; }
		public Guid UserUid { get; set; }
		public string ExternalUserId { get; set; }
		public string ProfileImageUrl { get; set; }
		public string Location { get; set; }
		public string Description { get; set; }
		public string ExternalScreenName { get; set; }
	}
}
