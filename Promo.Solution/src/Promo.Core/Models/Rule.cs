using System;

namespace Promo.Core.Models
{
    public class Rule
    {
        public int RuleId { get; set; }
        public Guid RuleUid { get; set; }
        public Guid CampaignUid { get; set; }
		public Guid SweepUid { get; set; }
        public string WatchTypeCode { get; set; }
        public string WatchValue { get; set; }
        public bool ActiveInd { get; set; }
		public long TweetId { get; set; }
	}
}
