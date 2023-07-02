using Newtonsoft.Json;
using System.Collections.Generic;

namespace hrvster.Core.Clients.Twitter.Models
{

	public class FollowerReponseObject
	{
		[JsonProperty("data")]
		public List<Follower> Data { get; set; }
		[JsonProperty("meta")]
		public Meta Meta { get; set; }
	}
}
