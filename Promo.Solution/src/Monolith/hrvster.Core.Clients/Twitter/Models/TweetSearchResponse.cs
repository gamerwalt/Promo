using Newtonsoft.Json;
using System.Collections.Generic;

namespace hrvster.Core.Clients.Twitter.Models
{
	public class TweetSearchResponse
	{
		[JsonProperty("data")]
		public List<Tweet> Tweets { get; set; }

		[JsonProperty("meta")]
		public Meta Meta { get; set; }
	}
}
