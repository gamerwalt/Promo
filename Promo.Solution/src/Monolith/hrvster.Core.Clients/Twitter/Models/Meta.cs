using Newtonsoft.Json;

namespace hrvster.Core.Clients.Twitter.Models
{
	public class Meta
	{
		[JsonProperty("result_count")]
		public long ResultCount { get; set; }
		[JsonProperty("next_token")]
		public string NextToken { get; set; }
	}
}
