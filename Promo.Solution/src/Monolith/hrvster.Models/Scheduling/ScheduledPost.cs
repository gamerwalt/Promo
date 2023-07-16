using hrvster.Models.Profiles;
using System;
using System.Collections.Generic;

namespace hrvster.Models.Scheduling
{
	public class ScheduledPost
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public Profile Profile { get; set; }
		public Rule Rule { get; set; }
		public GameTypesEnum GameType { get; set; }
		public string Post { get; set; }
		public DateTime ScheduledTime { get; set; }
		public DateTime ScheduledTimeUtc { get; set; }
		public string UrlLink { get; set; }
		public ScheduledStatus Status { get; set; }
		public List<ScheduledPostImage> Images { get; set; }
	}
}
