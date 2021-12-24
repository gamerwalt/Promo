using Promo.Core;
using System;

namespace Promo.Campaign.Infrastructure.Models
{
    public class CampaignType
    {
        public int CampaignTypeId { get; set; }
        public Guid CampaignTypeUid { get; set; }
        public CampaignTypeCode CampaignTypeCode { get; set; }
        public string CampaignTypeName { get; set; }
        public string CamapaignTypeDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
