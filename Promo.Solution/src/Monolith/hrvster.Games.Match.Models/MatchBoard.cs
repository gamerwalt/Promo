namespace hrvster.Games.Match.Models
{
	public class MatchBoard
	{
		public MatchBoard(int row, int column)
		{
			Tiles = new MatchTile[row][];
			for(var rowIndex = 0; rowIndex < row; rowIndex++)
			{
				Tiles[rowIndex] = new MatchTile[column];
				for(var colIndex = 0; colIndex < column; colIndex++)
				{
					Tiles[rowIndex][colIndex] = new MatchTile();
					Tiles[rowIndex][colIndex].Uid = Guid.NewGuid();
				}
			}
		}

		public int Id { get; set; }
		public Guid Uid { get; set; }
		public bool IsWinnable { get; set; }
		public MatchPrize Prize { get; set; }
		public MatchTile[][] Tiles { get; set; }
	}
}