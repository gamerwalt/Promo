using System;

namespace hrvster.Models.Harvester
{
	public class IncomingTweet
	{
		public int IncomingTweetId { get; set; }
		public Guid IncomingTweetUid { get; set; }
		public string Tweet { get; set; }
		public IncomingTweetStatus Status { get; set; }
		public bool ProcessedIndicator { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}
