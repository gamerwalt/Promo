using System.Collections.Generic;

namespace Promo.Membership.Infrastructure.Models
{
    public class CampaignRule
    {
        public string WatchTypeCode { get; set; }
        public List<string> WatchProperties { get; set; }
    }
}
