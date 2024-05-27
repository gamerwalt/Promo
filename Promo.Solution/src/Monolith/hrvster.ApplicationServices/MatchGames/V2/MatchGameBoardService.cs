using hrvster.ApplicationServices.Caching;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.MatchGames.V2
{
	public interface IMatchGameBoardService
	{
		Task GenerateTilesForBoardAsync(MatchGame matchGame, CancellationToken token);
	}

	public class MatchGameBoardService : IMatchGameBoardService
	{
		private readonly ILogger<MatchGameBoardService> _logger;
		private readonly IMatchBoardsRepository _matchBoardsRepository;
		private readonly IMatchPrizeService _matchPrizeService;
		private readonly IMatchImageService _matchImageService;
		private readonly ICacheDataService _cacheDataService;
		private readonly IConfiguration _config;

		private const string TileKeysName = "tiles: ";
		private static ThreadLocal<Random> localRandom = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
		private static readonly object randomLock = new object();

		private ConcurrentDictionary<int, MatchTile> tileCache = new ConcurrentDictionary<int, MatchTile>();

		public MatchGameBoardService(
			ILoggerFactory loggerFactory,
			IMatchBoardsRepository matchBoardsRepository,
			IMatchPrizeService matchPrizeService,
			IMatchImageService matchImageService,
			ICacheDataService cacheDataService,
			IConfiguration config)
		{
			_logger = loggerFactory.CreateLogger<MatchGameBoardService>();
			_matchBoardsRepository = matchBoardsRepository;
			_matchPrizeService = matchPrizeService;
			_matchImageService = matchImageService;
			_cacheDataService = cacheDataService;
			_config = config;
		}

		public async Task GenerateTilesForBoardAsync(MatchGame matchGame, CancellationToken token)
		{
			if (token.IsCancellationRequested)
			{
				token.ThrowIfCancellationRequested();
			}

			var boardsTask = _matchBoardsRepository.GetForMatchGameAsync(matchGame.Uid, token);
			var prizesTask = _matchPrizeService.GetAllForMatchGameAsync(matchGame.Uid, token);
			var imagesTask = _matchImageService.GetForMatchGameAsync(matchGame.Uid, token);

			await Task.WhenAll(boardsTask, prizesTask, imagesTask);

			var boards = boardsTask.Result;
			var prizes = prizesTask.Result;
			var images = imagesTask.Result;

			if (boards == null || prizes == null)
			{
				_logger.LogWarning("There are no boards, prizes or images for this game {GameUid} - {GameName}.", matchGame.Uid, matchGame.Name);

				return;
			}

			var boardList = boards.ToList();
			var prizeList = prizes.ToList();
			var imagesList = images.ToList();

			if (boardList.Count == 0 || prizeList.Count == 0 || imagesList.Count == 0)
			{
				_logger.LogWarning("There are no boards, prizes or images for this game {GameUid} - {GameName}.", matchGame.Uid, matchGame.Name);

				return;
			}

			_logger.LogInformation("Starting tile generation for the game {GameName}", matchGame.Name);

			var lossPrize = prizeList.Find(c => c.IsFiller);
			if (lossPrize != null)
			{
				prizeList.Remove(lossPrize);
			}

			foreach (var prize in prizeList)
			{
				var prototypeTile = new MatchTile
				{
					Uid = Guid.NewGuid(),
					Board = new MatchBoard { Id = 0, Uid = Guid.Empty },
					WinIndicator = false,
					Prize = prize,
					Image = prize.Image,
					Random = localRandom.Value.NextDouble()
				};

				tileCache.TryAdd(prize.Id, prototypeTile);
			}

			await GenerateBoardTilesAsync(matchGame, boardList, prizeList, token);
		}

		private async Task GenerateBoardTilesAsync(
			MatchGame game,
			List<MatchBoard> boardList,
			List<MatchPrize> prizes,
			CancellationToken token)
		{

			if (token.IsCancellationRequested)
			{
				token.ThrowIfCancellationRequested();
			}

			var options = new ParallelOptions
			{
				CancellationToken = token,
				MaxDegreeOfParallelism = Environment.ProcessorCount
			};

			var allTiles = new List<MatchTile>(boardList.Count * 9);
			object lockObject = new object();

			_logger.LogInformation("Start generating tiles for the boards.");

			await Parallel.ForEachAsync(boardList, options, async (board, token) =>
			{
				var tiles = GenerateTiles(game, board, prizes, token);

				lock (lockObject)
				{
					allTiles.AddRange(tiles);
					if (allTiles.Count % 500000 == 0)
					{
						_logger.LogInformation("Total tiles generated is {TileCount}", allTiles.Count);
					}
				}
			});

			allTiles.Shuffle();

			_logger.LogInformation("Finished Generating tiles.");

			await SaveTilesAsync(game.Uid, boardList, allTiles, token);
		}

		private async Task SaveTilesAsync(
			Guid gameUid,
			List<MatchBoard> boardList,
			List<MatchTile> allTiles,
			CancellationToken token)
		{
			if (token.IsCancellationRequested)
			{
				token.ThrowIfCancellationRequested();
			}

			_logger.LogInformation("Saving the tiles into the database.");

			await _matchBoardsRepository.SaveAllTilesAsync(boardList, token);

			_logger.LogInformation("The tiles for the game with uid {GameUid} has been saved to the database.", gameUid);
			#region oldCode
			//int.TryParse(_config["TileSaveMongoDBBatchSize"], out var batchSize);
			//var numberOfBatches = (int)Math.Ceiling((double)allTiles.Count / batchSize);

			//if (boardList.Count > 100000)
			//{
			//	_logger.LogInformation("Saving tiles into the file.");
			//	int.TryParse(_config["BoardsPerFile"], out var boardsPerFile);
			//	var batchCounter = 0;
			//	var totalBatches = (boardList.Count + boardsPerFile - 1) / boardsPerFile; // Calculate total number of batches

			//	for (var i = 0; i < boardList.Count; i += boardsPerFile)
			//	{
			//		var batchBoards = boardList.Skip(i).Take(boardsPerFile).ToList();
			//		var tilesToSave = batchBoards.SelectMany(board => board.Tiles.Select(tile => new CachedTile
			//		{
			//			BoardUid = tile.Board.Uid,
			//			TileUid = tile.Uid,
			//			ImageId = tile.Image.Id,
			//			ImageUid = tile.Image.Uid,
			//			ImageUrl = tile.Image.LocationUrl,
			//			PrizeId = tile.WinIndicator ? tile.Prize.Id : 0,
			//			PrizeUid = tile.WinIndicator ? tile.Prize.Uid : Guid.Empty,
			//			WinIndicator = tile.WinIndicator
			//		})).ToList();

			//		var fileName = $"C:\\hrvster\\tiles\\tiles_batch_{++batchCounter}_{gameUid}.json";

			//		using (var file = File.CreateText(fileName))
			//		{
			//			var serializer = new JsonSerializer();
			//			serializer.Serialize(file, tilesToSave);
			//		}

			//		_logger.LogInformation("Batch {BatchCounter} of {TotalBatches} written to file {FileName}", batchCounter, totalBatches, fileName);
			//	}

			//	_logger.LogInformation("All tiles for the game with uid {GameUid} have been written to files.", gameUid);
			//}
			//else
			//{
			//_logger.LogInformation("Saving tiles into the Database directly.");
			////Save into the database
			//var batchTasks = new List<Task>();
			//int.TryParse(_config["TotalBatchesToSave"], out var counterToAwait);
			//if(counterToAwait == 0)
			//{
			//	counterToAwait = 500;
			//}

			//for (var i = 0; i < numberOfBatches; i++)
			//{
			//	var tilesToSave = allTiles.Select(tile => new MatchTileEntity
			//	{
			//		Board = new MatchBoard { Id = tile.Id, Uid = tile.Uid },
			//		WinIndicator = tile.WinIndicator,
			//		Prize = tile.WinIndicator ? new MatchPrize { Id = tile.Prize.Id, Uid = tile.Prize.Uid } : null,
			//		Image = tile.Image,
			//		Random = localRandom.Value.NextDouble(),
			//		Uid = tile.Uid
			//	}).Skip(i * batchSize).Take(batchSize);

			//	batchTasks.Add(_mongoMatchTileRepository.SaveTilesAsync(tilesToSave));
			//	counterToAwait++;

			//	if(counterToAwait == 100)
			//	{
			//		_logger.LogInformation("Awaiting 100 tasks.");
			//		await Task.WhenAll(batchTasks);
			//		batchTasks.Clear();
			//	}
			//}
			//}

			//await SaveTilesIntoCacheAsync(boardList);
			#endregion oldCode
		}

		private async Task SaveTilesIntoCacheAsync(List<MatchBoard> boardList)
		{
			_logger.LogInformation("Saving tiles into the cache.");

			int.TryParse(_config["TileSaveBatchSize"], out var batchSize);
			var totalBoards = boardList.Count;
			var batchTasks = new List<Task>();

			for (var i = 0; i < totalBoards; i += batchSize)
			{
				var batchBoards = boardList.Skip(i).Take(batchSize).ToList();
				_logger.LogInformation("Processing tile batch from index {StartIndex} to {EndIndex}", i, i + batchSize - 1);

				foreach (var board in batchBoards)
				{
					var tileKey = GetTileKeysName(board.Uid);
					var cachedTiles = board.Tiles.Select(tile => new CachedTile
					{
						BoardUid = tile.Board.Uid,
						TileUid = tile.Uid,
						ImageId = tile.Image.Id,
						ImageUid = tile.Image.Uid,
						ImageUrl = tile.Image.LocationUrl,
						PrizeId = tile.WinIndicator ? tile.Prize.Id : 0,
						PrizeUid = tile.WinIndicator ? tile.Prize.Uid : Guid.Empty,
						WinIndicator = tile.WinIndicator
					}).ToList();

					batchTasks.Add(_cacheDataService.SetAsync(tileKey, cachedTiles));
				}

				// Await all tasks in the current batch
				await Task.WhenAll(batchTasks);
				_logger.LogInformation("Completed processing and saving tile batch {BatchNumber} to Redis", (i / batchSize) + 1);
				batchTasks.Clear(); // Clear the list of tasks for the next batch
			}

			_logger.LogInformation("All tiles have been processed and saved into the cache.");
		}

		private IEnumerable<MatchTile> GenerateTiles(
			MatchGame game,
			MatchBoard board,
			List<MatchPrize> prizes,
			CancellationToken token)
		{
			if (token.IsCancellationRequested)
			{
				token.ThrowIfCancellationRequested();
			}

			var winProbability = game.WinProbability;
			var normalizedWinProbability = (winProbability - 1) / 5.0;

			var winningTilesCount = (int)Math.Ceiling(3 * normalizedWinProbability + 2);
			winningTilesCount = Math.Max(3, Math.Min(winningTilesCount, 9));
			var otherPrizes = prizes.ToList();
			var boardTiles = new List<MatchTile>(9);

			if (board.IsWinnable)
			{
				var winningPrize = prizes.Find(p => p.Id == board.Prize.Id);

				if (tileCache.TryGetValue(winningPrize.Id, out var prototypeTile))
				{
					for (var i = 0; i < winningTilesCount; i++)
					{
						double randomDouble;
						lock (randomLock)
						{
							randomDouble = localRandom.Value.NextDouble();
						}

						boardTiles.Add(prototypeTile.CloneWithNewProperties(Guid.NewGuid(), randomDouble, board, true));
					}
				}

				otherPrizes = prizes.Except(new List<MatchPrize> { winningPrize }).ToList();
				var otherPrizesMaxCount = (normalizedWinProbability >= 0.5) ? 2 : 1;
				var remainingTileCount = 9 - winningTilesCount;
				var tileAdded = 0;

				for (var i = 0; i < remainingTileCount; i++)
				{
					foreach (var prize in otherPrizes)
					{
						for (var x = 0; x < otherPrizesMaxCount; x++)
						{
							if (tileCache.TryGetValue(prize.Id, out var prototypeLoosingTile))
							{
								if (tileAdded >= remainingTileCount)
								{
									continue;
								}
								double randomDouble;

								lock (randomLock)
								{
									randomDouble = localRandom.Value.NextDouble();
								}

								boardTiles.Add(prototypeLoosingTile.CloneWithNewProperties(Guid.NewGuid(), randomDouble, board, false));
								tileAdded++;
							}
						}
					}
				}
			}
			else
			{
				var otherPrizesMaxCount = (normalizedWinProbability >= 0.5) ? 2 : 1;
				var remainingTileCount = 9;
				var tileAdded = 0;

				for (var i = 0; i < remainingTileCount; i++)
				{
					foreach (var prize in otherPrizes)
					{
						for (var x = 0; x < otherPrizesMaxCount; x++)
						{
							if (tileCache.TryGetValue(prize.Id, out var prototypeLoosingTile))
							{
								if (tileAdded >= remainingTileCount)
								{
									continue;
								}
								double randomDouble;

								lock (randomLock)
								{
									randomDouble = localRandom.Value.NextDouble();
								}

								boardTiles.Add(prototypeLoosingTile.CloneWithNewProperties(Guid.NewGuid(), randomDouble, board, false));
								tileAdded++;
							}
						}
					}
				}
			}

			double randomValue;
			int randomInt;

			lock (randomLock)
			{
				randomValue = localRandom.Value.NextDouble();
				randomInt = localRandom.Value.Next(otherPrizes.Count);
			}

			if (boardTiles.Count < 9)
			{
				_logger.LogInformation("Tiles are less than 9.");
				while (boardTiles.Count < 9)
				{
					var fillerPrize = otherPrizes[randomInt];
					boardTiles.Add(new MatchTile
					{
						Uid = Guid.NewGuid(),
						Board = new MatchBoard { Id = board.Id, Uid = board.Uid },
						WinIndicator = false,
						Prize = fillerPrize,
						Image = fillerPrize.Image,
						Random = randomValue
					});
				}
			}

			boardTiles.Shuffle();

			board.Tiles = boardTiles;

			return boardTiles;
		}

		#region OldTileGeneration
		//private IEnumerable<MatchTile> GenerateTilesList(
		//	MatchBoard board,
		//	MatchPrize prize,
		//	bool winIndicator,
		//	int tileCount)
		//{
		//	var tiles = new List<MatchTile>();
		//	var tileBoard = new MatchBoard { Id = board.Id, Uid = board.Uid };

		//	for (var i = 0; i < tileCount; i++)
		//	{
		//		var randomValue = localRandom.Value.NextDouble();

		//		tiles.Add(new MatchTile
		//		{
		//			Uid = Guid.NewGuid(),
		//			Board = tileBoard,
		//			WinIndicator = winIndicator,
		//			Prize = prize,
		//			Image = prize.Image,
		//			Random = randomValue
		//		});
		//	}

		//	return tiles;
		//}

		//private IEnumerable<MatchTile> GenerateLoosingTilesList(
		//	MatchBoard board,
		//	List<MatchPrize> prizes,
		//	bool winIndicator,
		//	int count,
		//	int maxPerPrize = 1)
		//{
		//	var tiles = new List<MatchTile>();

		//	var tileBoard = new MatchBoard { Id = board.Id, Uid = board.Uid };

		//	var tileAdded = 0;
		//	foreach (var prize in prizes)
		//	{
		//		for (var i = 0; i < maxPerPrize; i++)
		//		{
		//			if (tileAdded >= count)
		//			{
		//				return tiles;
		//			}

		//			var randomValue = localRandom.Value.NextDouble();

		//			tiles.Add(new MatchTile
		//			{
		//				Uid = Guid.NewGuid(),
		//				Board = tileBoard,
		//				WinIndicator = winIndicator,
		//				Prize = prize,
		//				Image = prize.Image,
		//				Random = randomValue
		//			});

		//			tileAdded++;
		//		}
		//	}

		//	return tiles;
		//}

		#endregion OldTileGeneration

		private static string GetTileKeysName(Guid boardUid)
		{
			return $"{TileKeysName}{boardUid}";
		}
	}
}
