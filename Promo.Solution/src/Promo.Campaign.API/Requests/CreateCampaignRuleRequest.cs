using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Campaign.API.Requests
{
    public class CreateCampaignRuleRequest
    {
        public Guid CampaignUid { get; set; }
        public List<string> TweetIds { get; set; }
        public List<string> Hashtags { get; set; }

    }
}
