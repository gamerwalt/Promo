using hrvster.Models;

namespace hrvster.Games.Match.Models
{
	public class MatchGamePlayReport
	{
		public int Id { get; set; }
		public Guid GameUid { get; set; }
		public Guid PlayUid { get; set; }
		public Guid CampaignUid { get; set; }
		public string ScreenName { get; set; }
		public UserIdentifierType UserIdentifierType { get; set; }
		public string ExternalUserId { get; set; }
		public string ExternalId { get; set; }
		public int Plays { get; set; }
	}
}
