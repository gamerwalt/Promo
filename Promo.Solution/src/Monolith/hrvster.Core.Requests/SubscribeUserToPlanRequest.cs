using hrvster.Models.Core;
using hrvster.Models.Subscriptions;
using MediatR;
using System;

namespace hrvster.Core.Requests
{
	public class SubscribeUserToPlanRequest : IRequest<ServiceOperation<AccountSubscription>>
	{
		private Guid _userUid;
		private Plan _plan;

		public SubscribeUserToPlanRequest(Guid userUid, Plan plan)
		{
			_userUid = userUid;
			_plan = plan;
		}

		public Guid UserUId { get { return _userUid; } }
		public Plan Plan { get { return _plan; } }
	}
}
