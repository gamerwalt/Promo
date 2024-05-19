using hrvster.ApplicationServices.Caching;
using hrvster.ApplicationServices.Exceptions;
using hrvster.ApplicationServices.Extensions.LogExtensions;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.MongoEntities.MatchGames;
using hrvster.Infrastructure.MongoRepositories;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using MimeKit.Cryptography;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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
		Task RemoveBoardFromCacheAsync(Guid boardUid, CancellationToken cancellationToken);
		Task GenerateBoardsWithNoTilesAsync(MatchGame matchGame, CancellationToken token);
	}

	public class MatchGameBoardService : IMatchGameBoardService
	{
		private readonly ILogger<MatchGameBoardService> _logger;
		private readonly IMatchPrizeService _matchPrizeService;
		private readonly IMatchImageService _matchImageService;
		private readonly IMatchBoardsRepository _matchBoardsRepository;
		private readonly IMatchTilesRepository _matchTilesRepository;
		private readonly ICacheDataService _cacheDataService;
		private readonly IMatchGamesService _matchGameService;
		private readonly IMatchGamePlayRepository _gameplayRepository;
		private readonly IMongoMatchTileRepository _mongoMatchTileRepository;

		private const double MinWinProbability = 1;
		private const int MaxWinProbability = 6;

		private const string LossImageUid = "d1776f2a-2cb4-485c-9da4-b02d3714a0c8";
		private const string BoardKeysKeyName = "boards:keys";
		private const string BoardKeyKeyName = "board: ";
		private const string TileKeysName = "tiles: ";

		public MatchGameBoardService(
			ILoggerFactory loggerFactory,
			IMatchPrizeService matchPrizeService,
			IMatchImageService matchImageService,
			IMatchBoardsRepository matchBoardsRepository,
			IMatchTilesRepository matchTilesRepository,
			ICacheDataService cacheDataService,
			IMatchGamesService matchGamesService,
			IMatchGamePlayRepository gameplayRepository,
			IMongoMatchTileRepository mongoMatchTileRepository)
		{
			_logger = loggerFactory.CreateLogger<MatchGameBoardService>();
			_matchPrizeService = matchPrizeService;
			_matchImageService = matchImageService;
			_matchBoardsRepository = matchBoardsRepository;
			_matchTilesRepository = matchTilesRepository;
			_cacheDataService = cacheDataService;
			_matchGameService = matchGamesService;
			_gameplayRepository = gameplayRepository;
			_mongoMatchTileRepository = mongoMatchTileRepository;
		}

		public async Task GenerateBoardsWithNoTilesAsync(MatchGame matchGame, CancellationToken token)
		{
			_logger.LogInformation("Generating board with no tiles started.");

			var numberOfBoards = matchGame.NumberOfBoards;
			var numberOfWinnableBoards = matchGame.NumberOfWinnableBoards;

			_logger.LogInformation("Number of boards: {NumberOfBoards}, number of winnable boards: {NumberOfWinnableBoards}.",
					numberOfBoards,
					numberOfWinnableBoards);

			_logger.LogInformation("Updating loss image for match game.");
			await _matchImageService.UpdateLossImageAsync(matchGame.Uid, token);

			_logger.LogInformation("Retrieving prizes for match game.");

			var prizes = await _matchPrizeService.GetAllForMatchGameAsync(matchGame.Uid, token);
			var prizeList = prizes.Where(c => !c.IsFiller).ToList();

			_logger.LogInformation("Total prizes retrieved {TotalPrizes}.", prizeList.Count);

			var availablePrizes = new List<MatchPrize>();
			foreach (var prize in prizeList)
			{
				for (var i = 0; i < prize.Quantity; i++)
				{
					availablePrizes.Add(prize);
				}
				_logger.LogInformation("Added the prize {PrizeName} to available prizes.", prize.Name);
			}

			availablePrizes.Shuffle();

			var options = new ParallelOptions
			{
				CancellationToken = token,
				MaxDegreeOfParallelism = Environment.ProcessorCount
			};

			_logger.LogInformation("Generating boards with no tiles.");

			await GenerateAndSaveBoardsWithNoTilesAsync(matchGame, numberOfBoards, availablePrizes, options, token);

			_logger.LogInformation("Board generation completed.");
		}

		private async Task GenerateAndSaveBoardsWithNoTilesAsync(
			MatchGame matchGame,
			int numberOfBoards,
			List<MatchPrize> availablePrizes,
			ParallelOptions options,
			CancellationToken cancellationToken)
		{
			var generatedBoards = new List<MatchBoard>();

			await Parallel.ForEachAsync(Enumerable.Range(0, numberOfBoards), options, async (index, token) =>
			{
				var isWinnable = index < matchGame.NumberOfWinnableBoards;
				var board = GenerateBoardWithNoTiles(matchGame, isWinnable ? matchGame.WinProbability : 0, availablePrizes, isWinnable);
				lock (generatedBoards)
				{
					generatedBoards.Add(board);
				}
				await Task.CompletedTask;
			});

			await SaveBoardsWithNoTilesAsync(matchGame, generatedBoards, cancellationToken);
		}

		private MatchBoard GenerateBoardWithNoTiles(
			MatchGame matchGame, 
			int winProbability, 
			List<MatchPrize> availablePrizes, 
			bool winnable)
		{
			var normalizedWinProbability = (winProbability - 1) / 5.0;

			var board = new MatchBoard
			{
				Uid = Guid.NewGuid(),
				MatchGame = matchGame,
				WinProbability = normalizedWinProbability,
				IsWinnable = winnable,
				Tiles = new List<MatchTile>(9)
			};

			var randomIndex = RandomNumberGenerator.GetInt32(availablePrizes.Count);
			var winningPrize = availablePrizes[randomIndex];
			board.Prize = winningPrize;

			return board;
		}

		private async Task SaveBoardsWithNoTilesAsync(
			MatchGame matchGame, 
			List<MatchBoard> boards, 
			CancellationToken cancellationToken)
		{
			var batchSize = 40000;  // Set a reasonable batch size for bulk operations
			for (var i = 0; i < boards.Count; i += batchSize)
			{
				var batch = boards.Skip(i).Take(batchSize).ToList();

				_logger.LogInformation("Saving batch of {BatchCount} boards, batch start index: {I}", batch.Count, i);

				await _matchBoardsRepository.BulkInsertBoardsAndNoTilesAsync(matchGame, batch, cancellationToken);
			}

			_logger.LogInformation("Boards saved successfully into the database.");

			_logger.LogInformation("Retrieving the board ids of all inserted boards started.");

			var boardMappings = await _matchBoardsRepository.GetInsertedBoardIds(boards, cancellationToken);

			_logger.LogInformation("Retrieving the board ids of all inserted boards finished.");

			_logger.LogInformation("Saving boards into redis started.");

			var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
			await Parallel.ForEachAsync(boards, options, async (board, cancellationToken) =>
			{
				var key = GetBoardKeyName(board.Uid);
				var cachedBoard = new CachedBoard
				{
					Id = boardMappings[board.Uid],
					Uid = board.Uid,
					IsWinnable = board.IsWinnable,
					IsReserved = board.IsReserved,
					PrizeId = board.Prize.Id,
					PrizeUid = board.Prize.Uid,
					MatchGameId = board.MatchGame.Id,
					MatchGameUid = board.MatchGame.Uid,
					MatchGameName = board.MatchGame.Name,
				};
				
				await _cacheDataService.SetAddAsync(GetBoardKeysKeyName(matchGame.Uid), board.Uid.ToString());
				await _cacheDataService.SetAsync(key, cachedBoard);
			});


			_logger.LogInformation("Saving boards keys into redis completed.");

			_logger.LogInformation("Setting the match game as processed.");
			await _matchGameService.SetMatchGameAsProcessedAsync(matchGame, cancellationToken);
		}

		public async Task GenerateBoardsAsync(MatchGame matchGame, CancellationToken cancellationToken)
		{
			using (_logger.BeginGenerateBoard(matchGame.Name, matchGame.Uid))
			{
				_logger.LogInformation("Generating board started.");

				var rows = Convert.ToInt32(Math.Sqrt(matchGame.TileCountPerBoard));
				var columns = rows;

				_logger.LogInformation("Total rows: {rows}, total columns: {columns}.", rows, columns);

				var numberOfBoards = matchGame.NumberOfBoards;
				var numberOfWinnableBoards = matchGame.NumberOfWinnableBoards;
				var random = new Random();
				var generatedBoards = new ConcurrentBag<MatchBoard>();

				_logger.LogInformation("Number of boards: {numberOfBoards}, number of winnable boards: {numberOfWinnableBoards}.",
					numberOfBoards,
					numberOfWinnableBoards);

				_logger.LogInformation("Updating loss image for match game.");
				await _matchImageService.UpdateLossImageAsync(matchGame.Uid, cancellationToken);

				_logger.LogInformation("Retrieving prizes for match game.");

				var prizes = await _matchPrizeService.GetAllForMatchGameAsync(matchGame.Uid, cancellationToken);
				var prizeList = prizes.ToList();

				_logger.LogInformation("Total prizes retrieved {totalPrizes}.", prizeList.Count);

				var images = await _matchImageService.GetForMatchGameAsync(matchGame.Uid, cancellationToken);
				var lossImage = images.FirstOrDefault(c => c.IsLoss);

				var availablePrizes = new List<MatchPrize>();

				_logger.LogInformation("Generating available prizes.");
				var lossPrize = prizeList.FirstOrDefault(c => c.IsFiller);
				if (lossPrize != null)
				{
					prizeList.Remove(lossPrize);
				}

				foreach (var prize in prizeList)
				{
					for (var i = 0; i < prize.Quantity; i++)
					{
						availablePrizes.Add(prize);
					}
					_logger.LogInformation("Added the prize {prizeName} to available prizes.", prize.Name);
				}

				ParallelOptions options = new ParallelOptions
				{
					CancellationToken = cancellationToken,
					MaxDegreeOfParallelism = Environment.ProcessorCount
				};

				_logger.LogInformation("Generating boards.");
				await GenerateAndSaveBoardsAsync(matchGame, numberOfBoards, availablePrizes, options, cancellationToken);

				_logger.LogInformation("Board generation completed.");
			}
		}

		private async Task GenerateAndSaveBoardsAsync(
			MatchGame matchGame, 
			int numberOfBoards, 
			List<MatchPrize> availablePrizes, 
			ParallelOptions options, 
			CancellationToken cancellationToken)
		{
			var numberOfThreads = Math.Min(numberOfBoards/ 1000000, Environment.ProcessorCount);

			var boardsPerThread = numberOfBoards / numberOfThreads;

			var uniqueBoardUids = new ConcurrentDictionary<Guid, bool>();
			var tasks = new List<Task>();

			for(var threadIndex = 0; threadIndex < numberOfThreads; threadIndex++)
			{
				int start = threadIndex * boardsPerThread;
				int end = (threadIndex == numberOfThreads - 1) ? numberOfBoards : start + boardsPerThread;

				tasks.Add(Task.Run(()
					=> ProcessBoardsAndSaveAsync(matchGame, start, end, availablePrizes, uniqueBoardUids, cancellationToken)));
			}

			await Task.WhenAll(tasks);
			//await Parallel.ForEachAsync(Enumerable.Range(0, numberOfBoards), options, async (index, token) =>
			//{
			//	var isWinnable = index < matchGame.NumberOfWinnableBoards;
			//	var board = GenerateBoard(matchGame, isWinnable ? matchGame.WinProbability : 0, availablePrizes, isWinnable);

			//	generatedBoards.Add(board);
			//	await Task.CompletedTask;
			//});

			//_logger.LogInformation("Shuffling boards.");
			//var shuffledBoards = ShuffleBoards(generatedBoards.ToList());

			//_logger.LogInformation("Saving boards.");
			//await SaveBoardsAsync2(matchGame, shuffledBoards, cancellationToken);
		}

		private async Task ProcessBoardsAndSaveAsync(
			MatchGame matchGame, 
			int start, 
			int end, 
			List<MatchPrize> availablePrizes, 
			ConcurrentDictionary<Guid, bool> uniqueBoardUids, 
			CancellationToken cancellationToken)
		{
			var generatedBoards = new ConcurrentBag<MatchBoard>();

			for (var i = start; i < end; i++)
			{
				var isWinnable = i < matchGame.NumberOfWinnableBoards;
				var board = GenerateBoard(matchGame, isWinnable ? matchGame.WinProbability : 0, availablePrizes, isWinnable);

				if (uniqueBoardUids.TryAdd(board.Uid, true))
				{
					generatedBoards.Add(board);
				}
				else
				{
					_logger.LogWarning($"Duplicate UID generated: {board.Uid}");
				}
			}

			await SaveBoardsAsync2(matchGame, generatedBoards, cancellationToken);
		}

		private List<MatchBoard> ShuffleBoards(List<MatchBoard> boards)
		{
			Random rng = new Random();
			int n = boards.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				MatchBoard value = boards[k];
				boards[k] = boards[n];
				boards[n] = value;
			}
			return boards;
		}

		private MatchBoard GenerateBoard(MatchGame matchGame, int winProbability, List<MatchPrize> availablePrizes, bool winnable)
		{
			var normalizedWinProbability = (winProbability - 1) / 5.0;

			var board = new MatchBoard
			{
				Uid = Guid.NewGuid(),
				MatchGame = matchGame,
				WinProbability = normalizedWinProbability,
				IsWinnable = winnable,
				Tiles = new List<MatchTile>(9)
			};

			var winningTilesCount = (int)Math.Ceiling(6 * normalizedWinProbability + 2);
			winningTilesCount = Math.Max(3, Math.Min(winningTilesCount, 9));

			var randomIndex = RandomNumberGenerator.GetInt32(availablePrizes.Count);
			var winningPrize = availablePrizes[randomIndex];

			board.Prize = winningPrize;
			GenerateTiles(board, winningPrize, true, winningTilesCount);

			var otherPrizes = availablePrizes.Except(new List<MatchPrize> { winningPrize }).ToList();
			var otherPrizesMaxCount = (normalizedWinProbability >= 0.5) ? 2 : 1;

			GenerateLoosingTiles(board, otherPrizes, false, 9 - winningTilesCount, otherPrizesMaxCount);

			// Ensure the board always has 9 tiles
			while (board.Tiles.Count < 9)
			{
				var fillerPrize = availablePrizes[RandomNumberGenerator.GetInt32(availablePrizes.Count)];
				board.Tiles.Add(new MatchTile
				{
					Uid = Guid.NewGuid(),
					Board = board,
					WinIndicator = false,
					Prize = fillerPrize,
					Image = fillerPrize.Image
				});
			}

			board.Tiles.Shuffle();

			if (board.Tiles.Any(c => c.Uid == Guid.Empty))
			{
				_logger.LogWarning($"The board {board.Uid} has tiles where the Guid is empty. So has less than 9 tiles.");
			}

			return board;
		}

		private void GenerateTiles(MatchBoard board, MatchPrize prize, bool winIndicator, int count)
		{
			for (var i = 0; i < count; i++)
			{
				board.Tiles.Add(new MatchTile
				{
					Uid = Guid.NewGuid(),
					Board = board,
					WinIndicator = winIndicator,
					Prize = prize,
					Image = prize.Image
				});
			}
		}

		private void GenerateLoosingTiles(MatchBoard board, List<MatchPrize> prizes, bool winIndicator, int count, int maxPerPrize = 1)
		{
			var tileAdded = 0;
			foreach (var prize in prizes)
			{
				for (var i = 0; i < maxPerPrize; i++)
				{
					if (tileAdded >= count) return;  // Stop when enough tiles are added

					board.Tiles.Add(new MatchTile
					{
						Uid = Guid.NewGuid(),
						Board = board,
						WinIndicator = winIndicator,
						Prize = prize,
						Image = prize.Image
					});
					tileAdded++;
				}
			}
		}


		private async Task SaveBoardsAsync2(MatchGame matchGame, ConcurrentBag<MatchBoard> boards, CancellationToken cancellationToken)
		{
			var batchSize = 50000;  // Set a reasonable batch size for bulk operations
			for (var i = 0; i < boards.Count; i += batchSize)
			{
				var batch = boards.Skip(i).Take(batchSize).ToList();
				_logger.LogInformation($"Saving batch of {batch.Count} boards, batch start index: {i}");
				await _matchBoardsRepository.BulkInsertBoardsAndTiles(matchGame, batch, cancellationToken);
			}

			_logger.LogInformation("All boards saved successfully.");

			_logger.LogInformation("Retrieving the board ids of all inserted boards started.");

			var boardMappings = await _matchBoardsRepository.GetInsertedBoardIds(boards.ToList(), cancellationToken);

			_logger.LogInformation("Retrieving the board ids of all inserted boards finished.");

			_logger.LogInformation("Saving boards into redis started.");

			var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
			await Parallel.ForEachAsync(boards, options, async (board, cancellationToken) =>
			{
				var key = GetBoardKeyName(board.Uid);
				var tileKey = GetTileKeysName(board.Uid);
				var cachedBoard = new CachedBoard
				{
					Id = boardMappings[board.Uid],
					Uid = board.Uid,
					IsWinnable = board.IsWinnable,
					IsReserved = board.IsReserved,
					PrizeId = board.Prize.Id,
					PrizeUid = board.Prize.Uid,
					MatchGameId = board.MatchGame.Id,
					MatchGameUid = board.MatchGame.Uid,
					MatchGameName = board.MatchGame.Name,
				};

				var cachedTiles = board.Tiles.Select(tile => new CachedTile
				{
					TileUid = tile.Uid,
					ImageId = tile.Image.Id,
					ImageUid = tile.Image.Uid,
					ImageUrl = tile.Image.LocationUrl,
					PrizeId = tile.WinIndicator ? tile.Prize.Id : 0,
					PrizeUid = tile.WinIndicator ? tile.Prize.Uid : Guid.Empty,
					WinIndicator = tile.WinIndicator
				}).ToList();

				await _cacheDataService.SetAddAsync(GetBoardKeysKeyName(matchGame.Uid), board.Uid.ToString());

				await _cacheDataService.SetAsync(key, cachedBoard);
				await _cacheDataService.SetAsync(tileKey, cachedTiles);
			});


			_logger.LogInformation("Saving boards keys into redis completed.");

			_logger.LogInformation("Setting the match game as processed.");
			await _matchGameService.SetMatchGameAsProcessedAsync(matchGame, cancellationToken);
		}

		private static string GetBoardKeyName(Guid boardUid)
		{
			return $"{BoardKeyKeyName}{boardUid}";
		}

		private static string GetBoardKeysKeyName(Guid gameUid)
		{
			return $"{BoardKeysKeyName}.{gameUid}";
		}

		private static string GetTileKeysName(Guid boardUid)
		{
			return $"{TileKeysName}{boardUid}";
		}

		private async Task SaveBoardsAsync(
			MatchGame matchGame, 
			List<MatchBoard> generatedBoards, 
			CancellationToken cancellationToken)
		{
			var boardKeys = new List<Guid>();

			_logger.LogInformation("Saving the boards for match game.");

			foreach (var board in generatedBoards)
			{
				_logger.LogInformation("Saving the board with uid {boardUid} to the database.", board.Uid);
				await _matchBoardsRepository.CreateAsync(board, cancellationToken);

				_logger.LogInformation("Adding the board with uid {boardUid} to the board keys.", board.Uid);
				boardKeys.Add(board.Uid);

				_logger.LogInformation("Retrieving the board with uid {boardUid} from the database.", board.Uid);
				var createdBoard = await _matchBoardsRepository.GetGameBoardAsync(board.Uid);

				_logger.LogInformation("Updating the board id to {boardId}.", createdBoard.Id);
				board.Id = createdBoard.Id;

				_logger.LogInformation("Saving the board keys to cache.");
				await _cacheDataService.SetAsync(GenerateBoardKey(createdBoard), createdBoard);
			}

			_logger.LogInformation("Saving the board tiles for match game.");


			_logger.LogInformation("Saving the board tiles to the database.");
			foreach (var board in generatedBoards)
			{
				foreach (var tile in board.Tiles)
				{
					await _matchTilesRepository.CreateAsync(tile, cancellationToken);
				}
			}
			_logger.LogInformation("Saving the board tiles to the database finished.");

			_logger.LogInformation("Saving the board keys to cache.");
			await _cacheDataService.SetAsync(
				GenerateMatchBoardKeysForTheUids(matchGame.Uid), JsonConvert.SerializeObject(boardKeys));
			_logger.LogInformation("Saving the board keys to cache finished.");

			_logger.LogInformation("Setting the match game as processed.");
			await _matchGameService.SetMatchGameAsProcessedAsync(matchGame, cancellationToken);

			_logger.LogInformation("Saving the boards for match game finished.");
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
			var randomBoard = default(MatchBoard);
			var randomBoardUid = await GetBoardKeysFromCahceOrDatabase(gameUid, cancellationToken);

			_logger.LogInformation("Retrieving the board with uid {boardUid} from the cache.", randomBoardUid);

			return await GetBoardFromCacheAsync(randomBoardUid);
		}

		private async Task<MatchBoard> GetBoardFromCacheAsync(Guid randomBoardUid)
		{
			var boardString = await _cacheDataService.GetBoardDetailsAsync(GetBoardKeyName(randomBoardUid));

			if (string.IsNullOrWhiteSpace(boardString))
			{
				throw new NoMoreBoardForThisGameException($"There are no more boards for the game");
			}

			var unescapedBoardString = Regex.Unescape(boardString);

			var cachedBoard = JsonConvert.DeserializeObject<CachedBoard>(unescapedBoardString);

			var tileString = await _cacheDataService.GetTileDetailsAsync(GetTileKeysName(cachedBoard.Uid));

			if (string.IsNullOrWhiteSpace(tileString))
			{
				throw new NoMoreBoardForThisGameException($"There are no tiles for the game");
			}

			var cachedTiles = JsonConvert.DeserializeObject<List<CachedTile>>(Regex.Unescape(tileString));
			
			return new MatchBoard
			{
				Id = cachedBoard.Id,
				Uid = cachedBoard.Uid,
				MatchGame = new MatchGame { Id = cachedBoard.MatchGameId, Uid = cachedBoard.MatchGameUid, Name = cachedBoard.MatchGameName },
				Prize = new MatchPrize { Id = cachedBoard.PrizeId, Uid = cachedBoard.PrizeUid },
				IsWinnable = cachedBoard.IsWinnable,
				IsReserved = cachedBoard.IsReserved,
				Tiles = cachedTiles.Select(tile => new MatchTile
				{
					Uid = tile.TileUid,
					Image = new MatchImage { Id = tile.ImageId, Uid = tile.ImageUid, LocationUrl = tile.ImageUrl },
					Prize = new MatchPrize { Id = tile.PrizeId, Uid = tile.PrizeUid },
					WinIndicator = tile.WinIndicator
				}).ToList()
			};
		}

		public Task<Guid> GetRandomGameboardUidAsync(Guid gameUid, CancellationToken cancellationToken)
		{
			return GetBoardKeysFromCahceOrDatabase(gameUid, cancellationToken);
		}

		private async Task<Guid> GetBoardKeysFromCahceOrDatabase(Guid gameUid, CancellationToken cancellationToken)
		{
			var randomBoardUidString = await _cacheDataService.GetRandomBoardKey(GetBoardKeysKeyName(gameUid));

			var boardUid = randomBoardUidString.ToString();

			if(string.IsNullOrWhiteSpace(boardUid))
			{
				_logger.LogError($"There are no more boards for the game with uid {gameUid}.");
				throw new NoMoreBoardForThisGameException("There are no more boards for this game.");
			}

			return Guid.Parse(boardUid);
		}

		private async Task<List<Guid>> GetBoardKeysAsync(Guid gameUid)
		{
			var boardKeysString = await _cacheDataService.GetAsync<string>(GenerateMatchBoardKeysForTheUids(gameUid));

			if (string.IsNullOrWhiteSpace(boardKeysString))
			{
				return null;
			}

			return JsonConvert.DeserializeObject<List<Guid>>(boardKeysString);
		}

		public async Task<MatchBoard> GetGameBoardAsync(Guid gameBoardUid, CancellationToken cancellationToken)
		{
			return await GetBoardFromCacheAsync(gameBoardUid);
		}
			

		public async Task<IEnumerable<Guid>> GetGameBoardUidsAsync(Guid gameUid, CancellationToken cancellationToken)
			=> await _matchBoardsRepository.GetGameBoardUidsAsync(gameUid, cancellationToken);

		public async Task ReserveBoardAsync(MatchBoard gameBoard, CancellationToken cancellationToken)
		{
			var board = await GetBoardFromCacheAsync(gameBoard.Uid);

			await _matchBoardsRepository.ReserveBoardAsync(gameBoard.Uid, cancellationToken);

			//update the board
			var key = GetBoardKeyName(board.Uid);
			var cachedBoard = new CachedBoard
			{
				Id = board.Id,
				Uid = board.Uid,
				IsWinnable = board.IsWinnable,
				IsReserved = true,
				PrizeId = board.Prize.Id,
				PrizeUid = board.Prize.Uid,
				MatchGameId = board.MatchGame.Id,
				MatchGameUid = board.MatchGame.Uid,
				MatchGameName = board.MatchGame.Name,
			};

			await _cacheDataService.SetAsync(key, cachedBoard);
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

		public async Task RemoveBoardFromCacheAsync(Guid boardUid, CancellationToken cancellationToken)
		{
			await _cacheDataService.RemoveItemFromCacheAsync(GetBoardKeyName(boardUid));
			await _cacheDataService.RemoveItemFromCacheAsync(GetTileKeysName(boardUid));
		}
	}

	public static class ListExtensions
	{
		private static Random rng = new Random();
		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}
