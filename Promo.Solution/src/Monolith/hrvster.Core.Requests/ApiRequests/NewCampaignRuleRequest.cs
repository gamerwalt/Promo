using System;
using System.Collections.Generic;

namespace hrvster.Core.Requests.ApiRequests
{
    public class NewCampaignRuleRequest
    {
        public Guid CampaignUid { get; set; }
        public List<string> TweetIds { get; set; }
        public List<string> Hashtags { get; set; }

    }
}
