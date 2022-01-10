using MediatR;
using Promo.Core.Models;

namespace Promo.Sweeps.Core.Notifications
{
	public class SweepDrawHappened : INotification
	{
		private SweepDrawRule sweepDrawRule;

		public SweepDrawHappened(SweepDrawRule sweepDrawRule)
		{
			this.sweepDrawRule = sweepDrawRule;
		}

		public SweepDrawRule SweepDrawRule
		{
			get { return sweepDrawRule; }
		}
	}
}
