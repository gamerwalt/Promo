using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Models.Twitter
{
	public class Tweet
	{
		public string ScreenName { get; set; }
		public string ExternalUserId { get; set; }
		public string UserDescription { get; set; }
		public string TweetId { get; set; }
		public List<string> Hashtags { get; set; }
		public string TweetText { get;set; }
		public string InReplyToTweetId { get; set; }
		public string ProfileImageUrl { get; set; }
		public bool Verified { get; set; }
		public string Location { get; set; }
		public bool Retweeted { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
