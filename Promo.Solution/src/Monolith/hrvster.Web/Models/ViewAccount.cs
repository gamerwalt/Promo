using hrvster.Models;
using hrvster.Models.Profiles;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class UpdateAccount
	{
		public string DisplayName { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public AccountDetails AccountDetails { get; set; }
		public string EmailAddress { get; set; }
		public string PhoneNumber { get; set; }
	}

	public class AccountDetails
	{
		public Account Account { get; set; }
		public int TotalPromotions { get; set; }
		public int TotalSweepstakes { get; set; }
		public int TotalSweepEntries { get; set; }
		public IEnumerable<Profile> Profiles { get; set; }
	}
}
