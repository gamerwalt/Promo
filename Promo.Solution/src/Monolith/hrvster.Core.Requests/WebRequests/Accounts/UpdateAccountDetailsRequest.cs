using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace hrvster.Core.Requests.WebRequests.Accounts
{
	public class UpdateAccountDetailsRequest : IRequest
	{
		private Guid _accountUid;
		private string _emailAddress;
		private string _phoneNumber;

		public UpdateAccountDetailsRequest(Guid accountUid, string emailAddress, string phoneNumber)
		{
			_accountUid = accountUid;
			_emailAddress = emailAddress;
			_phoneNumber = phoneNumber;
		}

		public Guid AccountUid { get { return _accountUid; } }
		public string EmailAddress { get { return _emailAddress; } }
		public string PhoneNumber { get { return _phoneNumber; } }
	}
}
