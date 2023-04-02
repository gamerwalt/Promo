using System;

namespace hrvster.Web.Models
{
	public class CreateMatchPlayRule
	{
		public string GameName { get; set; }
		public Guid GameUid { get; set; }
		public string ControllerName { get; set; }
		public string ActionName { get; set; }
		public int PlayPerUser { get; set; }
		public int PlayPerTweet { get; set; }
	}
}
