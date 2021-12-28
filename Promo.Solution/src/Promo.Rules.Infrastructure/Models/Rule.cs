using Promo.Core;
using System;

namespace Promo.Rules.Infrastructure.Models
{
    public class Rule
    {
        public int RuleId { get; set; }
        public Guid RuleUid { get; set; }
        public Guid CampaignUid { get; set; }
        public WatchTypeCode WatchTypeCode { get; set; }
        public string WatchValue { get; set; }
        public bool ActiveInd { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
