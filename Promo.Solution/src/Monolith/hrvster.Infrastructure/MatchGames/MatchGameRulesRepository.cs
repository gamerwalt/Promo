using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchGameRulesRepository
	{
		Task CreateAsync(MatchGameRule matchGameRule, CancellationToken cancellationToken);
		Task<IEnumerable<MatchGameRule>> GetAllRulesAsync(Guid gameUid, CancellationToken cancellationToken);
		IEnumerable<MatchGameRule> GetRulesMatching(string watchValue);
	}

	public class MatchGameRulesRepository : IMatchGameRulesRepository
	{
		private readonly IDatabase _database;

		public MatchGameRulesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(MatchGameRule matchGameRule, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_game_rule_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("ruleUid", matchGameRule.Uid)
				.AddParameter("matchGameUid", matchGameRule.MatchGame.Uid)
				.AddParameter("watchTypeCode", matchGameRule.WatchType.WatchTypeCode)
				.AddParameter("externalId", matchGameRule.ExternalId)
				.AddParameter("watchValue", matchGameRule.WatchValue)
				.AddParameter("activeInd", matchGameRule.ActiveIndicator)
				.AddParameter("mustBeFollowing", matchGameRule.MustBeFollowing)
				.AddParameter("scheduledInd", matchGameRule.ScheduledIndicator);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<IEnumerable<MatchGameRule>> GetAllRulesAsync(Guid gameUid, CancellationToken cancellationToken)
		{
			var rules = new List<MatchGameRule>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_game_rules_get_by_match_game_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("matchGameUid", gameUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					rules.Add(CreateFrom(reader));
				}
			}

			return rules;
		}

		public IEnumerable<MatchGameRule> GetRulesMatching(string watchValue)
		{
			var rules = new List<MatchGameRule>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_games_get_rules_matching";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("watchValue", watchValue);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					rules.Add(CreateFrom(reader));
				}
			}

			return rules;
		}

		private MatchGameRule CreateFrom(MySqlDataReader reader)
		{
			return new MatchGameRule
			{
				Id = reader.GetInt32("rule_id"),
				Uid = reader.GetGuid("rule_uid"),
				MatchGame = new MatchGame
				{
					Id = reader.GetInt32("match_game_id"),
					Uid = reader.GetGuid("match_game_uid")
				},
				WatchType = new Models.WatchType
				{
					WatchTypeCode = reader.GetStringValue("watch_type_code"),
					WatchTypeName = reader.GetStringValue("watch_type_name")
				},
				WatchValue = reader.GetStringValue("watch_value"),
				ActiveIndicator = reader.GetBoolean("active_ind"),
				ExternalId = reader.GetStringValue("external_id"),
				MustBeFollowing = reader.GetBoolean("must_be_following"),
				NotifyWinners = reader.GetBoolean("notify_winners"),
				ScheduledIndicator = reader.GetBoolean("scheduled_ind")
			};
		}
	}
}
