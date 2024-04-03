namespace hrvster.Template1.Web.Models
{
	public class PlaySweepsGame
	{
		public int NumberOfEntries { get; set; } = 0;
		public string? GameName { get; set; }
		public DateTime? GameStartDate { get; set; }
		public DateTime? GameEndDate { get; set; }
		public string? ScreenName { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public Guid? GameUid { get; set; }
		public Guid? AccountUid { get; set; }
		public string? ExternalClient { get; internal set; }
		public Guid? RuleUid { get; internal set; }
		public string? HandshakeKey { get; internal set; }
		public Guid CampaignUid { get; internal set; }
	}
}
