using MediatR;
using Promo.Campaign.Core.Requests;
using Promo.Membership.Core.Requests;
using System;

namespace Promo.Membership.Core.Notifications
{
    public class CampaignCreated : INotification
    {
        private NewCampaignRequest request;
        private Guid campaignUid;

        public CampaignCreated(NewCampaignRequest request, System.Guid campaignUid)
        {
            this.request = request;
            this.campaignUid = campaignUid;
        }

        public NewCampaignRequest Request
        {
            get { return this.request; }
        }

        public Guid CampaignUid
        {
            get { return this.campaignUid; }
        }
    }
}
