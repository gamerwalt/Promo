using MediatR;
using Promo.Core.Models;
using System.Collections.Generic;

namespace Promo.Membership.Core.Notifications
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
