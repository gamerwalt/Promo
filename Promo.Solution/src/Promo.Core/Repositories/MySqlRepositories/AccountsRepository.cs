using MySqlConnector;
using Promo.Core.Extensions;
using Promo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Core.Repositories
{
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
                .AddParameter("termsAccepted", account.TermsAccepted);

            await conn.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<Account> GetAsync(Guid userUid)
        {
            var account = default(Account);

            using (var conn = new MySqlConnection(_database.GetConnectionString()))
            {
                var command = conn.CreateCommand();
                command.CommandText = "account_get_by_uid";
                command.CommandType = CommandType.StoredProcedure;
                command.AddParameter("userUid", userUid);

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
            var users = new List<Account>();

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
                        users.Add(CreateAccountFrom(reader));
                    }
                }
            }

            return users;
        }

        private Account CreateAccountFrom(MySqlDataReader reader)
        {
            return new Account
            {
                AccountId = reader.GetInt32("account_id"),
                AccountUid = reader.GetGuid("account_uid"),
                EmailAddress = reader.GetString("email_address"),
                PhoneNumber = reader.GetString("phone_number"),
                EmailVerified = reader.GetBoolean("email_address_verified"),
                PhoneNumberVerified = reader.GetBoolean("phone_number_verified"),
                DisplayName = reader.GetString("display_name"),
                Pin = reader.GetString("pin"),
                TermsAccepted = reader.GetBoolean("terms_accepted"),
                CreatedDate = reader.GetDateTime("created_date"),
                UpdatedDate = GetNullableDateTimeValue(reader, "updated_date")
            };
        }

        private DateTime? GetNullableDateTimeValue(MySqlDataReader reader, string fieldName)
        {
            var value = reader[fieldName].ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return reader.GetDateTime("updated_date");
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
                command.CommandText = "account_get_by_displayName";
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
    }
}
