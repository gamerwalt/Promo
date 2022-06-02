using hrvster.Models.Core;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Core
{
	public interface IWatchTypeProvidersRepository
	{
		Task<IEnumerable<WatchTypeProvider>> GetWatchTypeProviders();
	}

	public class WatchTypeProvidersRepository : IWatchTypeProvidersRepository
	{
		private readonly IDatabase _database;
		public WatchTypeProvidersRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<IEnumerable<WatchTypeProvider>> GetWatchTypeProviders()
		{
			var watchTypeProviders = new List<WatchTypeProvider>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "watch_type_providers_get";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					watchTypeProviders.Add(CreateRecordFrom(reader));
				}
			}

			return watchTypeProviders;
		}

		private WatchTypeProvider CreateRecordFrom(MySqlDataReader reader)
		{
			return new WatchTypeProvider
			{
				WatchTypeProviderId = reader.GetInt32("watch_type_provider_id"),
				WatchTypeProviderUid = Guid.Parse(reader.GetString("watch_type_provider_uid")),
				WatchTypeProviderCode = reader.GetString("provider_code"),
				WatchTypeProviderName = reader.GetString("provider_name"),
				ActiveInd = reader.GetBoolean("active_ind"),
				ImageUrl = GetStringValue(reader,"image_url")
			};
		}

		private string GetStringValue(MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}

			return reader[fieldName].ToString();
		}
	}
}
