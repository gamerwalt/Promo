using hrvster.Models.Subscriptions;
using hrvster.Models.Subscriptions.Flutterwave;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Subscriptions
{
	public class FlutterwavePlanRepository : IPlanRepository
	{
		private readonly IDatabase _database;
		public FlutterwavePlanRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<IEnumerable<Plan>> GetPlansAsync()
		{
			var plans = new List<Plan>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "flutterwave_plans_get";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					plans.Add(CreatePlanFrom(reader));
				}
			}

			return plans;
		}

		private static Plan CreatePlanFrom(MySqlDataReader reader)
		{
			return new Plan
			{
				PlanId = reader.GetInt32("plan_id"),
				PlanUid = reader.GetGuid("plan_uid"),
				PlanName = reader.GetString("plan_name"),
				PlanDescription = reader.GetString("plan_description"),
				PromotionsPerMonth = reader.GetInt32("promotions_per_month"),
				SweepstakesPerMonth = reader.GetInt32("sweeps_per_month"),
				SweepstakesMaximumEntries = reader.GetInt32("sweepstakes_maximum_entries"),
				HashtagsPerMonth = reader.GetInt32("sweep_hashtags_per_month"),
				PricePerMonth = reader.GetDecimal("price_per_month"),
				PricePerYear = reader.GetDecimal("price_per_year"),
				FlutterwavePlan = new FlutterwavePlan
				{
					FlutterwavePlanId = reader.GetInt32("flutterwave_plan_id"),
					FlutterwavePlanUid = reader.GetGuid("flutterwave_plan_uid"),
					ExternalMonthlyPaymentLink = new Uri(reader.GetString("external_monthly_payment_link"))
				}
			};
		}
	}
}
