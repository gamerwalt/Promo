using System;

namespace hrvster.Models.Facebook
{
	public class FacebookPage
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string PageAccessToken { get; set; }
		public Guid Uid { get; set; }
	}
}
