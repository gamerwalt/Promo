using MediatR;
using Promo.Core.MessageContracts;

namespace Promo.Core.Notifications
{
	public class CampaignCreated : INotification
	{
		private ICampaignCreated message;

		public CampaignCreated(ICampaignCreated message)
		{
			this.message = message;
		}

		public ICampaignCreated Campaign
		{
			get { return message; }
		}
	}
}
