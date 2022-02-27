using FluentValidation;
using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class AddCampaignRule : IRequest<IEnumerable<Rule>>
	{
		private Guid campaignUid;
		private Guid sweepUid;
		private CampaignRule rule;

		public AddCampaignRule(Guid campaignUid, Guid sweepUid, CampaignRule rule)
		{
			this.sweepUid = sweepUid;
			this.campaignUid = campaignUid;
			this.rule = rule;
		}

		public Guid CampaignUid
		{
			get { return campaignUid; }
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}

		public CampaignRule CampaignRule
		{
			get { return rule; }
		}
	}

	public class AddCampaignRuleHandler : IRequestHandler<AddCampaignRule, IEnumerable<Rule>>
	{
		private readonly IMediator _mediator;

		public AddCampaignRuleHandler(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<IEnumerable<Rule>> Handle(AddCampaignRule request, CancellationToken cancellationToken)
		{
			var rules = new List<Rule>();

			foreach (var property in request.CampaignRule.WatchProperties)
			{
				rules.Add(new Rule
				{
					CampaignUid = request.CampaignUid,
					SweepUid = request.SweepUid,
					RuleUid = Guid.NewGuid(),
					WatchValue = property,
					ActiveInd = true,
					WatchTypeCode = request.CampaignRule.WatchTypeCode
				});
			}

			await _mediator.Publish(new RulesAdded(rules));

			return rules;
		}
	}
}
