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
		private Guid accountUid;

        public CampaignCreated(NewCampaignRequest request, Guid accountUid, Guid campaignUid)
        {
            this.request = request;
            this.campaignUid = campaignUid;
			this.accountUid = accountUid;
        }

        public NewCampaignRequest Request
        {
            get { return this.request; }
        }

        public Guid CampaignUid
        {
            get { return this.campaignUid; }
        }

		public Guid AccountUid
		{
			get { return this.accountUid; }
		}
    }
}
