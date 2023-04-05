using hrvster.Models;

namespace hrvster.Games.Match.Models
{
	public class MatchImage
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public string Name { get; set; }
		public MatchGame MatchGame { get; set; }
		public string LocationUrl { get; set; }
		public bool IsLoss { get; set; }
	}
}
