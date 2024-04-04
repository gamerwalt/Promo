using hrvster.Models;

namespace hrvster.Games.Match.Models
{
	public class MatchGame : Game
	{
		public int CampaignId { get; set; }
		public int NumberOfBoards { get; set; }
		public int TileCountPerBoard { get; set; }
		public int NumberOfWinnableBoards { get; set; }
		public int WinProbability { get; set; }
		public bool HasBeenGenerated { get; set; }
		public bool ReadyToBeGenerated { get; set; }
		public Guid CampaignUid { get; set; }
		public DateTime GenerateBoardDate { get; set; }
		public DateTime GenerateBoardDateUtc { get; set; }
		public bool AccumulatePointsOnLoss { get; set; }
		public int PointsPerLoss { get; set; }
	}
}
