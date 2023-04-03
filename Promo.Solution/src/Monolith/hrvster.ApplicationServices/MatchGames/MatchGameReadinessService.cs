using hrvster.Infrastructure.MatchGames;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.MatchGames
{
	public interface IMatchGameReadinessService
	{
		Task<bool> IsReadyToGenerateBoardsAsync(Guid matchGameUid, CancellationToken cancellationToken);
	}

	public class MatchGameReadinessService : IMatchGameReadinessService
	{
		private readonly ILogger<MatchGameReadinessService> _logger;
		private readonly IMatchGamesRepository _repository;
		private readonly IMatchPrizesRepository _matchPrizesRepository;
		private readonly IMatchImagesRepository _matchImagesRepository;
		private readonly IMatchBoardsRepository _matchBoardsRepository;

		public MatchGameReadinessService(
			ILoggerFactory loggerFactory, 
			IMatchGamesRepository repository,
			IMatchPrizesRepository matchPrizesRepository,
			IMatchImagesRepository matchImagesRepository,
			IMatchBoardsRepository matchBoardsRepository)
		{ 
			_logger = loggerFactory.CreateLogger<MatchGameReadinessService>();
			_repository = repository;
			_matchPrizesRepository = matchPrizesRepository;
			_matchImagesRepository = matchImagesRepository;
			_matchBoardsRepository = matchBoardsRepository;
		}

		public async Task<bool> IsReadyToGenerateBoardsAsync(Guid matchGameUid, CancellationToken cancellationToken)
		{
			var matchGame = await _repository.GetAsync(matchGameUid, cancellationToken);
			if (matchGame == null) return false;

			var prizes = await _matchPrizesRepository.GetForMatchGameAsync(matchGameUid, cancellationToken);
			if(prizes.Count() == 0) return false;

			var images = await _matchImagesRepository.GetForMatchGamesAsync(matchGameUid, cancellationToken);
			if (images.Count() == 0) return false;

			var boards = await _matchBoardsRepository.GetForMatchGameAsync(matchGameUid, cancellationToken);
			if (boards.Count() > 0) return false;

			return true;
		}
	}
}
