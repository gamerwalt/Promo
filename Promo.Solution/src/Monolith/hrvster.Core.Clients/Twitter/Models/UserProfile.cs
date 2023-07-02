using Newtonsoft.Json;
using System;

namespace hrvster.Core.Clients.Twitter.Models
{
	public class UserProfile
	{
		[JsonProperty("public_metrics")]
		public UserMetrics PublicMetrics { get; set; }

		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("protected")]
		public bool Protected { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("verified")]
		public bool Verified { get; set; }

		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("profile_image_url")]
		public string ProfileImageUrl { get; set; }

		[JsonProperty("username")]
		public string Username { get; set; }
	}
}
