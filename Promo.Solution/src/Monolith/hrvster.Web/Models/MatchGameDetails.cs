using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class MatchGameDetails
	{
		public UpdateMatchGame MatchGame { get; set; }
		public List<MatchImage> MatchImages { get; set; }
		public List<MatchRule> MatchRules { get; set; }
		public List<MatchPrize> MatchPrizes { get; set; }
		public MatchPlayRule MatchPlayRule { get; set; }
		public int PlayCount { get; set; }
		public bool Generated { get; set; }
		public bool ReadyToBeGenerated { get; set; }
	}
}