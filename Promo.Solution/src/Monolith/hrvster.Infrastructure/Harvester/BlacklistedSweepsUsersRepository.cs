using hrvster.Models.Harvester;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Harvester
{
	public interface IBlacklistedSweepsUsersRepository
	{
		Task<IEnumerable<BlacklistedSweepUser>> GetBlacklistedSweepUsersAsync(Guid sweepUid);
	}

	public class BlacklistedSweepsUsersRepository : IBlacklistedSweepsUsersRepository
	{
		private readonly IDatabase _database;

		public BlacklistedSweepsUsersRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<IEnumerable<BlacklistedSweepUser>> GetBlacklistedSweepUsersAsync(Guid sweepUid)
		{
			var blacklistedUserList = new List<BlacklistedSweepUser>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
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
