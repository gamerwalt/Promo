using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Requests.WebRequests;
using hrvster.Games.Match.Models.Rules;
using hrvster.Infrastructure.Accounts;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.MatchGame
{
	public class GetMatchGameForRulesView : IRequestHandler<GetMatchGameAndPrizesRequest, MatchRulesAndPrizes>
	{
		private readonly IMatchGamesService _gameService;
		private readonly IMatchGameBoardService _boardService;
		private readonly IMatchRulesService _ruleService;
		private readonly IMatchPrizeService _prizeService;
		private readonly IProfilesRepository _profileRepository;

		public GetMatchGameForRulesView(
			IMatchGamesService gameService,
			IMatchGameBoardService boardService,
			IMatchRulesService ruleService,
			IMatchPrizeService prizeService,
			IProfilesRepository profileRepository)
		{
			_gameService = gameService;
			_boardService = boardService;
			_ruleService = ruleService;
			_prizeService = prizeService;
			_profileRepository = profileRepository;
		}

		public async Task<MatchRulesAndPrizes> Handle(GetMatchGameAndPrizesRequest request, CancellationToken cancellationToken)
		{
			var gameTask = _gameService.GetMatchGameAsync(request.GameUid, cancellationToken);
			var ruleTask = _ruleService.GetAsync(request.GameUid, cancellationToken);
			var prizeTask = _prizeService.GetAllForMatchGameAsync(request.GameUid, cancellationToken);

			await Task.WhenAll(gameTask, ruleTask, prizeTask);

			var profile = default(Models.Profiles.Profile);

			var rules = ruleTask.Result;
			var game = gameTask.Result;
			var prizes = prizeTask.Result;
			var newRules = rules.Where(c => c.WatchValue != "Not Applicable");

			if (newRules?.Count() > 0)
			{
				
				if(newRules == null)
				{
					profile = null;
				}
				else
				{
					var ruleUid = newRules.FirstOrDefault().RuleUid;
					profile = await _profileRepository.GetProfileBelongingToMatchRuleAsync(ruleUid, Models.ProfileType.Twitter);
				}
			}

			return new MatchRulesAndPrizes
			{
				Game = game,
				Prizes = prizes.ToList().Where(p => !p.IsFiller),
				Profile = profile,
				Rules = newRules
			};
		}
	}
}
