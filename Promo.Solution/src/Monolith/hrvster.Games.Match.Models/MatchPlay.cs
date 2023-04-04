namespace hrvster.Games.Match.Models
{
	public class MatchPlay
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public bool PlayedIndicator { get; set; }
		public MatchBoard MatchBoard { get; set; }
	}
}
