using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.Notifications
{
	public class SweepstakeUpdated : INotification
	{
		private Sweep _sweepstake;

		public SweepstakeUpdated(Sweep sweepstake)
		{
			_sweepstake = sweepstake;
		}

		public Sweep Sweepstake { get { return _sweepstake; } }
	}
}
