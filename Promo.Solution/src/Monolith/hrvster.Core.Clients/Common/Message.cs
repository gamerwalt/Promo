using hrvster.Models.Profiles;
using System;

namespace hrvster.Core.Clients.Common
{
	public class Message
	{
		public string Text { get; set; }
		public bool IsScheduled { get; set; }
		public DateTime ScheduledDate { get; set; }
		public Profile Profile { get; set; }
	}
}
