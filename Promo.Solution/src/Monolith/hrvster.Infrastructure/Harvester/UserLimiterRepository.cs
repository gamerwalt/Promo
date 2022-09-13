using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Harvester
{
	public interface IUserLimiterRepository
	{
		Task CreateAsync(Guid userLimiterUid, string externalUserId, UserIdentifierType userIdentifierType, string watchValue);
		Task<UserLimiter> GetByUserLimiterUidAsync(Guid userLimiterUid);
		Task<UserLimiter> GetByExernalUserId(string externalUserId, UserIdentifierType userIdentifierType);
		Task<IEnumerable<UserLimiter>> GetByWatchValue(string watchValue, UserIdentifierType userIdentifierType);
		Task UpdatedAsync(UserLimiter userLimiter);
	}

	public class UserLimiterRepository : IUserLimiterRepository
	{
		private readonly IDatabase _database;

		public UserLimiterRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(
			Guid userLimiterUid,
			string externalUserId,
			UserIdentifierType userIdentifierType,
			string watchValue)
		{
			using var conn = new MySqlConnection(_database.GetHarvesterConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "user_limiter_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter(nameof(userLimiterUid), userLimiterUid)
				.AddParameter(nameof(externalUserId), externalUserId)
				.AddParameter(nameof(userIdentifierType), userIdentifierType.ToString())
				.AddParameter(nameof(watchValue), watchValue);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<UserLimiter> GetByExernalUserId(string externalUserId, UserIdentifierType userIdentifierType)
		{
			var userLimiter = default(UserLimiter);

			using (var conn = new MySqlConnection(_database.GetHarvesterConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "user_limiter_get_by_external_user_id";
				command.CommandType = CommandType.StoredProcedure;
				command
					.AddParameter(nameof(externalUserId), externalUserId)
					.AddParameter(nameof(userIdentifierType), userIdentifierType.ToString());

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					userLimiter = CreateRecordFrom(reader);
				}
			}

			return userLimiter;
		}

		private UserLimiter CreateRecordFrom(MySqlDataReader reader)
		{
			return new UserLimiter
			{
				UserLimiterId = reader.GetInt32("user_limiter_id"),
				UserLimiterUid = reader.GetGuid("user_limiter_uid"),
				ExternalUserId = reader.GetString("external_user_id"),
				UserIdentifierType = Enum.Parse<UserIdentifierType>(reader.GetString("user_identifier_type")),
				WatchValue = reader.GetString("watch_value"),
				CreatedDate = reader.GetDateTime("created_date"),
				UpdatedDate = reader.GetNullableDateTimeValue("updated_date")
			};
		}

		public async Task<UserLimiter> GetByUserLimiterUidAsync(Guid userLimiterUid)
		{
			var userLimiter = default(UserLimiter);

			using (var conn = new MySqlConnection(_database.GetHarvesterConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "user_limiter_get_by_user_limiter_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(userLimiterUid), userLimiterUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					userLimiter = CreateRecordFrom(reader);
				}
			}

			return userLimiter;
		}

		public async Task<IEnumerable<UserLimiter>> GetByWatchValue(string watchValue, UserIdentifierType userIdentifierType)
		{
			var userLimiter = new List<UserLimiter>();

			using (var conn = new MySqlConnection(_database.GetHarvesterConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "user_limiters_get_by_watch_value";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(watchValue), watchValue)
					.AddParameter(nameof(userIdentifierType), userIdentifierType);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					userLimiter.Add(CreateRecordFrom(reader));
				}
			}

			return userLimiter;
		}

		public async Task UpdatedAsync(UserLimiter userLimiter)
		{
			using var conn = new MySqlConnection(_database.GetHarvesterConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "user_limiter_update";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter(nameof(userLimiter.UserLimiterUid), userLimiter.UserLimiterUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}
