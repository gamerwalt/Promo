using hrvster.Models;

namespace hrvster.Games.Match.Models.Rules
{
	public class MatchAndRules
	{
		public MatchGame Game { get; set; }
		public IEnumerable<Rule> Rules { get; set; }
	}
}
