using hrvster.Infrastructure.MongoEntities.MatchGames;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MongoRepositories
{
	public interface IMongoMatchTileRepository
	{
		Task SaveTileAsync(MatchTileEntity tile);
		Task SaveTilesAsync(IEnumerable<MatchTileEntity> tiles);
		Task<MatchTileEntity> GetRandomTileAsync();
	}

	public class MongoMatchTileRepository : IMongoMatchTileRepository
	{
		private readonly MongoClient _client;
		private readonly IMongoDatabase _database;

		private readonly IMongoCollection<MatchTileEntity> _tiles;

		public MongoMatchTileRepository(string connectionString, string databaseName, string collectionName)
		{
			_client = new MongoClient(connectionString);
			_database = _client.GetDatabase(databaseName);

			_tiles = _database.GetCollection<MatchTileEntity>(collectionName);
		}

		public async Task SaveTileAsync(MatchTileEntity tile)
		{
			await _tiles.InsertOneAsync(tile);
		}

		public async Task SaveTilesAsync(IEnumerable<MatchTileEntity> tiles)
		{
			var bulkOps = new List<WriteModel<MatchTileEntity>>();
			foreach (var tile in tiles)
			{
				var upsertOne = new ReplaceOneModel<MatchTileEntity>(
					Builders<MatchTileEntity>.Filter.Eq(t => t.Uid, tile.Uid), tile)
				{ IsUpsert = true };
				bulkOps.Add(upsertOne);
			}
			await _tiles.BulkWriteAsync(bulkOps);
		}

		public async Task<MatchTileEntity> GetRandomTileAsync()
		{
			var randomValue = new Random().NextDouble();
			var filter = Builders<MatchTileEntity>.Filter.Gte(tile => tile.Random, randomValue);
			var tile = await _tiles.Find(filter).SortBy(t => t.Random).FirstOrDefaultAsync();
			if (tile == null) // No document is greater than the random value; loop back to smaller values
			{
				filter = Builders<MatchTileEntity>.Filter.Lte(tile => tile.Random, randomValue);
				tile = await _tiles.Find(filter).SortByDescending(t => t.Random).FirstOrDefaultAsync();
			}
			return tile;
		}
	}
}
