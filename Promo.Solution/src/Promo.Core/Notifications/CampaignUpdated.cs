using MediatR;
using Promo.Core.MessageContracts;

namespace Promo.Core.Notifications
{
	public class CampaignUpdated : INotification
	{
		private ICampaignUpdated campaignUpdated;

		public CampaignUpdated(ICampaignUpdated message)
		{
			this.campaignUpdated = message;
		}

		public ICampaignUpdated Campaign
		{
			get { return campaignUpdated; }
		}
	}
}
