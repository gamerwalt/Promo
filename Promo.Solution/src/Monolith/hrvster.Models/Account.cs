using System;

namespace hrvster.Models
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
		public string TwitterUserName { get; set; }

		public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
		public string VerificationLink { get; set; }
	}
}
