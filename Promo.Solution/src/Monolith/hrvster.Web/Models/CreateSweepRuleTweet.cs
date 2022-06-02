using System;

namespace hrvster.Web.Models
{
	public class CreateSweepRuleTweet
	{
		public string TweetText { get; set; }
		public int SweepstakeId { get; internal set; }
		public Guid SweepstakeUid { get; internal set; }
		public string SweepstakeName { get; internal set; }
		public string ReturnAction { get; internal set; }
		public string ReturnController { get; internal set; }
	}
}
