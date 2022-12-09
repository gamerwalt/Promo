using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Accounts
{
	public class UpdateLastLoginRequest : IRequest
	{
		private string _accountUid;

		public UpdateLastLoginRequest(string accountUid)
		{
			_accountUid = accountUid;
		}

		public Guid AccountUid
		{
			get { return Guid.Parse(_accountUid); }
		}
	}
}
