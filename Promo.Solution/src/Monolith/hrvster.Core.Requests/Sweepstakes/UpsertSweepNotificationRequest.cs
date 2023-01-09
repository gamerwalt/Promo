using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MediatR;

namespace hrvster.Core.Requests.Sweepstakes
{
	public class UpsertSweepNotificationRequest : IRequest
	{
		private Sweep _sweep;
		private SweepNotificationType _notificationType;

		public UpsertSweepNotificationRequest(Sweep sweep, SweepNotificationType sweepNotificationType)
		{
			_sweep = sweep;
			_notificationType = sweepNotificationType;
		}

		public Sweep Sweepstake { get { return _sweep; } }
		public SweepNotificationType NotificationType { get { return _notificationType; } }
	}
}
