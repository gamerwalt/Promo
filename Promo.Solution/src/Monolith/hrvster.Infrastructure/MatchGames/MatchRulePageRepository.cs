using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchRulePageRepository
	{
		Task CreateAsync(MatchRulePage matchRulePage);
		Task<MatchRulePage> GetByRuleId(int ruleId);
	}

	public class MatchRulePageRepository : IMatchRulePageRepository
	{
		private readonly IDatabase _database;

		public MatchRulePageRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(MatchRulePage matchRulePage)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_rules_page_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("Uid", matchRulePage.Uid)
				.AddParameter("matchRuleId", matchRulePage.MatchRuleId)
				.AddParameter("pageId", matchRulePage.PageId);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<MatchRulePage> GetByRuleId(int ruleId)
		{
			var matchRulePage = default(MatchRulePage);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_rule_page_get_by_rule_id";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("ruleId", ruleId);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					matchRulePage = CreateRecordFrom(reader);
				}
			}

			return matchRulePage;
		}

		private MatchRulePage CreateRecordFrom(MySqlDataReader reader)
		{
			return new MatchRulePage
			{
				Id = reader.GetInt32("id"),
				Uid = reader.GetGuid("uid"),
				PageId = reader.GetStringValue("page_id"),
				MatchRuleId = reader.GetInt32("rule_id")
			};
		}
	}
}
