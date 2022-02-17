//using FluentValidation;
using MediatR;
using Promo.Core;
using Promo.Core.Models;
using Promo.Membership.Core.Notifications;
using Promo.Membership.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core.RequestHandlers
{
	//public class AddCampaignRuleValidator : AbstractValidator<AddCampaignRule>
	//{
	//	private readonly ICampaignRepository _campaignRepository;
	//	private readonly IRulesClient _rulesClient;

	//	public AddCampaignRuleValidator(ICampaignRepository campaignRepository, IRulesClient rulesClient)
	//	{
	//		_campaignRepository = campaignRepository;
	//		_rulesClient = rulesClient;

	//		RuleFor(x => x.CampaignUid).NotEmpty().NotNull().WithMessage("Please supply a valid campaign uid");
	//		RuleFor(x => x.CampaignRule).Must(x => RuleShouldHaveAValidWatchTypeCode(x)).WithMessage("Invalid watch type code.");
	//		RuleFor(x => x.SweepUid).NotEmpty().NotNull().WithMessage("Please supply a valid sweep uid");

	//		RuleFor(x => x).MustAsync(async (x, cancellation) =>
	//		{
	//			var watchTypeCode = (WatchTypeCode)Enum.Parse(typeof(WatchTypeCode), x.CampaignRule.WatchTypeCode);
	//			var watchTypeCampaignTypes = await _rulesClient.GetWatchTypeCampaignTypesAsync();
	//			var campaign = await _campaignRepository.GetCampaignByUidAsync(x.CampaignUid);

	//			var watchTypeCampaigns = watchTypeCampaignTypes.Where(c => c.CampaignTypeUid == campaign.CampaignType.CampaignTypeUid);

	//			return watchTypeCampaigns.Any(c => c.WatchTypeCode.Equals(watchTypeCode.ToString()));
	//		}).WithMessage("Your watch type does not match the campaign type.");
	//	}

	//	private bool RuleShouldHaveAValidWatchTypeCode(CampaignRule campaignRule)
	//	{
	//		return Enum.TryParse(typeof(WatchTypeCode), campaignRule.WatchTypeCode, out var watchTypeCode);
	//	}
	//}

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
