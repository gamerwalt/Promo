using hrvster.ApplicationServices.JobContracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace hrvster.Services.Tiles.MongoDb.Import
{
	public class MongoDBTileImporter : IService
	{
		private readonly ILogger<MongoDBTileImporter> _logger;
		private readonly IMongoCollection<BsonDocument> _collection;
		private readonly string _directory;
		private readonly SemaphoreSlim _semaphore;

		public MongoDBTileImporter(
			ILoggerFactory loggerFactory,
			string connectionString,
			string databaseName,
			string collectionName,
			string directory)
		{
			_logger = loggerFactory.CreateLogger<MongoDBTileImporter>();
			var client = new MongoClient(connectionString);
			var database = client.GetDatabase(databaseName);
			_collection = database.GetCollection<BsonDocument>(collectionName);
			_directory = directory;
			_semaphore = new SemaphoreSlim(20); // Limit to 20 concurrent tasks
		}

		public async Task ProcessService(CancellationToken token)
		{
			var files = Directory.GetFiles(_directory, "*.json");
			var fileBatches = files.Batch(1000); // Adjust batch size based on your needs and system capabilities

			_logger.LogInformation("Working on {count} files.", files.Length);

			foreach (var batch in fileBatches)
			{
				var tasks = batch.Select(async file =>
				{
					await _semaphore.WaitAsync();
					try
					{
						_logger.LogInformation("Importing the file {FileName}", file);

						var json = await File.ReadAllTextAsync(file);
						var documents = BsonSerializer.Deserialize<List<BsonDocument>>(json);
						await _collection.InsertManyAsync(documents);

						_logger.LogInformation("Finsihed importing the file {FileName}", file);

						_logger.LogInformation("Backing up the file {FileName}", file);
						var backupFilePath = Path.Combine(_directory, "tiles_backup", Path.GetFileName(file));
						File.Move(file, backupFilePath);
						_logger.LogInformation("Finished backing up the file {FileName}", file);
					}
					catch (Exception ex)
					{
						_logger.LogWarning("An error occurred while processing {FileName}: {Message}", file, ex.Message);
					}
					finally
					{
						_semaphore.Release();
					}
				});

				await Task.WhenAll(tasks);
			}

			_logger.LogInformation("Import and backup completed for all files.");
		}
	}

	public static class BatchExtensions
	{
		public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int maxItems)
		{
			return items.Select((item, index) => new { item, index })
						.GroupBy(x => x.index / maxItems)
						.Select(g => g.Select(x => x.item));
		}
	}
}
