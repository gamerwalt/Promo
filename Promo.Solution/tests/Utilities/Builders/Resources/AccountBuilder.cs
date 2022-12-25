using hrvster.Models;
using System;

namespace TestUtilities.Builders.Resources
{
	public class AccountBuilder
	{
		public int AccountId { get; set; }
		public Guid AccountUid { get; set; }
		public string EmailAddress { get; set; }
		public bool EmailVerified { get; set; }
		public string PhoneNumber { get; set; }
		public string DisplayName { get; set; }

		public AccountBuilder(int accountId, Guid accountUid, string emailAddress, bool emailVerified, string phoneNumber, string displayName)
		{
			AccountId = accountId;
			AccountUid = accountUid;
			EmailAddress = emailAddress;
			EmailVerified = emailVerified;
			PhoneNumber = phoneNumber;
			DisplayName = displayName;
		}

		public AccountBuilder Create(int accountId, Guid accountUid, string emailAddress, bool emailVerified, string phoneNumber, string displayName)
			=> new AccountBuilder(accountId, accountUid, emailAddress, emailVerified, phoneNumber, displayName);

		public Account Build()
			=> new Account
			{
				AccountId = AccountId,
				AccountUid = AccountUid,
				EmailAddress = EmailAddress,
				EmailVerified = EmailVerified,
				PhoneNumber = PhoneNumber,
				DisplayName = DisplayName
			};
	}
}
