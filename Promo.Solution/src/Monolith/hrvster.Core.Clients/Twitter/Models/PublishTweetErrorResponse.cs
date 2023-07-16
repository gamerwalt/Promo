using Newtonsoft.Json;

namespace hrvster.Core.Clients.Twitter.Models
{
	public class PublishTweetErrorResponse
	{
		[JsonProperty("detail")]
		public string Detail { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("status")]
		public long Status { get; set; }
	}
}
