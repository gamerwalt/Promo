using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class CreateWatch
	{
		public List<SelectListItem> WatchTypes { get; set; }
		public int WatchTypeId { get; set; }
		public Guid SweepUid { get; set; }
		public string TweetText { get; set; }
		public string Hashtag { get; set; }
		public bool PostNow { get; set; }
		public DateTime ScheduledDate { get; set; }
		public bool MustBeFollowing { get; set; }
	}
}
