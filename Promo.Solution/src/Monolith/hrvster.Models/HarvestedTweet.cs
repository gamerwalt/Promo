using System;

namespace hrvster.Models
{
	public class HarvestedTweet
	{
		public DateTime Datepart { get; set; }
		public int Count { get; set; }
		public string SweepName { get; set; }
		public Guid SweepUid { get; set; }
	}
}
