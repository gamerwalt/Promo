using hrvster.Models;

namespace hrvster.Games.Match.Models
{
	public class MatchGameRule : Rule
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public MatchGame MatchGame { get; set; }
	}
}
