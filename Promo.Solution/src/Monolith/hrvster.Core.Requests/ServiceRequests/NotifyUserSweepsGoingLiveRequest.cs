using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class NotifyUserSweepsGoingLiveRequest : IRequest
	{
		private Guid _accountUid;
		private Sweep _sweepStake;
		private int _hourValue;

		public NotifyUserSweepsGoingLiveRequest(Guid accountUid, Sweep sweepStake, int hourValue)
		{
			_accountUid = accountUid;
			_sweepStake = sweepStake;
			_hourValue = hourValue;
		}

		public Guid AccountUid { get { return _accountUid; } }
		public Sweep Sweepstake { get { return _sweepStake; } }
		public int HourValue { get { return _hourValue; } }
	}
}
