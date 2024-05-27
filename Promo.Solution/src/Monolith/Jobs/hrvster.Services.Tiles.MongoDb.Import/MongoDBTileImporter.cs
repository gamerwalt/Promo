using hrvster.ApplicationServices.JobContracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace hrvster.Services.Tiles.MongoDb.Import
{
	public class MongoDBTileImporter : IService
	{
		private readonly ILogger<MongoDBTileImporter> _logger;
		private readonly IMongoCollection<BsonDocument> _collection;
		private readonly string _directory;
		private readonly SemaphoreSlim _semaphore;
		private readonly IConfiguration _config;
		private readonly string _connectionString;
		private readonly string _databaseName;
		private readonly string _collectionName;

		public MongoDBTileImporter(
			ILoggerFactory loggerFactory,
			string connectionString,
			string databaseName,
			string collectionName,
			string directory,
			IConfiguration config)
		{
			_connectionString = connectionString;
			_databaseName = databaseName;
			_collectionName = collectionName;

			_logger = loggerFactory.CreateLogger<MongoDBTileImporter>();
			var client = new MongoClient(connectionString);
			var database = client.GetDatabase(databaseName);
			_collection = database.GetCollection<BsonDocument>(collectionName);
			_directory = directory;
			_semaphore = new SemaphoreSlim(20); // Limit to 20 concurrent tasks
			_config = config;
		}

		public async Task ProcessService(CancellationToken token)
		{
			var files = Directory.GetFiles(_directory, "*.json");
			var fileBatchSize = int.Parse(_config["FileBatchSize"]); // Number of files to load at a time
			var taskBatchSize = int.Parse(_config["TaskBatchSize"]); // Number of tasks to process concurrently

			_logger.LogInformation("Total files to process: {Count}", files.Length);

			// Configure MongoDB client settings
			var settings = MongoClientSettings.FromUrl(new MongoUrl(_connectionString));
			settings.ConnectTimeout = TimeSpan.FromSeconds(30);
			settings.SocketTimeout = TimeSpan.FromSeconds(30);
			var client = new MongoClient(settings);
			var database = client.GetDatabase(_databaseName);
			var collection = database.GetCollection<BsonDocument>(_collectionName);

			for (int i = 0; i < files.Length; i += fileBatchSize)
			{
				var fileBatch = files.Skip(i).Take(fileBatchSize).ToArray();
				_logger.LogInformation("Processing batch of {Count} files starting from index {Index}", fileBatch.Length, i);

				var taskList = new List<Task>();

				foreach (var file in fileBatch)
				{
					taskList.Add(Task.Run(async () =>
					{
						await _semaphore.WaitAsync(token);
						try
						{
							_logger.LogInformation("Importing the file {FileName}", file);

							var json = await File.ReadAllTextAsync(file);
							var documents = BsonSerializer.Deserialize<List<BsonDocument>>(json);

							await RetryPolicy.ExecuteAsync(async () =>
							{
								await collection.InsertManyAsync(documents, new InsertManyOptions { IsOrdered = false }, token);
							});

							_logger.LogInformation("Finished importing the file {FileName}", file);

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
					}));

					if (taskList.Count >= taskBatchSize)
					{
						await Task.WhenAll(taskList);
						taskList.Clear();
					}
				}

				// Await any remaining tasks
				if (taskList.Any())
				{
					await Task.WhenAll(taskList);
				}
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

	public static class RetryPolicy
	{
		private const int MaxRetryAttempts = 3;
		private static readonly TimeSpan PauseBetweenFailures = TimeSpan.FromSeconds(5);

		public static async Task ExecuteAsync(Func<Task> action)
		{
			for (int attempt = 1; attempt <= MaxRetryAttempts; attempt++)
			{
				try
				{
					await action();
					return;
				}
				catch (MongoException ex) when (IsTransientError(ex) && attempt < MaxRetryAttempts)
				{
					await Task.Delay(PauseBetweenFailures);
				}
			}
		}

		private static bool IsTransientError(MongoException ex)
		{
			return ex.HasErrorLabel("TransientTransactionError") || ex.HasErrorLabel("UnknownTransactionCommitResult");
		}
	}
}
