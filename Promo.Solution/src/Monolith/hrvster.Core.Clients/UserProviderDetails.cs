using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Clients
{
	public class UserProviderDetails
	{
		public string Name { get; internal set; }
		public string ScreenName { get; internal set; }
		public string Description { get; internal set; }
		public string AccessToken { get; internal set; }
		public string AccessTokenSecret { get; internal set; }
		public string Location { get; internal set; }
		public string ProfileImage { get; internal set; }
		public string BearerToken { get; internal set; }
		public string ExternalProfileId { get; internal set; }
		public string ConsumerKey { get; internal set; }
		public string ConsumerSecret { get; internal set; }
	}
}
