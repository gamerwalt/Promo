using hrvster.Models.Core;
using hrvster.Models.Subscriptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class SubscribeUserToPlanRequest : IRequest<ServiceOperation<AccountSubscription>>
	{
		private Guid _userUid;
		private Plan _plan;

		public SubscribeUserToPlanRequest(Guid userUid, Plan plan)
		{
			userUid = userUid;
			plan = plan;
		}

		public Guid UserUId { get { return _userUid; } }
		public Plan Plan { get { return _plan; } }
	}
}
