using System;

namespace hrvster.Web.Models
{
	public class MatchRule
	{
		public Guid RuleUid { get; set; }
		public int RuleId { get; set; }
		public bool IsActive { get; set; }
		public bool MustBeFollowing { get; set; }
		public bool NotifyWinners { get; set; }
		public string WatchValue { get; set; }
		public string ExternalId { get; set; }
	}
}
