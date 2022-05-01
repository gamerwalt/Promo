using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class AddCampaignRules : IRequest<IEnumerable<Rule>>
	{
		private IEnumerable<Rule> rules;

		public AddCampaignRules(IEnumerable<Rule> rules)
		{
			this.rules = rules;
		}

		public IEnumerable<Rule> Rules
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

			foreach (var rule in request.Rules)
			{
				var ruleUid = Guid.NewGuid();
				await _repository.CreateRuleAsync(new Rule
				{
					RuleUid = ruleUid,
					Campaign = rule.Campaign,
					Sweep = rule.Sweep,
					ActiveInd = rule.ActiveInd,
					WatchTypeCode = rule.WatchTypeCode,
					WatchValue = rule.WatchValue
				});

				rules.Add(await _repository.GetAsync(ruleUid));
			}

			await _mediator.Publish(new RulesAdded(rules));

			return rules;
		}
	}
}
