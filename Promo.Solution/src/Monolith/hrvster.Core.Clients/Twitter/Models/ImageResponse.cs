using Newtonsoft.Json;

namespace hrvster.Core.Clients.Twitter.Models
{
	public class ImageResponse
	{
		[JsonProperty("media_id")]
		public double MediaId { get; set; }

		[JsonProperty("media_id_string")]
		public string MediaIdString { get; set; }

		[JsonProperty("size")]
		public long Size { get; set; }

		[JsonProperty("expires_after_secs")]
		public long ExpiresAfterSecs { get; set; }

		[JsonProperty("image")]
		public TwitterImage Image { get; set; }
	}
}
