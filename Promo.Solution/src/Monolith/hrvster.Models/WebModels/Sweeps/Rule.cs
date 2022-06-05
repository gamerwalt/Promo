using System;

namespace hrvster.Models.WebModels.Sweeps
{
	public class Rule
	{
		public Guid RuleUid { get; set; }
		public WatchTypeCode WatchType { get; set; }
		public string WatchValue { get; set; }
		public bool ActiveInd { get; set; }
		public string ExternalId { get; set; }
	}
}
