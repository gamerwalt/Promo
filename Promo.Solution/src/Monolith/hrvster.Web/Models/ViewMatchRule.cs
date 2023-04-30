namespace hrvster.Web.Models
{
	public class ViewMatchRule
	{
		public string ExternalId { get; set; }
		public string ExternalClient { get; set; }
		public string GameName { get; set; }
		public string Hashtag { get; set; }
		public string ReturnAction { get; set; }
		public string ReturnController { get; set; }
		public DeleteMatchRule DeleteMatchRule { get; set; }
	}
}
