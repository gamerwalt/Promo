using System;

namespace Promo.Sweeps.Infrastructure.Models
{
    public class Sweep
    {
        public int SweepId { get; set; }
        public Guid SweepUid { get; set; }
        public Guid CampaignUid { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? DrawDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
