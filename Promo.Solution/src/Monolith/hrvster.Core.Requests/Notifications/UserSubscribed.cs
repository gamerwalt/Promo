using hrvster.Models.Subscriptions;
using MediatR;
using System;

namespace hrvster.Core.Requests.Notifications
{
	public class UserSubscribed : INotification
	{
		private Guid _accountUid;
		private Plan _plan;

		public UserSubscribed(Guid accountUid, Plan plan)
		{
			_accountUid = accountUid;
			_plan = plan;
		}

		public Guid AccountUid { get { return _accountUid; } }
		public Plan Plan { get { return _plan; } }
	}
}
