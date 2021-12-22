using System;

namespace Promo.Core.ApiRequests
{
    public class CreateCampaignRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CampaignTypeUid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
