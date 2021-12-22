using System;

namespace Promo.Core.CoreRequestNotifications
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
