using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Accounts
{
	public class UpdateLastLoginRequest : IRequest
	{
		private string _accountUid;
		private HrvsterUser _hrvsterUser;

		public UpdateLastLoginRequest(string accountUid, HrvsterUser hrvsterUser)
		{
			_accountUid = accountUid;
			_hrvsterUser = hrvsterUser;
		}

		public Guid AccountUid { get { return Guid.Parse(_accountUid); } }
		public HrvsterUser HrvsterUser { get { return _hrvsterUser; } }
	}
}
