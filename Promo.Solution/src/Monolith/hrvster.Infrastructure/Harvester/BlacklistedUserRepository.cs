using hrvster.Infrastructure.Extensions;
using hrvster.Models.Harvester;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Harvester
{
	public interface IBlacklistedUserRepository
	{
		Task CreateAsync(string externalUserId, Guid gameUid);
		Task<IEnumerable<BlacklistedUser>> GetBlacklistedUsersAsync(Guid gameUid);
	}

	public class BlacklistedUserRepository : IBlacklistedUserRepository
	{
		private readonly IDatabase _database;

		public BlacklistedUserRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(string externalUserId, Guid gameUid)
		{
			using var conn = new MySqlConnection(_database.GetHarvesterConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "blacklisted_user_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("externalUserId", externalUserId)
				.AddParameter("gameUid", gameUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<IEnumerable<BlacklistedUser>> GetBlacklistedUsersAsync(Guid gameUid)
		{
			var blacklistedUserList = new List<BlacklistedUser>();

			using (var conn = new MySqlConnection(_database.GetHarvesterConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "blacklisted_users_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(gameUid), gameUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						blacklistedUserList.Add(CreateRecordFrom(reader));
					}
				}
			}

			return blacklistedUserList;
		}

		private BlacklistedUser CreateRecordFrom(MySqlDataReader reader)
		{
			return new BlacklistedUser
			{
				GameUid = reader.GetGuid("game_uid"),
				UserId = reader.GetString("external_user_id")
			};
		}
	}
}
