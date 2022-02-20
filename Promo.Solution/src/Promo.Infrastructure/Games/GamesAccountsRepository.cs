using MySqlConnector;
using Promo.Core;
using Promo.Core.Extensions;
using Promo.Core.MessageContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Infrastructure.Games
{
	public interface IGamesAccountsRepository
	{
		Task CreateAsync(IAccountCreated accountCreated);
	}

    public class GamesAccountsRepository : IGamesAccountsRepository
    {
		private readonly IDatabase _database;
		public GamesAccountsRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(IAccountCreated accountCreated)
		{
			using var conn = new MySqlConnection(_database.GetGamesConnectionstring());
			var command = conn.CreateCommand();
			command.CommandText = "account_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("accountUid", accountCreated.AccountUid)
				.AddParameter("emailAddress", accountCreated.EmailAddress)
				.AddParameter("phoneNumber", accountCreated.PhoneNumber)
				.AddParameter("displayName", accountCreated.DisplayName);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}
