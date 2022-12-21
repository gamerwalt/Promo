using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface IAccountsRepository
	{
		Task CreateAsync(Account account);
		Task UpdateAsync(Guid accountUid, Account account);
		Task<Account> GetAsync(Guid accountUid);
		Task<Account> GetByEmailAddressAsync(string emailAddress);
		Task<Account> GetByPhoneNumberAsync(string phoneNumber);
		Task<Account> GetByDisplayNameAsync(string displayName);
		Task RemoveAsync(Account account);
		Task<List<Account>> GetAllAccounts();
		Task<IEnumerable<Account>> GetUnverifiedAccountsAsync(CancellationToken token);
		Task VerifyAccountEmailAsync(Guid accountUid);
		Account GetUserByEmail(string emailAddress);
		Account GetUserByUserName(string userName);
		Account GetByPhoneNumber(string phoneNumber);
		Task UpdateAsync(Guid accountUid, string firstName, string lastName, string displayName);
		Task UpdateAsync(Guid accountUid, string emailAddress, string phoneNumber);
		Task UpdateLastLoginAsync(Guid accountUid);
	}

	public class AccountsRepository : IAccountsRepository
	{
		private readonly IDatabase _database;
		public AccountsRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(Account account)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("email", account.EmailAddress)
				.AddParameter("emailVerified", account.EmailVerified)
				.AddParameter("phone", account.PhoneNumber)
				.AddParameter("phoneVerified", account.PhoneNumberVerified)
				.AddParameter("accountUid", account.AccountUid)
				.AddParameter("displayName", account.DisplayName)
				.AddParameter("pin", account.Pin)
				.AddParameter("termsAccepted", account.TermsAccepted)
				.AddParameter("verificationLink", account.VerificationLink);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<Account> GetAsync(Guid accountUid)
		{
			var account = default(Account);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", accountUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						account = CreateAccountFrom(reader);
					}
				}
			}

			return account;
		}

		public async Task<List<Account>> GetAllAccounts()
		{
			var accounts = new List<Account>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "accounts_get";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						accounts.Add(CreateAccountFrom(reader));
					}
				}
			}

			return accounts;
		}

		private Account CreateAccountFrom(MySqlDataReader reader)
		{
			return new Account
			{
				AccountId = reader.GetInt32("account_id"),
				AccountUid = reader.GetGuid("account_uid"),
				EmailAddress = reader.GetString("email_address"),
				PhoneNumber = reader.GetStringValue("phone_number"),
				EmailVerified = reader.GetBoolean("email_address_verified"),
				PhoneNumberVerified = reader.GetBoolean("phone_number_verified"),
				FirstName = reader.GetStringValue("first_name"),
				LastName = reader.GetStringValue("last_name"),
				DisplayName = reader.GetStringValue("display_name"),
				Pin = reader.GetStringValue("pin"),
				TermsAccepted = reader.GetBoolean("terms_accepted"),
				TwitterUserName = reader.GetStringValue("twitter_user_name"),
				CreatedDate = reader.GetDateTime("created_date"),
				UpdatedDate = GetNullableDateTimeValue(reader, "updated_date")
			};
		}

		private string GetNullableString(MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			return reader.GetString(fieldName);
		}

		private DateTime? GetNullableDateTimeValue(MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			return reader.GetDateTime(fieldName);
		}

		public Task RemoveAsync(Account account)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Guid userUid, Account account)
		{
			throw new NotImplementedException();
		}

		public async Task<Account> GetByEmailAddressAsync(string emailAddress)
		{
			var account = default(Account);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_get_by_email";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("email", emailAddress);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						account = CreateAccountFrom(reader);
					}
				}
			}

			return account;
		}

		public async Task<Account> GetByPhoneNumberAsync(string phoneNumber)
		{
			var account = default(Account);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_get_by_phone";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("phone", phoneNumber);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						account = CreateAccountFrom(reader);
					}
				}
			}

			return account;
		}

		public async Task<Account> GetByDisplayNameAsync(string displayName)
		{
			var account = default(Account);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_get_by_display_name";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("displayName", displayName);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						account = CreateAccountFrom(reader);
					}
				}
			}

			return account;
		}

		public async Task<IEnumerable<Account>> GetUnverifiedAccountsAsync(CancellationToken token)
		{
			var accounts = new List<Account>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "accounts_unverified_get";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						accounts.Add(CreateAccountFrom(reader));
					}
				}
			}

			return accounts;
		}

		public async Task VerifyAccountEmailAsync(Guid accountUid)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_email_verify";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("accountUid", accountUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public Account GetUserByEmail(string emailAddress)
		{
			var account = default(Account);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_get_by_email";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("email", emailAddress);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					account = CreateAccountFrom(reader);
				}
			}

			return account;
		}

		public Account GetUserByUserName(string userName)
		{
			var account = default(Account);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_get_by_displayname";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("displayName", userName);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					account = CreateAccountFrom(reader);
				}
			}

			return account;
		}

		public Account GetByPhoneNumber(string phoneNumber)
		{
			var account = default(Account);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_get_by_phonenumber";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("phoneNumber", phoneNumber);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					account = CreateAccountFrom(reader);
				}
			}

			return account;
		}

		public async Task UpdateAsync(Guid accountUid, string firstName, string lastName, string displayName)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_update";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("accountUid", accountUid)
				.AddParameter("firstName", firstName)
				.AddParameter("lastName", lastName)
				.AddParameter("displayName", displayName);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task UpdateAsync(Guid accountUid, string emailAddress, string phoneNumber)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_update_email_phone";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("accountUid", accountUid)
				.AddParameter("emailAddress", emailAddress)
				.AddParameter("phoneNumber", phoneNumber);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task UpdateLastLoginAsync(Guid accountUid)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_update_last_login";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("lastLoginDate", DateTime.Now)
				.AddParameter("lastLoginDateUTC", DateTime.UtcNow)
				.AddParameter("accountUid", accountUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}
