using Promo.Core;
using System;

namespace Promo.Rules.Infrastructure.Models
{
    public class WatchTypeCampaignType
    {
        public string WatchTypeCode { get; set; }
        public string WatchTypeName { get; set; }
        public Guid CampaignTypeUid { get; set; }
    }
}
