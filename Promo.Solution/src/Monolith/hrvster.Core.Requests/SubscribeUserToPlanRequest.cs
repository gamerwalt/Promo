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
		private bool _isTrial;

		public SubscribeUserToPlanRequest(Guid userUid, Plan plan, bool isTrial = false)
		{
			_userUid = userUid;
			_plan = plan;
			_isTrial = isTrial;
		}

		public Guid UserUId { get { return _userUid; } }
		public Plan Plan { get { return _plan; } }
		public bool IsTrial { get { return _isTrial; } }
	}
}
