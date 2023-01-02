using Newtonsoft.Json;

namespace hrvster.Models
{
	public class WebjobModel
	{
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
