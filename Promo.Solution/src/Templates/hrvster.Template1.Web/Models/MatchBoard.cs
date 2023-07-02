namespace hrvster.Template1.Web.Models
{
	public class MatchBoard
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public bool IsWinnable { get; set; }
		public MatchGame MatchGame { get; set; }
	}
}
