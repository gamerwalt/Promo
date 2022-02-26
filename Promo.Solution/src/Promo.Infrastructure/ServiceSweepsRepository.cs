using MySqlConnector;
using Promo.Core.Extensions;
using Promo.Core.MessageContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Infrastructure
{
	public interface IServiceSweepsRepository
	{
		Task CreateAsync(ISweepCreated sweepCreated, string connectionString);
		Task UpdateAsync(ISweepUpdated sweepUpdated, string connectionString);
	}

	public class ServiceSweepsRepository : IServiceSweepsRepository
	{
		public async Task CreateAsync(ISweepCreated sweepCreated, string connectionString)
		{
			using var conn = new MySqlConnection(connectionString);
			var command = conn.CreateCommand();
			command.CommandText = "sweep_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("accountUid", sweepCreated.AccountUid)
				.AddParameter("campaignUid", sweepCreated.CampaignUid)
				.AddParameter("sweepUid", sweepCreated.SweepUid)
				.AddParameter("sweepName", sweepCreated.SweepName)
				.AddParameter("rulesUrl", sweepCreated.RulesUrl)
				.AddParameter("startDate", sweepCreated.StartDate)
				.AddParameter("endDate", sweepCreated.EndDate)
				.AddParameter("drawDate", sweepCreated.DrawDate);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task UpdateAsync(ISweepUpdated sweepUpdated, string connectionString)
		{
			using var conn = new MySqlConnection(connectionString);
			var command = conn.CreateCommand();
			command.CommandText = "sweep_update";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("sweepUid", sweepUpdated.SweepUid)
				.AddParameter("sweepName", sweepUpdated.SweepName)
				.AddParameter("rulesUrl", sweepUpdated.RulesUrl)
				.AddParameter("startDate", sweepUpdated.StartDate)
				.AddParameter("endDate", sweepUpdated.EndDate)
				.AddParameter("drawDate", sweepUpdated.DrawDate)
				.AddParameter("drawnIndicator", sweepUpdated.DrawnIndicator)
				.AddParameter("dateDrawn", sweepUpdated.DateDrawn);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}
