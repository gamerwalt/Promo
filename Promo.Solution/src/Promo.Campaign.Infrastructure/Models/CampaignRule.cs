using System.Collections.Generic;

namespace Promo.Campaign.Infrastructure.Models
{
    public class CampaignRule
    {
        public string WatchTypeCode { get; set; }
        public List<string> WatchProperties { get; set; }
    }
}
