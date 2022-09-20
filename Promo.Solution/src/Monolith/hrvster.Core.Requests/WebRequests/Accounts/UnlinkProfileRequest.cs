using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Accounts
{
	public class UnlinkProfileRequest : IRequest
	{
		private Guid _accountUid;
		private ProfileType _profileType;

		public UnlinkProfileRequest(Guid userUid, ProfileType profileType)
		{
			_accountUid = userUid;
			_profileType = profileType;
		}

		public Guid AccountUid { get { return _accountUid; } }
		public ProfileType ProfileType { get { return _profileType; } }
	}
}
