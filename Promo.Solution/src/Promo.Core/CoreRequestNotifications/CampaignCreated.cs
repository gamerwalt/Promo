using MediatR;
using Promo.Core.CoreRequestHandlers;

namespace Promo.Core.CoreRequestNotifications
{
    public class CampaignCreated : INotification
    {
        private CreateCampaignRequest request;

        public CampaignCreated(CreateCampaignRequest request)
        {
            this.request = request;
        }

        public CreateCampaignRequest Request
        {
            get { return request; }
        }
    }
}
