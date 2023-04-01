using hrvster.ApplicationServices;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class GetActiveHashtagRules : IRequestHandler<GetActiveHashtagRulesRequest, IEnumerable<Rule>>
	{
		private readonly ILogger<GetActiveHashtagRules> _logger;
		private readonly ISweepRulesRepository _sweepRulesRepository;
		private readonly IGameService _gameService;

		public GetActiveHashtagRules(
			ILoggerFactory loggerFactory,
			ISweepRulesRepository sweepRulesRepository,
			IGameService gameService)
		{
			_logger = loggerFactory.CreateLogger<GetActiveHashtagRules>();
			_sweepRulesRepository = sweepRulesRepository;
			_gameService = gameService;
		}

		public async Task<IEnumerable<Rule>> Handle(GetActiveHashtagRulesRequest request, CancellationToken cancellationToken)
		{
			var rules = new List<Rule>();

			var sweepRules = await _sweepRulesRepository.GetSweepRuleAsync();
			_logger.LogInformation("Retrieved {count} Sweep Rules.", sweepRules.Count());

			var matchGameRules = await _gameService.GetRulesAsync(GameTypesEnum.MatchGames, cancellationToken);
			_logger.LogInformation("Retrieved {count} Match game rules.", matchGameRules.Count());

			rules.AddRange(sweepRules);
			rules.AddRange(matchGameRules);

			return rules;
		}
	}
}
