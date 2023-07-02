using Newtonsoft.Json;

namespace hrvster.Core.Clients.Twitter.Models
{
	public class TwitterImage
	{
		[JsonProperty("image_type")]
		public string ImageType { get; set; }

		[JsonProperty("w")]
		public long Width { get; set; }

		[JsonProperty("h")]
		public long Height { get; set; }
	}
}
