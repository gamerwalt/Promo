using MediatR;
using Promo.Core;
using Promo.Rules.Core.Notifications;
using Promo.Rules.Infrastructure;
using Promo.Rules.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Rules.Core.RequestHandlers
{
    public class AddCampaignRules : IRequest<IEnumerable<Rule>>
    {
        private IEnumerable<Promo.Core.Models.Rule> rules;

        public AddCampaignRules(IEnumerable<Promo.Core.Models.Rule> rules)
        {
            this.rules = rules;
        }

        public IEnumerable<Promo.Core.Models.Rule> Rules
        {
            get { return rules; }
        }
    }

    public class AddCampaigRulesHandler : IRequestHandler<AddCampaignRules, IEnumerable<Rule>>
    {
        private readonly IMediator _mediator;
        private readonly IRulesRepository _repository;

        public AddCampaigRulesHandler(IMediator mediator, IRulesRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<IEnumerable<Rule>> Handle(AddCampaignRules request, CancellationToken cancellationToken)
        {
            var rules = new List<Rule>();

            foreach(var rule in request.Rules)
            {
                await _repository.CreateRuleAsync(new Rule
                {
                    CampaignUid = rule.CampaignUid,
                    RuleUid = rule.RuleUid,
                    ActiveInd = rule.ActiveInd,
                    WatchTypeCode = (WatchTypeCode)Enum.Parse(typeof(WatchTypeCode), rule.WatchTypeCode),
                    WatchValue = rule.WatchValue
                });

                rules.Add(await _repository.GetAsync(rule.RuleUid));
            }

            await _mediator.Publish(new RulesAdded(rules));

            return rules;
        }
    }
}
