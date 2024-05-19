namespace hrvster.Games.Match.Models
{
	public class MatchTile
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public MatchBoard Board { get; set; }
		public MatchImage Image { get; set; }
		public MatchPrize Prize { get; set; }
		public bool WinIndicator { get; set; }
		public double Random { get; set; }

		public MatchTile CloneWithNewProperties(Guid uid, double random, MatchBoard board, bool winIndicator)
		{
			return new MatchTile
			{
				Uid = uid,
				Board = board,
				WinIndicator = winIndicator,
				Prize = this.Prize,
				Image = this.Image,
				Random = random
			};
		}
	}
}
