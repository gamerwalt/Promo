using System;

namespace Promo.Rules.Infrastructure.Models
{
	public class DrawRule
	{
		public int SweepDrawruleId { get; set; }
		public Guid SweepDrawRuleUid { get; set; }
		public Guid SweepUid { get; set; }
		public int WinnerCount { get; set; }
		public int WinnerBackupCount { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}
