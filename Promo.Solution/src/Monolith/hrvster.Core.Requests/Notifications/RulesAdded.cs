using hrvster.Models;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Core.Requests.Notifications
{
	public class RulesAdded : INotification
	{
		private readonly List<Rule> rules;

		public RulesAdded(List<Rule> rules)
		{
			this.rules = rules;
		}

		public List<Rule> GetRules()
		{
			return rules;
		}
	}
}
