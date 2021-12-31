using System;

namespace Promo.Sweeps.Core.Requests
{
    public class NewSweepRequest
    {
        public string Name { get; set; }
        public Guid CampaignUid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
		public DateTime DrawDate { get; set; }
    }
}
