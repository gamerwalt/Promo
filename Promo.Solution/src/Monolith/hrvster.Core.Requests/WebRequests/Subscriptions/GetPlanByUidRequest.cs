using hrvster.Models.Subscriptions;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Subscriptions
{
	public class GetPlanByUidRequest : IRequest<Plan>
	{
		private Guid _planUid;

		public GetPlanByUidRequest(Guid planUid)
		{
			_planUid = planUid;
		}

		public Guid PlanUid { get { return _planUid; } }
	}
}
