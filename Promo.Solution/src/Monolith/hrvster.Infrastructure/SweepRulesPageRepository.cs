using hrvster.Infrastructure.Extensions;
using hrvster.Models.Sweepstakes;
using MySqlConnector;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface ISweepRulesPageRepository
	{
		Task CreateAsync(SweepRulesPage sweepRulesPage);
		Task<SweepRulesPage> GetByRuleIdAsync(int ruleId);
	}

	public class SweepRulesPageRepository : ISweepRulesPageRepository
	{
		private readonly IDatabase _database;

		public SweepRulesPageRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(SweepRulesPage sweepRulesPage)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_rules_page_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("Uid", sweepRulesPage.Uid)
				.AddParameter("sweepRuleId", sweepRulesPage.SweepRuleId)
				.AddParameter("pageId", sweepRulesPage.PageId);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<SweepRulesPage> GetByRuleIdAsync(int ruleId)
		{
			var sweepRulePage = default(SweepRulesPage);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_rule_page_get_by_rule_id";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("ruleId", ruleId);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					sweepRulePage = CreateRecordFrom(reader);
				}
			}

			return sweepRulePage;
		}

		private SweepRulesPage CreateRecordFrom(MySqlDataReader reader)
		{
			return new SweepRulesPage
			{
				Id = reader.GetInt32("id"),
				Uid = reader.GetGuid("uid"),
				PageId = reader.GetStringValue("page_id"),
				SweepRuleId = reader.GetInt32("rule_id")
			};
		}
	}
}
