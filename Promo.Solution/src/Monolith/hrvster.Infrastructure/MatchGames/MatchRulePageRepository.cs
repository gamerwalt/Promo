using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchRulePageRepository
	{
		Task CreateAsync(MatchRulePage matchRulePage);
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
	}
}
