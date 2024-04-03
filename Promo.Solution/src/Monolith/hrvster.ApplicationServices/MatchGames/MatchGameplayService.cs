using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.Reporting;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.MatchGames
{
	public interface IMatchGameplayService
	{
		Task FulfillPrizeAsync(MatchPlay gamePlay, Guid prizeUid, CancellationToken cancellationToken);
		Task<bool> GameBeenPlayedAsync(Guid gameplayUid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchPlay>> GetAllGamePlays(Guid gameUid, CancellationToken cancellationToken);
		Task<MatchPlay> GetAsync(Guid gameplayUid, CancellationToken cancellationToken);
		Task<bool> IsAWinAsync(Guid gameUid, Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken);
		Task MarkAsPlayedAndLostAsync(Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken);
		Task MarkAsPlayedAndWinAsync(Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken);
		Task<Guid> ReserveGameboardAsync(
			Guid gameUid, string screenName, UserIdentifierType userIdentifierType, CancellationToken cancellationToken);
	}

	public class MatchGameplayService : IMatchGameplayService
	{
		private readonly ILogger<MatchGameplayService> _logger;
		private readonly IMatchGamePlayRepository _repository;
		private readonly IMatchTilesRepository _tilesRepository;
		private readonly IMatchBoardsRepository _boardsRepository;
		private readonly IMatchGamePlayRepository _gameplayRepository;
		private readonly IMatchGameBoardService _gameboardService;

		public MatchGameplayService(
			ILoggerFactory loggerFactory,
			IMatchGamePlayRepository repository,
			IMatchTilesRepository tilesRepository,
			IMatchBoardsRepository boardsRepository,
			IMatchGamePlayRepository gameplayRepository,
			IMatchGameBoardService gameboardService)
		{
			_logger = loggerFactory.CreateLogger<MatchGameplayService>();
			_repository = repository;
			_tilesRepository = tilesRepository;
			_boardsRepository = boardsRepository;
			_gameplayRepository = gameplayRepository;
			_gameboardService = gameboardService;
		}

		public async Task FulfillPrizeAsync(
			MatchPlay gamePlay, 
			Guid prizeUid,
			CancellationToken cancellationToken)
		{
			_logger.LogInformation("Fulfilling the prize {prizeUid} for the game play {gameplayUid}", prizeUid, gamePlay.Uid);

			await _repository.FulfillPrizeAsync(gamePlay.Uid, cancellationToken);
		}

		public async Task<Guid> ReserveGameboardAsync(
			Guid gameUid, 
			string screenName, 
			UserIdentifierType userIdentifierType, 
			CancellationToken cancellationToken)
		{
			_logger.LogInformation("Reserving gameboard.");
			var gameBoard = default(MatchBoard);

			gameBoard = await _gameboardService.GetRandomBoardAsync(gameUid, cancellationToken);
			if (gameBoard == null)
			{
				_logger.LogInformation("Could not get a random gameboard.");

				return Guid.Empty;
			}

			_logger.LogInformation("Retrieved the gameboard {gameboardUid} from the cache.", gameBoard.Uid);

			_logger.LogInformation("Creating a game play for the gameboard {gameboardUid}", gameBoard.Uid);
			var gameplayUid = await CreateGamePlayAsync(gameBoard, screenName, userIdentifierType, cancellationToken);

			_logger.LogInformation("Reserving the gameboard {gameboardUid}", gameBoard.Uid);
			await _gameboardService.ReserveBoardAsync(gameBoard, cancellationToken);


			return gameplayUid;
		}

		private async Task<Guid> CreateGamePlayAsync(
			MatchBoard gameBoard, string screenName, Models.UserIdentifierType handle, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Creating a game play for the gameboard {gameboardUid}", gameBoard.Uid);

			var gameplayUid = Guid.NewGuid();

			await _gameplayRepository.CreateAsync(gameplayUid, gameBoard.Id, screenName, handle, cancellationToken);

			return gameplayUid;
		}

		public async Task<bool> GameBeenPlayedAsync(Guid gameplayUid, CancellationToken cancellationToken)
		{
			var gameplay = await GetAsync(gameplayUid, cancellationToken);

			return gameplay.PlayedIndicator;
		}

		public async Task<IEnumerable<MatchPlay>> GetAllGamePlays(Guid gameUid, CancellationToken cancellationToken)
			=> await _gameplayRepository.GetForGamePlayAsync(gameUid, cancellationToken);

		public async Task<MatchPlay> GetAsync(Guid gameplayUid, CancellationToken cancellationToken)
			=> await _repository.GetAsync(gameplayUid, cancellationToken);

		public async Task<bool> IsAWinAsync(
			Guid gameUid, Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken)
		{
			
			var gameplay = await GetAsync(gameplayUid, cancellationToken);

			if (gameplay == null) return false;

			if (!gameplay.MatchBoard.IsWinnable) return false;

			var listOfTiles = new List<MatchTile>();

			var selectedTilesInGuid = selectedTiles.Split(',').ToList();

			foreach(var tileUid in selectedTilesInGuid)
			{
				listOfTiles.Add(await _tilesRepository.GetTileAsync(tileUid, cancellationToken));
			}

			if(!HaveTheSamePrize(listOfTiles))
			{
				return false;
			}

			return true;
		}

		public async Task MarkAsPlayedAndLostAsync(Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken)
		{
			await _repository.MarkAsPlayedAndLossAsync(gameplayUid, selectedTiles, cancellationToken);

			var gameplay = await GetAsync(gameplayUid, cancellationToken);

			await _boardsRepository.UpdateAsPlayedAsync(gameplay.MatchBoard.Uid, cancellationToken);
		}

		public async Task MarkAsPlayedAndWinAsync(Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken)
		{
			await _repository.MarkAsPlayedAndWonAsync(gameplayUid, selectedTiles, cancellationToken);

			var gameplay = await GetAsync(gameplayUid, cancellationToken);

			await _boardsRepository.UpdateAsPlayedAsync(gameplay.MatchBoard.Uid, cancellationToken);
		}

		private bool HaveTheSamePrize(List<MatchTile> listOfTiles)
		{
			var same = true;

			var firstTile = listOfTiles.FirstOrDefault();
			var firstPrize = firstTile.Prize;

			foreach(var tile in listOfTiles)
			{
				if (tile.Prize == null)
				{
					same = false;
					break;
				}

				if (tile.Prize.Id != firstPrize.Id)
				{
					same = false;
					break;
				}
			}

			return same;
		}
	}
}
