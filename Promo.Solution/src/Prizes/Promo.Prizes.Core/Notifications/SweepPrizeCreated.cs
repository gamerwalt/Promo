using MediatR;
using Promo.Prizes.Infrastructure.Models;

namespace Promo.Prizes.Core.Notifications
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
