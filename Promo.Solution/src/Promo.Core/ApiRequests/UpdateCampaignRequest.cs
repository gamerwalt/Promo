using System;

namespace Promo.Core.ApiRequests
{
    public class UpdateCampaignRequest
    {
        public string CampaignName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ActiveInd { get; set; }
    }
}
