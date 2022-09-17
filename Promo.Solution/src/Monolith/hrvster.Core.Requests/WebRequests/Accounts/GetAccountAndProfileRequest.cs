using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Accounts
{
	public class GetAccountAndProfileRequest : IRequest<Account>
	{
		private Guid _accountUid;

		public GetAccountAndProfileRequest(Guid accountUid)
		{
			_accountUid = accountUid;
		}

		public Guid AccountUid { get { return _accountUid; } }
	}
}
