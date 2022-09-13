using System;

namespace hrvster.Models
{
	public class UserLimiter
	{
		public int UserLimiterId { get; set; }
		public Guid UserLimiterUid { get; set; }
		public string ExternalUserId { get; set; }
		public UserIdentifierType UserIdentifierType { get; set; }
		public string WatchValue { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}
