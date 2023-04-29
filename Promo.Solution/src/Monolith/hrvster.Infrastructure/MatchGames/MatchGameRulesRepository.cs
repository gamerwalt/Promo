using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using hrvster.Models;
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
		Task CreateAsync(Models.Rule matchGameRule, CancellationToken cancellationToken);
		Task<IEnumerable<Models.Rule>> GetAllRulesAsync(Guid gameUid, CancellationToken cancellationToken);
		Task<Models.Rule> GetAsync(Guid ruleUid);
		IEnumerable<Models.Rule> GetRulesMatching(string watchValue);
		Task<IEnumerable<Models.Rule>> GetRulesMatchingAsync(string watchValue, CancellationToken cancellationToken);
	}

	public class MatchGameRulesRepository : IMatchGameRulesRepository
	{
		private readonly IDatabase _database;

		public MatchGameRulesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(Models.Rule matchGameRule, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_game_rule_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("ruleUid", matchGameRule.RuleUid)
				.AddParameter("matchGameUid", matchGameRule.Game.Uid)
				.AddParameter("watchTypeCode", matchGameRule.WatchTypeCode)
				.AddParameter("externalId", matchGameRule.ExternalId)
				.AddParameter("watchValue", matchGameRule.WatchValue)
				.AddParameter("activeInd", matchGameRule.ActiveInd)
				.AddParameter("mustBeFollowing", matchGameRule.MustBeFollowing)
				.AddParameter("scheduledInd", matchGameRule.ScheduledInd)
				.AddParameter("externalClientType", matchGameRule.ExternalClientType);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<IEnumerable<Models.Rule>> GetAllRulesAsync(Guid gameUid, CancellationToken cancellationToken)
		{
			var rules = new List<Models.Rule>();

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

		public async Task<Models.Rule> GetAsync(Guid ruleUid)
		{
			var rule = default(Models.Rule);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_game_rule_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("ruleUid", ruleUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					rule = CreateFrom(reader);
				}
			}

			return rule;
		}

		public IEnumerable<Models.Rule> GetRulesMatching(string watchValue)
		{
			var rules = new List<Models.Rule>();

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

		public async Task<IEnumerable<Models.Rule>> GetRulesMatchingAsync(string watchValue, CancellationToken cancellationToken)
		{
			var rules = new List<Models.Rule>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_games_get_rules_matching";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("watchValue", watchValue);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					rules.Add(CreateFrom(reader));
				}
			}

			return rules;
		}

		private Models.Rule CreateFrom(MySqlDataReader reader)
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
				Game = new Game
				{
					Id = reader.GetInt32("match_game_id"),
					Uid = reader.GetGuid("match_game_uid"),
					Name = reader.GetString("match_game_name"),
					GameType = GameTypesEnum.MatchGames,
					StartDate = reader.GetDateTime("start_date"),
					StartDateUtc = reader.GetDateTime("start_date_utc"),
					EndDate = reader.GetDateTime("end_date"),
					EndDateUtc = reader.GetDateTime("end_date_utc")
				},
				WatchTypeCode = Enum.Parse<WatchTypeCode>(reader.GetStringValue("watch_type_code")),
				WatchValue = reader.GetStringValue("watch_value"),
				ActiveInd = reader.GetBoolean("active_ind"),
				ExternalId = reader.GetStringValue("external_id"),
				MustBeFollowing = reader.GetBoolean("must_be_following"),
				NotifyWinners = reader.GetBoolean("notify_winners"),
				ScheduledInd = reader.GetBoolean("scheduled_ind"),
				Campaign = new Campaign
				{
					CampaignName = reader.GetString("campaign_name"),
					CampaignUid = reader.GetGuid("campaign_uid"),
					CampaignId = reader.GetInt32("campaign_id"),
					StartDate = reader.GetDateTime("campaign_start_date"),
					StartDateUtc = reader.GetDateTime("campaign_start_date_utc"),
					EndDate = reader.GetDateTime("campaign_end_date"),
					EndDateUtc = reader.GetDateTime("campaign_end_date_utc"),
					ActiveInd = reader.GetBoolean("campaign_active_ind"),
					CampaignType = new CampaignType
					{
						CampaignTypeName = reader.GetString("campaign_type_name"),
						CampaignTypeCode = Enum.Parse<CampaignTypeCode>(reader.GetString("campaign_type_code")),
						CampaignTypeUid = reader.GetGuid("campaign_type_uid"),
					},
					Account = new Account
					{
						AccountUid = reader.GetGuid("account_uid"),
						AccountId = reader.GetInt32("account_id")
					}
				},
				ExternalClientType = Enum.Parse<ExternalClientType>(reader.GetString("provider_name"))
			};
		}
	}
}
