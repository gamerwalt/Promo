using System.Collections.Generic;

namespace hrvster.Games.Match.Models
{
	public class MatchBoard
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public bool IsWinnable { get; set; }
		public MatchPrize Prize { get; set; }
		public MatchGame MatchGame { get; set; }
		public List<List<MatchTile>> Tiles { get; set; }
		public double WinProbability { get; set; }
	}
}