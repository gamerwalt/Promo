using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.Sweepstakes
{
	public class GetSweepNotificationRequest : IRequest<SweepNotification>
	{
		private Sweep _sweep;

		public GetSweepNotificationRequest(Sweep sweep)
		{
			_sweep = sweep;
		}

		public Sweep Sweepstake { get { return _sweep; } }
	}
}
