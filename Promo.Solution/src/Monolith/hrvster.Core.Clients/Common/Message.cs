using hrvster.Models.Profiles;
using System;
using System.Collections.Generic;

namespace hrvster.Core.Clients.Common
{
	public class Message
	{
		public string SweepName { get; set; }
		public Guid SweepUid { get; set; }
		public Guid GameUid { get; set; }
		public string Text { get; set; }
		public bool IsScheduled { get; set; }
		public DateTime ScheduledDate { get; set; }
		public Profile Profile { get; set; }
		public string Hashtag { get; set; }
		public int WatchTypeId { get; set; }
		public IEnumerable<string> Media { get; set; }
		public string PageId { get; set; }
		public string PageAccessToken { get; set; }
	}

	public class PublishTweetMessage : Message
	{
		public Guid GameUid { get; set; }
	}
}
