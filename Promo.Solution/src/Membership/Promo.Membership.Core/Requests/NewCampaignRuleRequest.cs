using System;
using System.Collections.Generic;

namespace Promo.Membership.Core.Requests
{
    public class NewCampaignRuleRequest
    {
        public Guid CampaignUid { get; set; }
        public List<string> TweetIds { get; set; }
        public List<string> Hashtags { get; set; }

    }
}
