using hrvster.Models.Harvester;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using hrvster.Infrastructure.Extensions;

namespace hrvster.Infrastructure.Harvester
{
	public interface IBlacklistedSweepsUsersRepository
	{
		Task CreateAsync(string externalUserId, Guid sweepUid);
		Task<IEnumerable<BlacklistedSweepUser>> GetBlacklistedSweepUsersAsync(Guid sweepUid);
	}

	public class BlacklistedSweepsUsersRepository : IBlacklistedSweepsUsersRepository
	{
		private readonly IDatabase _database;

		public BlacklistedSweepsUsersRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(string externalUserId, Guid sweepUid)
		{
			using var conn = new MySqlConnection(_database.GetReportingConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "blacklisted_sweeps_user_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("externalUserId", externalUserId)
				.AddParameter("sweepUid", sweepUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<IEnumerable<BlacklistedSweepUser>> GetBlacklistedSweepUsersAsync(Guid sweepUid)
		{
			var blacklistedUserList = new List<BlacklistedSweepUser>();

			using (var conn = new MySqlConnection(_database.GetHarvesterConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "blacklisted_sweeps_users_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(sweepUid), sweepUid);

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

		private BlacklistedSweepUser CreateRecordFrom(MySqlDataReader reader)
		{
			return new BlacklistedSweepUser
			{
				SweepUid = reader.GetGuid("sweeps_uid"),
				TweetUserId = reader.GetInt64("tweet_user_id")
			};
		}
	}
}
