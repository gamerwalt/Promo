using Newtonsoft.Json;

namespace hrvster.Core.Clients.Twitter.Models
{
	public class TweetResponse
	{
		[JsonProperty("data")]
		public Tweet Tweet { get; set; }
	}
}
