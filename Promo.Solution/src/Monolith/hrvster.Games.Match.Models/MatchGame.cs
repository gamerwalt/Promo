namespace hrvster.Games.Match.Models
{
	public class MatchGame
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public int CampaignId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int NumberOfBoards { get; set; }
		public int TileCountPerBoard { get; set; }
		public int NumberOfWinnableBoards { get; set; }
		public int WinProbability { get; set; }
		public bool HasBeenGenerated { get; set; }
		public bool ReadyToBeGenerated { get; set; }
		public Guid CampaignUid { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime StartDateUtc { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime EndDateUtc { get; set; }
	}
}
