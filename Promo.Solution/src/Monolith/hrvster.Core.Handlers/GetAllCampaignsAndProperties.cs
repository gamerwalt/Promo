using AutoMapper;
using hrvster.Core.Requests.WebRequests;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Harvester;
using hrvster.Models.Harvester;
using hrvster.Models.WebModels;
using hrvster.Models.WebModels.Sweeps;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers
{
	public class GetAllCampaignsAndProperties : IRequestHandler<GetAllCampaignsAndPropertiesRequest, IEnumerable<Promotion>>
	{
		private readonly ILogger<GetAllCampaignsAndProperties> _logger;
		private readonly ICampaignRepository _campaignRepository;
		private readonly ISweepsRepository _sweepsRepository;
		private readonly ISweepsEntriesRepository _sweepEntriesRepository;
		private readonly ISweepRulesRepository _sweepRulesRepository;
		private readonly IHarvesterRepository _harvesterRepository;
		private readonly IMapper _mapper;

		public GetAllCampaignsAndProperties(
			ILoggerFactory loggerFactory,
			ICampaignRepository campaignRepository,
			ISweepsRepository sweepsRepository,
			ISweepsEntriesRepository sweepEntriesRepository,
			ISweepRulesRepository sweepsRulesRepository,
			IHarvesterRepository harvesterRepository,
			IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<GetAllCampaignsAndProperties>();
			_campaignRepository = campaignRepository;
			_sweepsRepository = sweepsRepository;
			_sweepEntriesRepository = sweepEntriesRepository;
			_sweepRulesRepository = sweepsRulesRepository;
			_harvesterRepository = harvesterRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<Promotion>> Handle(
			GetAllCampaignsAndPropertiesRequest request,
			CancellationToken cancellationToken)
		{
			var campaigns = await _campaignRepository.GetAccountCampaignsAsync(request.AccountUid);

			var promotions = _mapper.Map<IEnumerable<Promotion>>(campaigns);

			foreach(var promotion in promotions)
			{
				var totalEntries = 0;
				var totalHarvested = 0;
				var sweepsForPromotion = await _sweepsRepository.GetAllForCampaignAsync(promotion.CampaignUid);

				promotion.Sweepstakes = _mapper.Map<IEnumerable<Sweepstake>>(sweepsForPromotion);

				foreach(var sweep in promotion.Sweepstakes)
				{
					sweep.SweepEntries = await _sweepEntriesRepository.GetSweepEntriesAsync(sweep.SweepUid);
					if(sweep.SweepEntries != null) totalEntries += sweep.SweepEntries.Count();

					var rules = await _sweepRulesRepository.GetSweepRuleAsync(sweep.SweepUid);
					var harvested = new List<Harvested>();
					foreach(var rule in rules)
					{
						var harvests = await _harvesterRepository.GetTweetsAsync(rule.RuleUid);
						harvested.AddRange(harvests);
						if (harvests != null) totalHarvested += harvests.Count();
					}
				}

				promotion.SweepEntries = totalEntries;
				promotion.Harvested = totalHarvested;
			}

			return promotions;
		}
	}
}
