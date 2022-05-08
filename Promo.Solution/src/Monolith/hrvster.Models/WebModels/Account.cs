using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Models.WebModels
{
	public class Account
	{
		public int AccountId { get; set; }
		public string EmailAddress { get; set; }
		public bool EmailVerified { get; set; }
		public string PhoneNumber { get; set; }
		public string DisplayName { get; set; }
		public bool PhoneNumberVerified { get; set; }
		public string Pin { get; set; }
		public Guid AccountUid { get; set; }
		public bool TermsAccepted { get; set; }
	}
}
