using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using MySqlConnector;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchPlayRulesRepository
	{
		Task CreateAsync(Guid matchGameUid, int playPerUser, int playPerTweet, CancellationToken cancellationToken);
		Task<MatchPlayRule> GetAsync(Guid matchGameUid, CancellationToken cancellationToken);
	}

	public class MatchPlayRulesRepository : IMatchPlayRulesRepository
	{
		private readonly IDatabase _database;

		public MatchPlayRulesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(
			Guid matchGameUid, int playPerUser, int playPerTweet, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_play_rule_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter(nameof(matchGameUid), matchGameUid)
				.AddParameter("playRuleUid", Guid.NewGuid())
				.AddParameter(nameof(playPerUser), playPerUser)
				.AddParameter(nameof(playPerTweet), playPerTweet);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<MatchPlayRule> GetAsync(Guid matchGameUid, CancellationToken cancellationToken)
		{
			var playRule = default(MatchPlayRule);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_play_rules_get_for_game";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(matchGameUid), matchGameUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					playRule = CreateFrom(reader);
				}
			}

			return playRule;
		}

		private MatchPlayRule CreateFrom(MySqlDataReader reader)
		{
			return new MatchPlayRule
			{
				Id = reader.GetInt32("play_rule_id"),
				EntryRuleUid = reader.GetGuid("play_rule_uid"),
				PlayPerTweet = reader.GetInt32("play_per_tweet"),
				PlayPerGame = reader.GetInt32("play_per_game"),
				GameId = reader.GetInt32("match_game_id"),
				GameName = reader.GetString("match_game_name"),
				GameUid = reader.GetGuid("match_game_uid")
			};
		}
	}
}
