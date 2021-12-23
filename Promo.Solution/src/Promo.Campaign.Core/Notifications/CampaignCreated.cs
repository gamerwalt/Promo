using MediatR;
using Promo.Core.CoreRequestHandlers;
using System;

namespace Promo.Campaign.Core.Notifications
{
    public class CampaignCreated : INotification
    {
        private CreateCampaignRequest request;
        private Guid campaignUid;

        public CampaignCreated(CreateCampaignRequest request, System.Guid campaignUid)
        {
            this.request = request;
            this.campaignUid = campaignUid;
        }

        public CreateCampaignRequest Request
        {
            get { return this.request; }
        }

        public Guid CampaignUid
        {
            get { return this.campaignUid; }
        }
    }
}
