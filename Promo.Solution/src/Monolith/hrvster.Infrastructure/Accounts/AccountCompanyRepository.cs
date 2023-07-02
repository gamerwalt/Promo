using hrvster.Infrastructure.Extensions;
using hrvster.Models.Company;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Accounts
{
	public interface IAccountCompanyRepository
	{
		Task CreateAsync(AccountCompany accountCompany, CancellationToken cancellationToken);
		bool CompanyNameExists(string companyName);
		bool SubdomainNameExists(string subdomainName);
		Task<IEnumerable<AccountCompany>> GetAccountCompaniesAsync(Guid accountUid);
		Task<AccountCompany> GetAccountCompanyAsync(string subdomain);
	}

	public class AccountCompanyRepository : IAccountCompanyRepository
	{
		private readonly IDatabase _database;

		public AccountCompanyRepository(IDatabase database)
		{
			_database = database;
		}

		public bool CompanyNameExists(string companyName)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_company_get_by_company_name";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("companyName", companyName);

			conn.Open();
			using var reader = command.ExecuteReader();

			return reader.HasRows;
		}

		public async Task CreateAsync(AccountCompany accountCompany, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_company_create";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("companyUid", accountCompany.Uid)
				.AddParameter("accountUid", accountCompany.Account.AccountUid)
				.AddParameter("companyName", accountCompany.Name)
				.AddParameter("subdomain", accountCompany.Subdomain);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<IEnumerable<AccountCompany>> GetAccountCompaniesAsync(Guid accountUid)
		{
			var companyList = new List<AccountCompany>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_company_get_by_account_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", accountUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					companyList.Add(CreateRecordFrom(reader));
				}
			}

			return companyList;
		}

		private AccountCompany CreateRecordFrom(MySqlDataReader reader)
		{
			return new AccountCompany
			{
				Account = new Models.Account {  AccountUid = reader.GetGuid("account_uid")},
				Name = reader.GetStringValue("company_name"),
				Subdomain = reader.GetStringValue("subdomain"),
				Id = reader.GetInt32("company_id"),
				Uid = reader.GetGuid("company_uid"),
				IsActive = reader.GetBoolean("is_active")
			};
		}

		public bool SubdomainNameExists(string subdomainName)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_company_get_by_subdomain_name";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("subdomainName", subdomainName);

			conn.Open();
			using var reader = command.ExecuteReader();

			return reader.HasRows;
		}

		public async Task<AccountCompany> GetAccountCompanyAsync(string subdomain)
		{
			var accountCompany = default(AccountCompany);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_company_get_by_subdomain_name";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("subdomainName", subdomain);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					accountCompany = CreateRecordFrom(reader);
				}
			}

			return accountCompany;
		}
	}
}
