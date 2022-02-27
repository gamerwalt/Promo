using System;

namespace hrvster.Core.Requests.ApiRequests
{
	public class NewEntryRuleRequest
	{
		public Guid CampaignUid { get; set; }
		public Guid SweepUid { get; set; }
		public int EntryPerUser { get; set; }
	}
}
