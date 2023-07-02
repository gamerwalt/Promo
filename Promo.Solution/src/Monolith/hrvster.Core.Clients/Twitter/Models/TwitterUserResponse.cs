using Newtonsoft.Json;

namespace hrvster.Core.Clients.Twitter.Models
{
	public class TwitterUserResponse
	{
		[JsonProperty("data")]
		public UserProfile Profile { get; set; }
	}
}
