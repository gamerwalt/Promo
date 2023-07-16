using Newtonsoft.Json;

namespace hrvster.Core.Clients.Twitter.Models
{
	public class Meta
	{
		[JsonProperty("result_count")]
		public long ResultCount { get; set; }
		[JsonProperty("next_token")]
		public string NextToken { get; set; }
		[JsonProperty("newest_id")]
		public string NewestId { get; set; }

		[JsonProperty("oldest_id")]
		public string OldestId { get; set; }
	}
}
