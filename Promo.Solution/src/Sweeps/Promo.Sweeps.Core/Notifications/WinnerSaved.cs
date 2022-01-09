using MediatR;
using Promo.Core.Models;
using System.Collections.Generic;

namespace Promo.Sweeps.Core.Notifications
{
	public class WinnerSaved : INotification
	{
		private IEnumerable<SweepWinner> winners;
		private SweepDrawRule sweepDrawRule;

		public WinnerSaved(IEnumerable<SweepWinner> winners, SweepDrawRule sweepDrawRule)
		{
			this.winners = winners;
			this.sweepDrawRule = sweepDrawRule;
		}

		public IEnumerable<SweepWinner> Winners
		{
			get { return winners; }
		}

		public SweepDrawRule SweepDrawRule
		{
			get { return sweepDrawRule; }
		}
	}
}
