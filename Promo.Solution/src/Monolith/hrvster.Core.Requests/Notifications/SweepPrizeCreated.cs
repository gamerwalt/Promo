using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.Notifications
{
	public class SweepPrizeCreated : INotification
	{
		private SweepPrize sweepPrize;

		public SweepPrizeCreated(SweepPrize sweepPrize)
		{
			this.sweepPrize = sweepPrize;
		}

		public SweepPrize SweepPrize
		{
			get { return sweepPrize; }
		}
	}
}
