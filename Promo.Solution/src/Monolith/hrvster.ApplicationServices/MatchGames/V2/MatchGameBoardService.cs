using hrvster.ApplicationServices.Caching;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.MongoEntities.MatchGames;
using hrvster.Infrastructure.MongoRepositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
		private readonly IMongoMatchTileRepository _mongoMatchTileRepository;

		private const string TileKeysName = "tiles: ";
		private static ThreadLocal<Random> localRandom = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
		private static readonly object randomLock = new object();

		private ConcurrentDictionary<int, MatchTile> tileCache = new ConcurrentDictionary<int, MatchTile>();

		public MatchGameBoardService(
			ILoggerFactory loggerFactory,
			IMatchBoardsRepository matchBoardsRepository,
			IMatchPrizeService matchPrizeService,
			IMatchImageService matchImageService,
			IMongoMatchTileRepository mongoMatchTileRepository,
			ICacheDataService cacheDataService)
		{
			_logger = loggerFactory.CreateLogger<MatchGameBoardService>();
			_matchBoardsRepository = matchBoardsRepository;
			_matchPrizeService = matchPrizeService;
			_matchImageService = matchImageService;
			_mongoMatchTileRepository = mongoMatchTileRepository;
			_cacheDataService = cacheDataService;
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

			_logger.LogInformation("Saving tiles into the file.");

			var batchSize = 100000;  // Number of tiles per batch
			var numberOfBatches = (int)Math.Ceiling((double)allTiles.Count / batchSize);

			if(boardList.Count > 500000)
			{
				//Serialize and write to files
				for (var i = 0; i < numberOfBatches; i++)
				{
					using var file = File.CreateText($"C:\\hrvster\\tiles\\tiles_batch_{i + 1}_{gameUid}.json");
					var serializer = new JsonSerializer();

					var tilesToSave = allTiles.Select(tile => new CachedTile
					{
						BoardUid = tile.Board.Uid,
						TileUid = tile.Uid,
						ImageId = tile.Image.Id,
						ImageUid = tile.Image.Uid,
						ImageUrl = tile.Image.LocationUrl,
						PrizeId = tile.WinIndicator ? tile.Prize.Id : 0,
						PrizeUid = tile.WinIndicator ? tile.Prize.Uid : Guid.Empty,
						WinIndicator = tile.WinIndicator
					}).Skip(i * batchSize).Take(batchSize);

					serializer.Serialize(file, tilesToSave);
				}

				_logger.LogInformation("The tiles for the game with uid {GameUid} has been written to a file.", gameUid);
			}
			else
			{
				//Save into the database
				for (var i = 0; i < numberOfBatches; i++)
				{
					var tilesToSave = allTiles.Select(tile => new MatchTileEntity
					{
						Board = new MatchBoard { Id = tile.Id, Uid = tile.Uid },
						WinIndicator = tile.WinIndicator,
						Prize = tile.WinIndicator ? new MatchPrize { Id = tile.Prize.Id, Uid = tile.Prize.Uid } : null,
						Image = tile.Image,
						Random = localRandom.Value.NextDouble(),
						Uid = tile.Uid
					}).Skip(i * batchSize).Take(batchSize);

					await _mongoMatchTileRepository.SaveTilesAsync(tilesToSave);
				}

				_logger.LogInformation("The tiles for the game with uid {GameUid} has been saved to the database.", gameUid);
			}

			_logger.LogInformation("Saving tiles into the cache.");

			var options = new ParallelOptions { MaxDegreeOfParallelism = 4 }; // Reduced parallelism

			await Parallel.ForEachAsync(boardList, options, async (board, cancellationToken) =>
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

				await _cacheDataService.SetAsync(tileKey, cachedTiles);
			});


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
