using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Accounts
{
	public class GetTotalResourcesRequest : IRequest<TotalResources>
	{
		private Guid _accountUid;

		public GetTotalResourcesRequest(Guid accountUid)
		{
			_accountUid = accountUid;
		}

		public Guid AccountUid { get { return _accountUid; } }
	}
}
