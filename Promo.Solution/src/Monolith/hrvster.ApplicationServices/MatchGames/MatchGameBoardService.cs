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
		Task<Guid> GetRandomGameboardUidAsync(CancellationToken cancellationToken);
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

		private const double MinWinProbability = 16.6666;
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

			_logger.LogInformation("There is a total of {totalAvailablePrizes} prizes for the match game with Uid {matchGameUid} and name {matchName}",
				availablePrizes.Count,
				matchGame.Uid,
				matchGame.Name);

			for (var i = 0; i < numberOfWinnableBoards; i++)
			{
				var winProbability =
					MinWinProbability + random.NextDouble() *
					((matchGame.WinProbability / MaxWinProbability) * 100) - MinWinProbability;

				var prizeIndex = random.Next(availablePrizes.Count);
				var selectedPrize = availablePrizes[prizeIndex];
				availablePrizes.RemoveAt(prizeIndex);

				var tiles = new List<List<MatchTile>>(rows);
				var winningTilesCount = matchGame.WinProbability * rows * columns;
				var placedWinningTiles = 0;

				var board = new MatchBoard { Uid = Guid.NewGuid() };

				placedWinningTiles = GenerateBoardAndTiles(
					rows,
					columns,
					random,
					images,
					selectedPrize,
					tiles,
					winningTilesCount,
					placedWinningTiles,
					board,
					lossImage);

				board.Id = i;
				board.IsWinnable = true;
				board.WinProbability = winProbability;
				board.Tiles = tiles;
				board.MatchGame = matchGame;
				board.Prize = selectedPrize;

				generatedBoards.Add(board);
			}

			// Initialize remaining boards
			GenerateRemainingBoards(matchGame, rows, columns, numberOfBoards, generatedBoards, images, lossImage);

			//shuffle
			ShuffleBoards(numberOfBoards, random, generatedBoards);

			await SaveBoardsAsync(matchGame, generatedBoards, cancellationToken);

			_logger.LogInformation("");
		}

		private static void ShuffleBoards(int numberOfBoards, Random random, List<MatchBoard> generatedBoards)
		{
			for (int i = 0; i < numberOfBoards; i++)
			{
				int randomIndex = random.Next(i, numberOfBoards);
				SwapBoards(generatedBoards, i, randomIndex);
			}
		}

		private static void SwapBoards(List<MatchBoard> generatedBoards, int i, int randomIndex)
		{
			var temp = generatedBoards[i];

			generatedBoards[i] = generatedBoards[randomIndex];
			generatedBoards[randomIndex] = temp;
		}

		private static void GenerateRemainingBoards(
			MatchGame matchGame, 
			int rows, 
			int columns, 
			int numberOfBoards, 
			List<MatchBoard> generatedBoards, 
			IEnumerable<MatchImage> images,
			MatchImage lossImage)
		{
			for (int i = matchGame.NumberOfWinnableBoards; i < numberOfBoards; i++)
			{
				var board = new MatchBoard { Uid = Guid.NewGuid() };

				var tiles = new List<List<MatchTile>>(rows);

				GenerateTiles(rows, columns, images, board, tiles, lossImage);

				CreateBoard(matchGame, generatedBoards, i, board, tiles);
			}
		}

		private static void GenerateTiles(
			int rows, 
			int columns, 
			IEnumerable<MatchImage> images, 
			MatchBoard board, 
			List<List<MatchTile>> tiles,
			MatchImage lossImage)
		{
			for (int row = 0; row < rows; row++)
			{
				tiles.Add(new List<MatchTile>(columns));
				for (int col = 0; col < columns; col++)
				{
					AddTile(columns, images, board, tiles, row, col, lossImage);
				}
			}
		}

		private static void AddTile(
			int columns, 
			IEnumerable<MatchImage> images, 
			MatchBoard board, 
			List<List<MatchTile>> tiles, 
			int row, 
			int col,
			MatchImage lossImage)
		{
			tiles[row].Add(
				new MatchTile
				{
					Prize = null,
					WinIndicator = false,
					Uid = Guid.NewGuid(),
					Board = board,
					Image = lossImage
				});
		}

		private static int GenerateBoardAndTiles(
			int rows, 
			int columns, 
			Random random, 
			IEnumerable<MatchImage> images, 
			MatchPrize selectedPrize, 
			List<List<MatchTile>> tiles, 
			int winningTilesCount, 
			int placedWinningTiles, 
			MatchBoard board,
			MatchImage lossImage)
		{
			for (int row = 0; row < rows; row++)
			{
				tiles.Add(new List<MatchTile>(columns));

				for (int col = 0; col < columns; col++)
				{
					// Calculate the remaining total tiles
					int remainingTotalTiles = (rows * columns) - (row * columns + col);

					// Calculate the remaining winning tiles
					int remainingWinningTiles = winningTilesCount - placedWinningTiles;

					// If the ratio of remaining winning tiles to remaining total tiles is greater than or equal to the random probability,
					// place a winning tile, otherwise, place a non-winning tile
					bool isWinningTile = remainingWinningTiles > 0 && remainingWinningTiles / (double)remainingTotalTiles >= random.NextDouble();
					if (isWinningTile)
					{
						placedWinningTiles++;
					}

					var image = isWinningTile ?
								images.FirstOrDefault(i => i.Uid == selectedPrize.Image.Uid) :
								lossImage;
					var prize = isWinningTile ? selectedPrize : null;

					tiles[row].Add(
						new MatchTile
						{
							Prize = prize,
							WinIndicator = isWinningTile,
							Uid = Guid.NewGuid(),
							Image = image,
							Board = board
						});
				}
			}

			return placedWinningTiles;
		}

		private static void CreateBoard(
			MatchGame matchGame, List<MatchBoard> generatedBoards, int i, MatchBoard board, List<List<MatchTile>> tiles)
		{
			board.IsWinnable = false;
			board.WinProbability = 0;
			board.Tiles = tiles;
			board.MatchGame = matchGame;

			generatedBoards.Add(board);
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

			foreach(var board in generatedBoards)
			{
				_logger.LogInformation("Adding the board with uid {boardUid} to the database.", board.Uid);
				await _matchBoardsRepository.CreateAsync(board, cancellationToken);

				boardKeys.Add(board.Uid);

				_logger.LogInformation("Adding the board with uid {boardUid} to the cache.", board.Uid);
				
				var createdBoard = await _matchBoardsRepository.GetGameBoardAsync(board.Uid);

				board.Id = createdBoard.Id;

				await _cahceDataService.SetAsync(GenerateBoardKey(createdBoard), createdBoard);
			}

			_logger.LogInformation("Saving the board tiles for match game with uid {matchGameUid} and name {matchName}",
				matchGame.Uid,
				matchGame.Name);

			foreach(var board in generatedBoards)
			{
				foreach(var tiles in board.Tiles)
				{
					foreach(var tile in tiles)
					{
						_logger.LogInformation("Saving the tile with uid {tileUid} to the database", tile.Uid);
						await _matchTilesRepository.CreateAsync(tile, cancellationToken);
					}
				}
			}

			await _cahceDataService.SetAsync($"{CacheKeys.BoardKeys}", JsonConvert.SerializeObject(boardKeys));

			await _matchGameService.SetMatchGameAsProcessedAsync(matchGame, cancellationToken);
		}

		private static string GenerateBoardKey(MatchBoard board)
		{
			return $"{CacheKeys.GameBoards}{board.Uid}";
		}

		public async Task<MatchBoard> GetRandomBoardAsync(Guid gameUid, CancellationToken cancellationToken)
		{
			var boardKeys = await GetBoardKeysAsync();

			if (boardKeys == null || boardKeys.Count == 0)
			{
				//Todo: do something
			}

			var randomIndex = RandomNumberGenerator.GetInt32(boardKeys.Count);
			var randomBoardKey = boardKeys[randomIndex];

			var board = await _cahceDataService.GetAsync<MatchBoard>($"{CacheKeys.GameBoards}{randomBoardKey}");
			if (board == null)
			{
				//Todo: Load from database.
			}

			return board;
		}

		public async Task<Guid> GetRandomGameboardUidAsync(CancellationToken cancellationToken)
		{
			var boardKeys = await GetBoardKeysAsync();

			if (boardKeys == null || boardKeys.Count == 0)
			{
				return Guid.Empty;
			}

			var randomIndex = RandomNumberGenerator.GetInt32(boardKeys.Count);

			return boardKeys[randomIndex];
		}

		private async Task<List<Guid>> GetBoardKeysAsync()
		{
			var boardKeysString = await _cahceDataService.GetAsync<string>(CacheKeys.BoardKeys);
			var boardKeys = JsonConvert.DeserializeObject<List<Guid>>(boardKeysString);
			return boardKeys;
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
