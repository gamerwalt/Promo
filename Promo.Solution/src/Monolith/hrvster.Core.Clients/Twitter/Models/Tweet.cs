using Newtonsoft.Json;

namespace hrvster.Core.Clients.Twitter.Models
{
	public class Tweet
	{
		[JsonProperty("edit_history_tweet_ids")]
		public string[] EditHistoryTweetIds { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }
	}
}
