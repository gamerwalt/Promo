using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Models.Subscriptions;
using hrvster.Models.Subscriptions.Flutterwave;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Subscriptions
{
	public interface IPlansRepository
	{
		Task<Plan> GetPlanByUidAsync(Guid planUid, CancellationToken cancellationToken);
		Task<IEnumerable<Plan>> GetPlansAsync();
	}

	public class PlansRepository : IPlansRepository
	{
		private readonly IDatabase _database;
		public PlansRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<Plan> GetPlanByUidAsync(Guid planUid, CancellationToken cancellationToken)
		{
			var plan = default(Plan);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "plan_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("planUid", planUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					plan = CreatePlanFrom(reader);
				}
			}

			return plan;
		}

		public async Task<IEnumerable<Plan>> GetPlansAsync()
		{
			var plans = new List<Plan>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "plans_get";
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

		private Plan CreatePlanFrom(MySqlDataReader reader)
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
