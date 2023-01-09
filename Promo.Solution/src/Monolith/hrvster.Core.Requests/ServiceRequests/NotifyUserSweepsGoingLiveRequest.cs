using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class NotifyUserSweepsGoingLiveRequest : IRequest
	{
		private Account _account;
		private Sweep _sweepStake;
		private SweepProperty _sweepProperty;
		private int _hourValue;

		public NotifyUserSweepsGoingLiveRequest(Account account, Sweep sweepStake, SweepProperty sweepProperty, int hourValue)
		{
			_account = account;
			_sweepStake = sweepStake;
			_sweepProperty = sweepProperty;
			_hourValue = hourValue;
		}

		public Account Account { get { return _account; } }
		public Sweep Sweepstake { get { return _sweepStake; } }
		public SweepProperty SweepProperty { get { return _sweepProperty; } }
		public int HourValue { get { return _hourValue; } }
	}
}
