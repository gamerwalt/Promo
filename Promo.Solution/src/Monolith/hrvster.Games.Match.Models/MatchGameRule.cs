using hrvster.Models;

namespace hrvster.Games.Match.Models
{
	public class MatchGameRule : Rule
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public MatchGame MatchGame { get; set; }
		public WatchType WatchType { get; set; }
		public string WatchValue { get; set; }
		public bool MustBeFollowing { get; set; }
		public bool NotifyWinners { get; set; }
		public bool ScheduledIndicator { get; set; }
		public bool ActiveIndicator { get; set; }
	}
}
