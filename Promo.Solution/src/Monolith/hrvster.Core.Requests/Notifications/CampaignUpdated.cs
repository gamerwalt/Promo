using MediatR;
using System;

namespace hrvster.Core.Requests.Notifications
{
    public class CampaignUpdated : INotification
    {
        private Guid _campaignUid;
		private Guid _accountUid;

        public CampaignUpdated(Guid campaignUid, Guid accountUid)
        {
			_accountUid= accountUid;
            _campaignUid = campaignUid;
        }

        public Guid CampaignUid
        {
            get { return _campaignUid; }
        }

		public Guid AccountUid { get { return _accountUid; } }
    }
}
