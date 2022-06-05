using hrvster.Models.Profiles;
using System;

namespace hrvster.Core.Clients.Common
{
	public class Message
	{
		public string SweepName { get; set; }
		public Guid SweepUid { get; set; }
		public string Text { get; set; }
		public bool IsScheduled { get; set; }
		public DateTime ScheduledDate { get; set; }
		public Profile Profile { get; set; }
		public string Hashtag { get; set; }
		public int WatchTypeId { get; set; }
	}
}
