using System;

namespace hrvster.Models.Options
{
	public class PosterOptions
	{
		public const string SectionName = "PosterSettings";

		public string TweetIdToReplyTo { get; set; }
		public string ProfileNameToReplyTo { get; set; }
		public Guid AccountUidToLogonWith { get; set; }
		public string Hashtag { get; set; }
		public int MinutesApart { get; set; }
		public string Messages { get; set; }
		public int MaxMinutesToContinuePosting { get; set; }
	}
}
