namespace hrvster.Template1.Web.Models
{
	public class MatchGame
	{
		public Guid MatchGameUid { get; set; }
		public string Name { get; set; }
		public int NumberOfBoards { get; set; }
		public int NumberWinnable { get; set; }
		public bool ReadyToGenerate { get; set; }
		public bool IsGenerated { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
