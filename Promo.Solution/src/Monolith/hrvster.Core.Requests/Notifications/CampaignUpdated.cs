using MediatR;
using System;

namespace hrvster.Core.Requests.Notifications
{
    public class CampaignUpdated : INotification
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
