using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
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
		Task<IEnumerable<MatchGame>> GetCampaignMatchGamesAsync(Guid campaignUid, CancellationToken cancellationToken);
		Task<MatchGame> GetMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken);
		Task SetAsReadyToBeProcessedAsync(MatchGame matchGame, CancellationToken cancellationToken);
		Task SetMatchGameAsProcessedAsync(MatchGame matchGame, CancellationToken cancellationToken);
		Task UpdateMatchGameAsync(MatchGame matchGame, string timeZone, bool isDaylight, CancellationToken cancellationToken);
	}

	public class MatchGamesService : IMatchGamesService
	{
		private readonly ILogger<MatchGamesService> _logger;
		private readonly IMatchGamesRepository _repository;

		public MatchGamesService(ILoggerFactory loggerFactory, IMatchGamesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<MatchGamesService>();
			_repository = repository;
		}

		public async Task CreateMatchGameAsync(
			MatchGame matchGame, string timeZone, bool isDaylight, CancellationToken cancellationToken)
		{
			if (matchGame == null) throw new ArgumentNullException(nameof(matchGame));
			if (matchGame.CampaignId == 0 || matchGame.CampaignUid == Guid.Empty)
				throw new ArgumentException("Match game campaign needs to be supplied.");

			await _repository.CreateAsync(matchGame, timeZone, isDaylight, cancellationToken);
		}

		public async Task<IEnumerable<MatchGame>> GetCampaignMatchGamesAsync(Guid campaignUid, CancellationToken cancellationToken)
		{
			if (campaignUid == Guid.Empty)
				throw new ArgumentNullException(nameof(campaignUid));

			return await _repository.GetByCampaignAsync(campaignUid, cancellationToken);
		}

		public async Task<MatchGame> GetMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken)
		{
			if (matchGameUid == Guid.Empty)
				throw new ArgumentNullException(nameof(matchGameUid));

			return await _repository.GetByUidAsync(matchGameUid, cancellationToken);
		}

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
