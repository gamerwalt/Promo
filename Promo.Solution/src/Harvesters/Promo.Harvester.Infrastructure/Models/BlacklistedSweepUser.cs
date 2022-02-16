using System;

namespace Promo.Harvester.Infrastructure.Models
{
	public class BlacklistedSweepUser
	{
		public Guid SweepUid { get; set; }
		public long TweetUserId { get; set; }
	}
}
