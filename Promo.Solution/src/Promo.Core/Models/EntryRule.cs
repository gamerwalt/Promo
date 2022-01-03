using System;

namespace Promo.Core.Models
{
	public class EntryRule
	{
		public Guid EntryRuleUid { get; set; }
		public Guid CampaignUid { get; set; }
		public Guid SweepUid { get; set; }
		public int EntryPerUser { get; set; }
	}
}
