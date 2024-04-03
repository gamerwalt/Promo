using System;

namespace hrvster.Web.Models
{
	public class CreateLinkRequest
	{
		public string UrlLink { get; set; }
		public string GameType { get; set; }
		public Guid GameUid { get; set; }
		public Guid RuleUid { get; set; }
		public string ActionName { get; set; }
		public string ControllerName { get; set; }
		public string GameName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
