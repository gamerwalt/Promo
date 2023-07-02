namespace hrvster.Template1.Web.Models
{
	public class PlayMatchGame
	{
		public Guid? GameUid { get; set; }
		public string? GameName { get; set; }
		public MatchBoard? MatchBoard { get; set; }
		public string? SelectedTiles { get; set; }
		public Guid? BoardUid { get; set; }
		public List<Guid>? Tiles { get; set; }
		public Guid? GameplayUid { get; set; }
		public string? ExternalClient { get; set; }
	}
}
