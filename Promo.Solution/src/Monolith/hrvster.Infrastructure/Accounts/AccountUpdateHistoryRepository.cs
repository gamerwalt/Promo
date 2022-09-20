using hrvster.Infrastructure.Extensions;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Accounts
{
	public interface IAccountUpdateHistoryRepository
	{
		Task CreateRecordAsync(Guid uid, int accountId, string oldValue, string newValue);
	}

	public class AccountUpdateHistoryRepository : IAccountUpdateHistoryRepository
	{
		private readonly IDatabase _database;

		public AccountUpdateHistoryRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateRecordAsync(Guid uid, int accountId, string oldValue, string newValue)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_update_history_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("uid", uid)
				.AddParameter("accountId", accountId)
				.AddParameter("oldValue", oldValue)
				.AddParameter("newValue", newValue);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}
