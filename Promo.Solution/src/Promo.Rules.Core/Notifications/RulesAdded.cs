using MediatR;
using Promo.Rules.Infrastructure.Models;
using System.Collections.Generic;

namespace Promo.Rules.Core.Notifications
{
    public class RulesAdded : INotification
    {
        private List<Rule> rules;

        public RulesAdded(List<Rule> rules)
        {
            this.rules = rules;
        }

        public List<Rule> Rules
        {
            get { return rules; }
        }
    }
}
