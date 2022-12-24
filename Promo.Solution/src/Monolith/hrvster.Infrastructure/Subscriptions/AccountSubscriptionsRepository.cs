using hrvster.Infrastructure.Extensions;
using hrvster.Models.Subscriptions;
using MySqlConnector;
using System;
using System.Data;

namespace hrvster.Infrastructure.Subscriptions
{
	public interface IAccountSubscriptionsRepository
	{
		AccountSubscription GetAccountSubscription(Guid accountUid);
	}

	public class AccountSubscriptionsRepository : IAccountSubscriptionsRepository
	{
		private readonly IDatabase _database;
		public AccountSubscriptionsRepository(IDatabase database)
		{
			_database = database;
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
					account = CreateAccounbtSubscriptionFrom(reader);
				}
			}

			return account;
		}

		private AccountSubscription CreateAccounbtSubscriptionFrom(MySqlDataReader reader)
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
					PricePerYear = reader.GetDecimal("price_per_year")
				},
				IsDiscounted = reader.GetBoolean("is_discounted"),
				DiscountedBy = reader.GetInt32("discounted_by"),
				StartDate = reader.GetDateTime("start_date"),
				StartDateUtc = reader.GetDateTime("start_date_utc"),
				EndDate = reader.GetDateTime("end_date"),
				EndDateUtc = reader.GetDateTime("end_date_utc")
			};
		}
	}
}
