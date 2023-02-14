using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Subscriptions
{
	public class UnsubscribeAccountRequest : IRequest
	{
		private Guid _accountUid;

		public UnsubscribeAccountRequest(Guid accountUid)
		{
			_accountUid = accountUid;
		}

		public Guid AccountUid { get { return _accountUid; } }
	}
}
