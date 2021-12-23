using System;

namespace Promo.Campaign.Core.Notifications
{
    public class CampaignUpdated
    {
        private Guid _campaignUid;

        public CampaignUpdated(Guid campaignUid)
        {
            _campaignUid = campaignUid;
        }

        public Guid CampaignUid
        {
            get { return _campaignUid; }
        }
    }
}
