using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Models.Subscriptions;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Subscriptions
{
	public interface IAccountSubscriptionsRepository
	{
		Task CreateAsync(AccountSubscription model);
		AccountSubscription GetAccountSubscription(Guid accountUid);
		Task<AccountSubscription> GetAccountSubscriptionByEmailAsync(string email);
		Task<AccountSubscription> GetAsync(Guid subscriptionUid);
		Task UpdateAccountSubscriptionStatusAsync(Guid subscriptionUid, SubscriptionStatus subscribed);
	}

	public class AccountSubscriptionsRepository : IAccountSubscriptionsRepository
	{
		private readonly IDatabase _database;
		public AccountSubscriptionsRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(AccountSubscription model)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_subscription_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("subscriptionUid", model.SubscriptionUid)
				.AddParameter("accountUid", model.Account.AccountUid)
				.AddParameter("planId", model.Plan.PlanId)
				.AddParameter("subscriptionStatus", model.SubscriptionStatus.ToString())
				.AddParameter("startDate", model.StartDate)
				.AddParameter("startDateUtc", model.StartDateUtc)
				.AddParameter("endDate", model.EndDate)
				.AddParameter("endDateUtc", model.EndDateUtc)
				.AddParameter("isDiscounted", model.IsDiscounted)
				.AddParameter("discountedBy", model.DiscountedBy);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public AccountSubscription GetAccountSubscription(Guid accountUid)
		{
			var account = default(AccountSubscription);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_subscriptions_get_by_account_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", accountUid);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					account = CreateAccountSubscriptionFrom(reader);
				}
			}

			return account;
		}

		public async Task<AccountSubscription> GetAccountSubscriptionByEmailAsync(string email)
		{
			var accountSubscription = default(AccountSubscription);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_subscription_get_by_email";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("email", email);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					accountSubscription = CreateAccountSubscriptionFrom(reader);
				}
			}

			return accountSubscription;
		}

		public async Task<AccountSubscription> GetAsync(Guid subscriptionUid)
		{
			var accountSubscription = default(AccountSubscription);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_subscription_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("subscriptionUid", subscriptionUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					accountSubscription = CreateAccountSubscriptionFrom(reader);
				}
			}

			return accountSubscription;
		}

		public async Task UpdateAccountSubscriptionStatusAsync(Guid subscriptionUid, SubscriptionStatus status)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_subscription_update_status";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("subscriptionUid", subscriptionUid)
				.AddParameter("subscriptionStatus", status.ToString());

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		private AccountSubscription CreateAccountSubscriptionFrom(MySqlDataReader reader)
		{
			return new AccountSubscription
			{
				SubscriptionId = reader.GetInt32("subscription_id"),
				SubscriptionUid = reader.GetGuid("subscription_uid"),
				Account = new Models.Account
				{
					AccountId = reader.GetInt32("account_id"),
					AccountUid = reader.GetGuid("account_uid")
				},
				Plan = new Plan
				{
					PlanId = reader.GetInt32("plan_id"),
					PlanUid = reader.GetGuid("plan_uid"),
					PlanName = reader.GetStringValue("plan_name"),
					PlanDescription = reader.GetStringValue("plan_description"),
					PromotionsPerMonth = reader.GetInt32("promotions_per_month"),
					SweepstakesPerMonth = reader.GetInt32("sweeps_per_month"),
					SweepstakesMaximumEntries = reader.GetInt32("sweepstakes_maximum_entries"),
					PricePerMonth = reader.GetDecimal("price_per_month"),
					PricePerYear = reader.GetDecimal("price_per_year"),
					HashtagsPerMonth = reader.GetInt32("sweep_hashtags_per_month")
				},
				IsDiscounted = reader.GetBoolean("is_discounted"),
				DiscountedBy = reader.GetInt32("discounted_by"),
				StartDate = reader.GetDateTime("start_date"),
				StartDateUtc = reader.GetDateTime("start_date_utc"),
				EndDate = reader.GetDateTime("end_date"),
				EndDateUtc = reader.GetDateTime("end_date_utc"),
				SubscriptionStatus = Enum.Parse<SubscriptionStatus>(reader.GetString("subscription_status"))
			};
		}
	}
}
