namespace hrvster.Template1.Web.Models
{
	public class GameplayLoss
	{
		public Guid GameUid { get; set; }
		public string GameName { get; set; }
		public MatchBoard MatchBoard { get; set; }
		public Guid BoardUid { get; set; }
		public List<MatchTile> Tiles { get; set; }
		public List<Guid> SelectedTiles { get; set; }
		public Guid GameplayUid { get; set; }
		public int Points { get; set; }
	}
}
