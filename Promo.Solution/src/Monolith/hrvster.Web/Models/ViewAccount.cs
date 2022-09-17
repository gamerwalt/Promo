using hrvster.Models;
using hrvster.Models.Profiles;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class ViewAccount
	{
		public Account Account { get; set; }
		public int TotalPromotions {get;set;}
		public int TotalSweepstakes { get; set; }
		public int TotalSweepEntries { get; set; }
		public UpdateAccount UpdateAccount { get; set; }
		public IEnumerable<Profile> Profiles { get; internal set; }
		public UpdateEmailAddress UpdateEmailAddress { get; internal set; }
	}

	public class UpdateAccount
	{
		public string DisplayName { get; set; }
		public string LastName { get; internal set; }
		public string FirstName { get; internal set; }
	}

	public class UpdateEmailAddress
	{
		public string EmailAddress { get; set; }
	}

}
