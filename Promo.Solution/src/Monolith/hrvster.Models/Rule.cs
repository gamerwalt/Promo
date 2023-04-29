using System;

namespace hrvster.Models
{
	public class Rule
	{
		public int RuleId { get; set; }
		public Guid RuleUid { get; set; }
		public Campaign Campaign { get; set; }
		public Sweep Sweep { get; set; }
		public Game Game { get; set; }
		public WatchTypeCode WatchTypeCode { get; set; }
		public string WatchValue { get; set; }
		public string ExternalId { get; set; }
		public bool ActiveInd { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public bool MustBeFollowing { get; set; }
		public bool ScheduledInd { get; set; }
		public bool NotifyWinners { get; set; }
		public ExternalClientType ExternalClientType { get; set; }
	}
}
