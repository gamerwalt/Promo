using hrvster.Models.Sweepstakes;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetSweepForAccountUidRequest : IRequest<SweepstakeDetails>
	{
		private Guid _userUid;

		public GetSweepForAccountUidRequest(Guid userUid)
		{
			_userUid = userUid;
		}

		public Guid AccountUid { get { return _userUid; } }
	}
}
