using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Clients.Internal;
using Promo.Core.Models;
using Promo.Sweeps.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Core.RequestHandlers
{
	public class GetSweepDrawRules : IRequest<IEnumerable<SweepDrawRule>>
	{
		
	}

	public class GetSweepDrawRulesHandler : IRequestHandler<GetSweepDrawRules, IEnumerable<SweepDrawRule>>
	{
		private readonly ISweepsRepository _sweepsRepository;
		private readonly ICampaignsClient _campaignClient;
		private readonly IRulesClient _rulesClient;
		private readonly ILogger<GetSweepDrawRulesHandler> _logger;

		public GetSweepDrawRulesHandler(
			ISweepsRepository sweepsRepository,
			ICampaignsClient campaignsClient,
			IRulesClient rulesClient,
			ILogger<GetSweepDrawRulesHandler> logger)
		{
			_sweepsRepository = sweepsRepository;
			_campaignClient = campaignsClient;
			_rulesClient = rulesClient;
			_logger = logger;
		}

		public async Task<IEnumerable<SweepDrawRule>> Handle(GetSweepDrawRules request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retrieving sweeps requiring a draw...");
			var sweepsRequiringDraw = await _sweepsRepository.GetSweepsRequiringADrawAsync();

			_logger.LogInformation($"Retrieved {sweepsRequiringDraw.Count()} sweepstakes received requiring a draw.");

			if (sweepsRequiringDraw == null)
				return new List<SweepDrawRule>();

			var campaignGuidList = sweepsRequiringDraw.Select(c => c.CampaignUid);

			var campaignList = await _campaignClient.GetCampaignsAsync(campaignGuidList);
			_logger.LogInformation($"Retrieved {campaignList.Count()} campaigns retrieved to map to SweepDrawRule");

			var rules = await _rulesClient.GetSweepDrawRulesAsync(sweepsRequiringDraw.Select(c => c.SweepUid));
			_logger.LogInformation($"Retrieved {rules.Count()} draw rules retrieved to map to SweepDrawRule");

			var sweepRules = await _rulesClient.GetHashTagRulesAsync();

			//var sweepRules = await _rulesClient.getSwee

			var sweepDrawRules = new List<SweepDrawRule>();

			foreach(var sweep in sweepsRequiringDraw)
			{
				sweepDrawRules.Add(CreateSweepDrawRuleFrom(
					sweep,
					campaignList.FirstOrDefault(c => c.CampaignUid.Equals(sweep.CampaignUid)),
					rules.FirstOrDefault(c => c.SweepUid.Equals(sweep.SweepUid)),
					sweepRules.FirstOrDefault(c => c.SweepUid.Equals(sweep.SweepUid))
					)
				);
			}

			return sweepDrawRules;
		}

		private SweepDrawRule CreateSweepDrawRuleFrom(Infrastructure.Models.Sweep sweep, Campaign campaign, SweepDrawRule sweepDrawRule, Rule rule)
		{
			sweepDrawRule.CampaignName = campaign.CampaignName;
			sweepDrawRule.CampaignUid = campaign.CampaignUid;
			sweepDrawRule.SweepId = sweep.SweepId;
			sweepDrawRule.SweepName = sweep.Name;
			sweepDrawRule.StartDate = sweep.StartDate;
			sweepDrawRule.EndDate = sweep.EndDate;
			sweepDrawRule.DrawDate = sweep.DrawDate.GetValueOrDefault();
			sweepDrawRule.RulesUrl = sweep.RulesUrl;
			sweepDrawRule.OriginalTweetiId = rule.TweetId;

			return sweepDrawRule;
		}
	}
}
