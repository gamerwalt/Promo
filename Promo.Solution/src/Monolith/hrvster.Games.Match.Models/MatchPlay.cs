namespace hrvster.Games.Match.Models
{
	public class MatchPlay
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public bool PlayedIndicator { get; set; }
		public MatchBoard MatchBoard { get; set; }
		public string SelectedTiles { get; set; }
		public bool WinIndicator { get; set; }
	}
}
