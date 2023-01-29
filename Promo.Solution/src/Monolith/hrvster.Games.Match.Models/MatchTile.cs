namespace hrvster.Games.Match.Models
{
	public class MatchTile
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public MatchImage Image { get; set; }
		public MatchPrize Prize { get; set; }
		public bool WinIndicator { get; set; }
	}
}
