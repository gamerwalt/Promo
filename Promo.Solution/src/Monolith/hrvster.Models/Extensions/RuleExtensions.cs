using System.Collections.Generic;
using System.Linq;

namespace hrvster.Models.Extensions
{
	public static class RuleExtensions
	{
		public static IEnumerable<Rule> GetActiveRules(this IEnumerable<Rule> rules)
		{
			return rules.Where(r => r.Game.IsActive() || r.Game.WillBeInProgress()).ToList();
		}
	}
}
