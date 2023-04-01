using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.MatchGames
{
	public interface IMatchGamesService
	{
		Task CreateMatchGameAsync(MatchGame matchGame, string timeZone, bool isDaylight, CancellationToken cancellationToken);
		Task<IEnumerable<MatchGame>> GetAccountMatchGamesAsync(Guid userUid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchGame>> GetCampaignMatchGamesAsync(Guid campaignUid, CancellationToken cancellationToken);
		MatchGame GetMatchGame(Guid matchGameUid);
		Task<MatchGame> GetMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchGame>> GetMatchGamesToGenerateBoardAsync(CancellationToken token);
		Task<IEnumerable<Rule>> GetRulesAsync(CancellationToken cancellationToken);
		IEnumerable<MatchGameRule> GetRulesMatching(string watchValue);
		Task SetAsReadyToBeProcessedAsync(MatchGame matchGame, CancellationToken cancellationToken);
		Task SetMatchGameAsProcessedAsync(MatchGame matchGame, CancellationToken cancellationToken);
		Task UpdateMatchGameAsync(MatchGame matchGame, string timeZone, bool isDaylight, CancellationToken cancellationToken);
	}

	public class MatchGamesService : IMatchGamesService
	{
		private readonly ILogger<MatchGamesService> _logger;
		private readonly IMatchGamesRepository _repository;
		private readonly IMatchRulesService _matchRulesService;

		public MatchGamesService(
			ILoggerFactory loggerFactory,
			IMatchGamesRepository repository,
			IMatchRulesService matchRulesService)
		{
			_logger = loggerFactory.CreateLogger<MatchGamesService>();
			_repository = repository;
			_matchRulesService = matchRulesService;
		}

		public async Task CreateMatchGameAsync(
			MatchGame matchGame, string timeZone, bool isDaylight, CancellationToken cancellationToken)
		{
			if (matchGame == null) throw new ArgumentNullException(nameof(matchGame));
			if (matchGame.CampaignId == 0 || matchGame.CampaignUid == Guid.Empty)
				throw new ArgumentException("Match game campaign needs to be supplied.");

			await _repository.CreateAsync(matchGame, timeZone, isDaylight, cancellationToken);
		}

		public async Task<IEnumerable<MatchGame>> GetAccountMatchGamesAsync(Guid userUid, CancellationToken cancellationToken)
		{
			if (userUid == Guid.Empty)
				throw new ArgumentNullException(nameof(userUid));

			return await _repository.GetByAccountAsync(userUid, cancellationToken);
		}

		public async Task<IEnumerable<MatchGame>> GetCampaignMatchGamesAsync(Guid campaignUid, CancellationToken cancellationToken)
		{
			if (campaignUid == Guid.Empty)
				throw new ArgumentNullException(nameof(campaignUid));

			return await _repository.GetByCampaignAsync(campaignUid, cancellationToken);
		}

		public MatchGame GetMatchGame(Guid matchGameUid)
		{
			if (matchGameUid == Guid.Empty)
				throw new ArgumentNullException(nameof(matchGameUid));

			return _repository.Get(matchGameUid, default(CancellationToken));
		}

		public async Task<MatchGame> GetMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken)
		{
			if (matchGameUid == Guid.Empty)
				throw new ArgumentNullException(nameof(matchGameUid));

			return await _repository.GetAsync(matchGameUid, cancellationToken);
		}

		public async Task<IEnumerable<MatchGame>> GetMatchGamesToGenerateBoardAsync(CancellationToken token)
			=> await _repository.GetGameReadyForGenerationAsync(token);

		public async Task<IEnumerable<Rule>> GetRulesAsync(CancellationToken cancellationToken)
			=> await _repository.GetRulesAsync(cancellationToken);

		public IEnumerable<MatchGameRule> GetRulesMatching(string watchValue)
			=> _matchRulesService.GetRulesMatching(watchValue);

		public async Task SetAsReadyToBeProcessedAsync(MatchGame matchGame, CancellationToken cancellationToken)
		{
			if (matchGame == null) throw new ArgumentNullException(nameof(matchGame));

			await _repository.SetAsReadyToBeGeneratedAsync(matchGame, cancellationToken);
		}

		public Task SetMatchGameAsProcessedAsync(MatchGame matchGame, CancellationToken cancellationToken)
			=> _repository.SetAsGeneratedAsync(matchGame, cancellationToken);

		public async Task UpdateMatchGameAsync(
			MatchGame matchGame, string timeZone, bool isDaylight, CancellationToken cancellationToken)
		{
			if (matchGame == null) throw new ArgumentNullException(nameof(matchGame));
			if (matchGame.CampaignId == 0 || matchGame.CampaignUid == Guid.Empty)
				throw new ArgumentException("Match game campaign needs to be supplied.");

			await _repository.UpdateAsync(matchGame, timeZone, isDaylight, cancellationToken);
		}
	}
}
