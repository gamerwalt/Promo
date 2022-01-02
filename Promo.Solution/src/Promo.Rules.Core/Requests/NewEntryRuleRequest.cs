using System;

namespace Promo.Rules.Core.Requests
{
	public class NewEntryRuleRequest
	{
		public Guid CampaignUid { get; set; }
		public Guid SweepUid { get; set; }
		public int EntryPerUser { get; set; }
	}
}
