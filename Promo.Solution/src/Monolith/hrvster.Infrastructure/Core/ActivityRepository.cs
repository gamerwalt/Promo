using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Core
{
	public interface IActivityRepository
	{
		Task AddActivityAsync(Guid activityUid, Guid accountUid, Guid linkedUid, ActivityType activityType, string baseTableName);
	}

	public class ActivityRepository : IActivityRepository
	{
		private readonly IDatabase _database;
		public ActivityRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task AddActivityAsync(
			Guid activityUid,
			Guid accountUid,
			Guid linkedUid,
			ActivityType activityType,
			string baseTableName)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "activity_add";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("activityUid", activityUid)
				.AddParameter("accountUid", accountUid)
				.AddParameter("linkedUid", linkedUid)
				.AddParameter("activityType", activityType.ToString())
				.AddParameter("activityTableBaseName", baseTableName)
				.AddParameter("createdDate", DateTime.Now)
				.AddParameter("createdDateUtc", DateTime.UtcNow);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}
