using MediatR;
using Promo.Core.Models;
using System.Collections.Generic;

namespace Promo.Sweeps.Core.Notifications
{
	public class SaveWinnersFailed : INotification
	{
		private IEnumerable<SweepWinner> winners;
		private string campaignName;
		private string sweepName;

		public SaveWinnersFailed(IEnumerable<SweepWinner> winners, string campaignName, string sweepName)
		{
			this.winners = winners;
			this.campaignName = campaignName;
			this.sweepName = sweepName;
		}

		public IEnumerable<SweepWinner> Winners
		{
			get { return winners; }
		}

		public string CampaignName
		{
			get { return campaignName; }
		}

		public string SweepName
		{
			get { return sweepName; }
		}
	}
}
