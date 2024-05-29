using hrvster.Models.Profiles;

namespace hrvster.Games.Match.Models.Rules
{
	public class MatchRulesAndPrizes : MatchAndRules
	{
		public IEnumerable<MatchPrize> Prizes { get; set; }
		public Profile Profile { get; set; }
	}
}
