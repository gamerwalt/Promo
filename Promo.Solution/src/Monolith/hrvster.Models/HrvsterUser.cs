using System;

namespace hrvster.Models
{
	public class HrvsterUser
	{
		public Guid UserUid { get; set; }
		public string EmailAddress { get; set; }
		public string Token { get; set; }
		public string DisplayName { get; set; }
		public string TwitterUserName { get; set; }
		public string Locale { get; set; }
		public string WindowsTimeZone { get; set; }
		public bool IsDaylight { get; set; }
	}
}
