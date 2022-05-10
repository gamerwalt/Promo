using System;

namespace hrvster.Core.Authentication
{
	public class HrvsterUser
	{
		public Guid UserUid { get; set; }
		public string EmailAddress { get; set; }
		public string Token { get; set; }
		public string DisplayName { get; set; }
		public string TwitterUserName { get; set; }
	}
}
