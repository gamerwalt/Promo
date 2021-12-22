using System;

namespace Promo.Models.Campaigns
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
