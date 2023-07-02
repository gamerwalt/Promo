using hrvster.ApplicationServices.Caching;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.MatchGames
{
	public interface IMatchGameBoardService
	{
		Task GenerateBoardsAsync(MatchGame matchGame, CancellationToken cancellationToken);
		Task<MatchBoard> GetGameBoardAsync(Guid gameboardUid, CancellationToken cancellationToken);
		Task<IEnumerable<Guid>> GetGameBoardUidsAsync(Guid gameUid, CancellationToken cancellationToken);
		Task<MatchBoard> GetRandomBoardAsync(Guid gameUid, CancellationToken cancellationToken);
		Task<Guid> GetRandomGameboardUidAsync(Guid gameUid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchTile>> GetTilesAsync(Guid boardUid, CancellationToken cancellationToken);
		Task<bool> IsBoardPlayableByGameplayAsync(Guid gameplayUid, CancellationToken cancellationToken);
		Task ReserveBoardAsync(MatchBoard gameBoard, CancellationToken cancellationToken);
	}

	public class MatchGameBoardService : IMatchGameBoardService
	{
		private readonly ILogger<MatchGameBoardService> _logger;
		private readonly IMatchPrizeService _matchPrizeService;
		private readonly IMatchImageService _matchImageService;
		private readonly IMatchBoardsRepository _matchBoardsRepository;
		private readonly IMatchTilesRepository _matchTilesRepository;
		private readonly ICacheDataService _cahceDataService;
		private readonly IMatchGamesService _matchGameService;
		private readonly IMatchGamePlayRepository _gameplayRepository;

		private const double MinWinProbability = 1;
		private const int MaxWinProbability = 6;

		private const string LossImageUid = "d1776f2a-2cb4-485c-9da4-b02d3714a0c8";

		public MatchGameBoardService(
			ILoggerFactory loggerFactory,
			IMatchPrizeService matchPrizeService,
			IMatchImageService matchImageService,
			IMatchBoardsRepository matchBoardsRepository,
			IMatchTilesRepository matchTilesRepository,
			ICacheDataService cacheDataService,
			IMatchGamesService matchGamesService,
			IMatchGamePlayRepository gameplayRepository)
		{
			_logger = loggerFactory.CreateLogger<MatchGameBoardService>();
			_matchPrizeService = matchPrizeService;
			_matchImageService = matchImageService;
			_matchBoardsRepository = matchBoardsRepository;
			_matchTilesRepository = matchTilesRepository;
			_cahceDataService = cacheDataService;
			_matchGameService = matchGamesService;
			_gameplayRepository = gameplayRepository;
		}

		public async Task GenerateBoardsAsync(MatchGame matchGame, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Generating boards for match game with uid {matchGameUid} and name {matchName}",
				matchGame.Uid,
				matchGame.Name);

			var rows = Convert.ToInt32(Math.Sqrt(matchGame.TileCountPerBoard));
			var columns = rows;

			var numberOfBoards = matchGame.NumberOfBoards;
			var numberOfWinnableBoards = matchGame.NumberOfWinnableBoards;
			var random = new Random();
			var generatedBoards = new List<MatchBoard>(numberOfBoards);

			//set up loss images for this game
			await _matchImageService.UpdateLossImageAsync(matchGame.Uid, cancellationToken);

			var prizes = await _matchPrizeService.GetAllForMatchGameAsync(matchGame.Uid, cancellationToken);
			var prizeList = prizes.ToList();
			var images = await _matchImageService.GetForMatchGameAsync(matchGame.Uid, cancellationToken);
			var lossImage = images.FirstOrDefault(c => c.IsLoss);
			var availablePrizes = new List<MatchPrize>();

			_logger.LogInformation("Retrieved a total of {totalPrizeCount} prizes for match game with uid {matchGameUid} and name {matchName}",
				prizeList.Count,
				matchGame.Uid,
				matchGame.Name);

			foreach (var prize in prizeList)
			{
				for (var i = 0; i < prize.Quantity; i++)
				{
					availablePrizes.Add(prize);
				}
			}

			for(var i = 0; i < numberOfWinnableBoards; i++)
				generatedBoards.Add(GenerateBoard(matchGame, matchGame.WinProbability, availablePrizes, true));

			for (var i = numberOfWinnableBoards; i < numberOfBoards; i++)
				generatedBoards.Add(GenerateBoard(matchGame, 0, availablePrizes, false));

			generatedBoards = generatedBoards.OrderBy(x => Guid.NewGuid()).ToList();

			await SaveBoardsAsync(matchGame, generatedBoards, cancellationToken);
		}

		private MatchBoard GenerateBoard(
			MatchGame matchGame, int winProbability, List<MatchPrize> availablePrizes, bool winnable)
		{
			var normalizedWinProbability = (winProbability - 1) / 5.0;

			var board = new MatchBoard
			{
				Uid = Guid.NewGuid(),
				MatchGame = matchGame,
				WinProbability = normalizedWinProbability,
				IsWinnable = winnable
			};

			var winningTilesCount = (int)Math.Ceiling(6 * normalizedWinProbability + 2);

			winningTilesCount = Math.Max(3, Math.Min(winningTilesCount, 9));

			var randomIndex = RandomNumberGenerator.GetInt32(availablePrizes.Count);
			var winningPrize = availablePrizes[randomIndex];

			var tiles = new List<MatchTile>();
			for (var i = 0; i < winningTilesCount; i++)
			{
				tiles.Add(new MatchTile
				{
					Uid = Guid.NewGuid(),
					Board = board,
					WinIndicator = true,
					Prize = winningPrize,
					Image = winningPrize.Image
				});
			}
			board.Prize = winningPrize;

			var otherPrizes = availablePrizes.Except(new List<MatchPrize> { winningPrize }).ToList();
			int otherPrizesMaxCount = (normalizedWinProbability >= 0.5) ? 2 : 1;

			while(tiles.Count < 9)
			{
				foreach(var prize in otherPrizes)
				{
					for(var i = 0; i < otherPrizesMaxCount; i++)
					{
						tiles.Add(new MatchTile
						{
							Uid = Guid.NewGuid(),
							Board = board,
							WinIndicator = false,
							Prize = prize,
							Image = prize.Image
						});

						if (tiles.Count >= 9)
							break;
					}

					if (tiles.Count >= 9)
						break;
				}
			}

			tiles = tiles.OrderBy(t => Guid.NewGuid()).ToList();

			board.Tiles = tiles;

			return board;
		}

		private async Task SaveBoardsAsync(
			MatchGame matchGame, 
			List<MatchBoard> generatedBoards, 
			CancellationToken cancellationToken)
		{
			var boardKeys = new List<Guid>();

			_logger.LogInformation("Saving the boards for match game with uid {matchGameUid} and name {matchName}",
				matchGame.Uid,
				matchGame.Name);

			foreach (var board in generatedBoards)
			{
				await _matchBoardsRepository.CreateAsync(board, cancellationToken);

				boardKeys.Add(board.Uid);

				var createdBoard = await _matchBoardsRepository.GetGameBoardAsync(board.Uid);

				board.Id = createdBoard.Id;

				await _cahceDataService.SetAsync(GenerateBoardKey(createdBoard), createdBoard);
			}

			_logger.LogInformation("Saving the board tiles for match game with uid {matchGameUid} and name {matchName}",
				matchGame.Uid,
				matchGame.Name);

			foreach (var board in generatedBoards)
			{
				foreach (var tile in board.Tiles)
				{
					await _matchTilesRepository.CreateAsync(tile, cancellationToken);
				}
			}

			await _cahceDataService.SetAsync(
				GenerateMatchBoardKeysForTheUids(matchGame.Uid), JsonConvert.SerializeObject(boardKeys));

			await _matchGameService.SetMatchGameAsProcessedAsync(matchGame, cancellationToken);
		}

		private static string GenerateMatchBoardKeysForTheUids(Guid gameUid)
		{
			return $"{CacheKeys.BoardKeys}-{gameUid}";
		}

		private static string GenerateBoardKey(MatchBoard board)
		{
			return $"{CacheKeys.GameBoards}{board.Uid}";
		}

		public async Task<MatchBoard> GetRandomBoardAsync(Guid gameUid, CancellationToken cancellationToken)
		{
			var boardKeys = await GetBoardKeysFromCahceOrDatabase(gameUid, cancellationToken);

			var randomIndex = RandomNumberGenerator.GetInt32(boardKeys.Count);
			var randomBoardUid = boardKeys[randomIndex];

			var board = await _cahceDataService.GetAsync<MatchBoard>($"{CacheKeys.GameBoards}{randomBoardUid}");
			if (board == null)
			{
				board = await _matchBoardsRepository.GetGameBoardAsync(randomBoardUid);
			}

			return board;
		}

		public async Task<Guid> GetRandomGameboardUidAsync(Guid gameUid, CancellationToken cancellationToken)
		{
			var boardKeys = await GetBoardKeysFromCahceOrDatabase(gameUid, cancellationToken);

			var randomIndex = RandomNumberGenerator.GetInt32(boardKeys.Count);

			return boardKeys[randomIndex];
		}

		private async Task<List<Guid>> GetBoardKeysFromCahceOrDatabase(Guid gameUid, CancellationToken cancellationToken)
		{
			var boardKeys = await GetBoardKeysAsync(gameUid);

			if (boardKeys == null || boardKeys.Count == 0)
			{
				var boardUids = await _matchBoardsRepository.GetGameBoardUidsAsync(gameUid, cancellationToken);
				boardKeys = boardUids.ToList();
			}

			return boardKeys;
		}

		private async Task<List<Guid>> GetBoardKeysAsync(Guid gameUid)
		{
			var boardKeysString = await _cahceDataService.GetAsync<string>(GenerateMatchBoardKeysForTheUids(gameUid));
			
			return JsonConvert.DeserializeObject<List<Guid>>(boardKeysString);
		}

		public async Task<MatchBoard> GetGameBoardAsync(Guid gameboardUid, CancellationToken cancellationToken)
			=> await _matchBoardsRepository.GetGameBoardAsync(gameboardUid);

		public async Task<IEnumerable<Guid>> GetGameBoardUidsAsync(Guid gameUid, CancellationToken cancellationToken)
			=> await _matchBoardsRepository.GetGameBoardUidsAsync(gameUid, cancellationToken);

		public async Task ReserveBoardAsync(MatchBoard gameBoard, CancellationToken cancellationToken)
		{
			await _matchBoardsRepository.ReserveBoardAsync(gameBoard.Uid, cancellationToken);

			await _cahceDataService.RemoveItemFromCacheAsync(GenerateBoardKey(gameBoard));

			await _cahceDataService.SetAsync(GenerateBoardKey(gameBoard), gameBoard);
		}

		public async Task<bool> IsBoardPlayableByGameplayAsync(Guid gameplayUid, CancellationToken cancellationToken)
		{
			var gameplay = await _gameplayRepository.GetAsync(gameplayUid, cancellationToken);
			if(gameplay == null)
			{
				_logger.LogWarning("Gameplay doesn't exist for {uid}", gameplayUid);
				return false;
			}

			if(gameplay.MatchBoard == null)
			{
				_logger.LogWarning("Gameplay with uid {uid} is not attached to a gameboard.", gameplayUid);
				return false;
			}

			if(!gameplay.MatchBoard.IsReserved)
			{
				_logger.LogWarning("Gameboard with uid {boardUid} for gameplay {uid} is not reserved",
					gameplay.MatchBoard.Uid,
					gameplay.Uid);
				return false;
			}

			if(gameplay.PlayedIndicator || gameplay.MatchBoard.PlayedIndicator)
			{
				_logger.LogWarning("Either the gameboard with uid {boardUid} or the gameplay with uid {uid} has been played.",
					gameplay.MatchBoard.Uid,
					gameplay.Uid);
				return false;
			}

			return true;
		}

		public async Task<IEnumerable<MatchTile>> GetTilesAsync(Guid boardUid, CancellationToken cancellationToken)
		{
			return await _matchTilesRepository.GetForBoardAsync(boardUid, cancellationToken);
		}
	}
}
