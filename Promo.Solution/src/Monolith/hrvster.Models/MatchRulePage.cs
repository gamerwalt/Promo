using System;

namespace hrvster.Models
{
	public class MatchRulePage
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public int MatchRuleId { get; set; }
		public string PageId { get; set; }
	}
}
