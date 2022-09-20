using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Accounts
{
	public class UpdateAccountRequest : IRequest
	{
		private Guid _accountUid;
		private string _displayName;
		private string _firstName;
		private string _lastName;

		public UpdateAccountRequest(Guid accountUid, string displayName, string firstName, string lastName)
		{
			_accountUid = accountUid;
			_displayName = displayName;
			_firstName = firstName;
			_lastName = lastName;
		}

		public Guid AccountUid { get { return _accountUid; } }
		public string DisplayName { get { return _displayName; } }
		public string FirstName { get { return _firstName; } }
		public string LastName { get { return _lastName; } }
	}
}
