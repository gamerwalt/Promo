using System;

namespace hrvster.Models.Sweepstakes
{
	public class SweepRulesPage
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public int SweepRuleId { get; set; }
		public string PageId { get; set; }
	}
}
