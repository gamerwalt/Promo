using hrvster.Games.Match.Models;
using hrvster.Models.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Caching
{
	public interface ICacheDataService
	{
		Task SetAsync<T>(string key, T value, CancellationToken token = default, TimeSpan? expiresIn = null);
		Task<T> GetAsync<T>(string key);
		Task RemoveItemFromCacheAsync(string key);
		Task SetAddAsync(string boardKeysKeyName, string value);
		Task<string> GetRandomBoardKey(string boardKeysKeyName);
		Task<string> GetBoardDetailsAsync(string key);
		Task<string> GetTileDetailsAsync(string key);
		IDatabase GetDatabase();
	}

	public class RedisCacheService : ICacheDataService
	{
		private readonly HarvesterSettingsOptions _hrvsterSettings;
		private readonly ILogger<RedisCacheService> _logger;

		private readonly ConnectionMultiplexer _redis;
		private readonly IDatabase _cache;


		public RedisCacheService(ILoggerFactory loggerFactory, HarvesterSettingsOptions hrvsterSettings)
		{
			_logger = loggerFactory.CreateLogger<RedisCacheService>();
			_hrvsterSettings = hrvsterSettings;

			var connectionStringForRedis = _hrvsterSettings.RedisCache;
			_logger.LogInformation($"Connecting to {connectionStringForRedis}");

			try
			{
				_redis = ConnectionMultiplexer.Connect(_hrvsterSettings.RedisCache, c => CreateConfigurationOptions());

				_cache = _redis.GetDatabase();
			}
			catch (Exception ex)
			{
				_logger.LogError("Unable to connect to the redis server. Error message: {message}", ex.Message);
			}
		}

		private static ConfigurationOptions CreateConfigurationOptions()
		{
			var deltaBackOffInMilliseconds = TimeSpan.FromSeconds(5).Milliseconds;
			var maxDeltaBackOffInMilliseconds = TimeSpan.FromSeconds(20).Milliseconds;

			var config = new ConfigurationOptions();

			//https://learn.microsoft.com/en-us/azure/architecture/best-practices/retry-service-specific#azure-redis-cache-retry-guidelines
			config.AbortOnConnectFail = false;
			config.ConnectRetry = 3;
			config.ConnectTimeout = 2000;
			config.ReconnectRetryPolicy = new ExponentialRetry(deltaBackOffInMilliseconds, maxDeltaBackOffInMilliseconds);

			return config;
		}

		public async Task SetAsync<T>(string key, T value, CancellationToken token = default, TimeSpan? expiresIn = null)
		{
			if(token.IsCancellationRequested)
			{
				token.ThrowIfCancellationRequested();
			}

			var settings = new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			};

			var json = JsonConvert.SerializeObject(value, settings);

			await _cache.StringSetAsync(key, json, expiresIn);
		}

		public async Task<T> GetAsync<T>(string key)
		{
			var json = await _cache.StringGetAsync(key);

			if (json.IsNullOrEmpty)
			{
				return default(T);
			}

			return JsonConvert.DeserializeObject<T>(json);
		}

		public async Task RemoveItemFromCacheAsync(string key)
			=> await _cache.KeyDeleteAsync(key);

		public Task SetAddAsync(string boardKeysKeyName, string value)
		{
			return _cache.SetAddAsync(boardKeysKeyName, value);
		}

		public async Task<string> GetRandomBoardKey(string boardKeysKeyName)
		{
			var value = await _cache.SetPopAsync(boardKeysKeyName);

			if(value.HasValue)
			{
				return value.ToString();
			}

			return string.Empty;
		}

		public async Task<string> GetBoardDetailsAsync(string key)
		{
			var value = await _cache.StringGetAsync(key);

			if(value.HasValue)
			{
				return value.ToString();
			}

			return string.Empty;
		}

		public async Task<string> GetTileDetailsAsync(string key)
		{
			var value = await _cache.StringGetAsync(key);

			if (value.HasValue)
			{
				return value.ToString();
			}

			return string.Empty;
		}

		public IDatabase GetDatabase()
			=> _cache;
	}
}
