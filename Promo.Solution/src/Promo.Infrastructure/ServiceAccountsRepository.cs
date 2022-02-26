using MySqlConnector;
using Promo.Core.Extensions;
using Promo.Core.MessageContracts;
using System.Data;
using System.Threading.Tasks;

namespace Promo.Infrastructure
{
	public interface IServiceAccountsRepository
	{
		Task CreateAsync(IAccountCreated accountCreated, string connectionString);
	}

	public class ServiceAccountsRepository : IServiceAccountsRepository
	{
		public async Task CreateAsync(IAccountCreated accountCreated, string connectionString)
		{
			using var conn = new MySqlConnection(connectionString);
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
