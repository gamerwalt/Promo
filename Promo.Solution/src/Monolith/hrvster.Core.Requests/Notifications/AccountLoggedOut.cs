using MediatR;
using System;

namespace hrvster.Core.Requests.Notifications
{
	public class AccountLoggedOut : INotification
	{
		private Guid _accountUid;

		public AccountLoggedOut(string accountUid)
		{
			_accountUid = Guid.Parse(accountUid);
		}

		public Guid AccountUid { get { return _accountUid; } }
	}
}
