using System;

namespace Promo.Campaign.Infrastructure.Models
{
    public class Campaign
    {
        public int CampaignId { get; set; }
        public Guid CampaignUid { get; set; }
        public Guid AccountUid { get; set; }
        public CampaignType CampaignType { get; set; }
        public string CampaignName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool ActiveInd { get; set; }
    }
}
