using System;

namespace hrvster.Core.Requests.ApiRequests
{
	public class NewRuleRequest
	{
		public Guid RuleUid { get; set; }
		public Guid CampaignUid { get; set; }
		public Guid SweepUid { get; set; }
		public string WatchTypeCode { get; set; }
		public string WatchValue { get; set; }
		public bool ActiveInd { get; set; }
	}
}
