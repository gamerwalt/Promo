using hrvster.ApplicationServices.MatchGames;
using hrvster.Games.Match.Models;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace hrvster.Services
{
	public class MatchGameBoardService : IService
	{
		private readonly ILogger<MatchGameBoardService> _logger;
		private readonly IMatchGameReadinessService _matchGameReadinessService;
		private readonly IMatchGamesService _matchGamesService;
		private readonly IMatchGameBoardService _matchGameBoardService;

		public MatchGameBoardService(
			ILoggerFactory loggerFactory, 
			IMatchGameReadinessService matchGameReadinessService,
			IMatchGamesService matchGamesService,
			IMatchGameBoardService matchGameBoardService)
		{
			_logger = loggerFactory.CreateLogger<MatchGameBoardService>();
			_matchGameReadinessService = matchGameReadinessService;
			_matchGamesService = matchGamesService;
			_matchGameBoardService = matchGameBoardService;
		}

		public async Task ProcessService(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Processing the Match Game Board Service.");

			var matchGames = await _matchGamesService.GetMatchGamesToGenerateBoardAsync(cancellationToken);

			_logger.LogInformation("The number of games ready for board generation is {total}", matchGames.Count());

			foreach(var matchGame in matchGames)
			{
				_logger.LogInformation("Processing for match game with uid {matchGameUid} and name {matchGameName}",
					matchGame.Uid,
					matchGame.Name);

				if(await _matchGameReadinessService.IsReadyToGenerateBoardsAsync(matchGame.Uid, cancellationToken))
				{
					_logger.LogInformation("The game with uid {gameUid} and name {matchGameName} is ready to generate boards.",
						matchGame.Uid,
						matchGame.Name);

					await _matchGameBoardService.GenerateBoardsAsync(matchGame, cancellationToken);

					continue;
				}

				_logger.LogWarning("The game with uid {matchGameUid} and name {matchGameName} is NOT ready to generate boards.",
					matchGame.Uid,
					matchGame.Name);

				//TODO: Notify account holder about the issue for this game

				_logger.LogInformation("Finished processing for match game with uid {matchGameUid} and name {matchGameName}",
					matchGame.Uid,
					matchGame.Name);
			}

			_logger.LogInformation("Finished processing the Game Board Service");
		}
	}
}
