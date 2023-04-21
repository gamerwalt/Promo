using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Requests.MatchGames.Harvester;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.MatchGame
{
	public class ReserveGameboard : IRequestHandler<ReserveGameboardRequest, Guid>
	{
		private readonly ILogger<ReserveGameboard> _logger;
		private readonly IMatchGameBoardService _gameboardService;
		private readonly IMatchGamePlayRepository _gameplayRepository;

		public ReserveGameboard(
			ILoggerFactory loggerFactory,
			IMatchGameBoardService gameBoardService,
			IMatchGamePlayRepository gameplayRepository)
		{
			_logger = loggerFactory.CreateLogger<ReserveGameboard>();
			_gameboardService = gameBoardService;
			_gameplayRepository = gameplayRepository;
		}


		public async Task<Guid> Handle(ReserveGameboardRequest request, CancellationToken cancellationToken)
		{
			var gameUid = request.GameUid;
			var screenName = request.Tweet.ScreenName;
			var userId = request.Tweet.TweetId;

			using (_logger.BeginScope(new Dictionary<string, object>
			{
				{ "GameUid", gameUid },
				{ "ScreenName", screenName },
				{ "UserId", userId }
			}))
			{
				_logger.LogInformation("Reserving a gameboard and gameplay for the game with uid {gameUid} for {screenName}",
					gameUid,
					screenName);

				//1. Get a gameboard from the cahce, if not, get one from the database
				//2. Create a gameplay record and return the uid
				var gameBoard = default(MatchBoard);

				gameBoard = await _gameboardService.GetRandomBoardAsync(gameUid, cancellationToken);
				if (gameBoard == null)
				{
					_logger.LogInformation("Could not get a random gameboard from the cache.");

					_logger.LogInformation("Attempting to get a random gameboard uid.");

					var gameboardUid = await _gameboardService.GetRandomGameboardUidAsync(gameUid, cancellationToken);

					if (gameboardUid == Guid.Empty)
					{
						_logger.LogInformation("Getting all the gameboard uids of the game that are not reserved.");

						var gameBoardUids = await _gameboardService.GetGameBoardUidsAsync(gameUid, cancellationToken);
						var uids = gameBoardUids.ToList();

						_logger.LogInformation("Retrieved a total of {count} gameboard uids that are not reserved.", uids.Count);

						var randomIndex = RandomNumberGenerator.GetInt32(uids.Count);

						gameboardUid = uids[randomIndex];
					}

					gameBoard = await _gameboardService.GetGameBoardAsync(gameboardUid, cancellationToken);
				}

				//create a gameplay
				var gameplayUid = await CreateGamePlayAsync(gameBoard, screenName, request.Handle, cancellationToken);

				await _gameboardService.ReserveBoardAsync(gameBoard, cancellationToken);

				return gameplayUid;
			}
		}

		private async Task<Guid> CreateGamePlayAsync(
			MatchBoard gameBoard, string screenName, Models.UserIdentifierType handle, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Creating a game play for the gameboard {gameboardUid}", gameBoard.Uid);

			var gameplayUid = Guid.NewGuid();

			await _gameplayRepository.CreateAsync(gameplayUid, gameBoard.Id, screenName, handle, cancellationToken);

			return gameplayUid;
		}
	}
}
