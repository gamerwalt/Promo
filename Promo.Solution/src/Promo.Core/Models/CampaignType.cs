using System;

namespace Promo.Core.Models
{
    public class CampaignType
    {
        public Guid CampaignTypeUid { get; set; }
        public CampaignTypeCode CampaignTypeCode { get; set; }
        public string CampaignTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
