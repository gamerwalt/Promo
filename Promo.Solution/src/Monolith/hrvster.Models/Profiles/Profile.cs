using System;

namespace hrvster.Models.Profiles
{
	public class Profile
	{
		public Account Account { get; set; }
		public string ProfileImageUrl { get; set; }
		public string Location { get; set; }
		public string AccessTokenSecret { get; set; }
		public string AccessToken { get; set; }
		public string Description { get; set; }
		public string ScreenName { get; set; }
		public string DisplayName { get; set; }
		public string ProfileType { get; set; }
		public Guid ProfileUid { get; set; }
	}
}
