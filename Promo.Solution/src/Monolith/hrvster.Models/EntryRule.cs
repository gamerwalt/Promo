using System;

namespace hrvster.Models
{
	public class EntryRule
	{
		public int Id { get; set; }
		public Guid EntryRuleUid { get; set; }
		public Guid CampaignUid { get; set; }
		public Guid SweepUid { get; set; }
		public int EntryPerUser { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}
