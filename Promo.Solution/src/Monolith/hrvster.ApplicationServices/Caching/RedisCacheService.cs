using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Caching
{
	public interface ICacheDataService
	{
		Task SetAsync<T>(string key, T value, TimeSpan? expiresIn = null);
		Task<T> GetAsync<T>(string key);
		Task RemoveItemFromCacheAsync(string key);
	}

	public class RedisCacheService : ICacheDataService
	{
		private readonly IConfiguration _configuration;

		private readonly ConnectionMultiplexer _redis;
		private readonly IDatabase _cache;


		public RedisCacheService(IConfiguration configuration)
		{
			_configuration = configuration;

			_redis = ConnectionMultiplexer.Connect(configuration["RedisCache:ConnectionString"]);
			_cache = _redis.GetDatabase();
		}

		public async Task SetAsync<T>(string key, T value, TimeSpan? expiresIn = null)
		{
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
	}
}
