using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Models.Properties;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchGamesRepository
	{
		Task CreateAsync(MatchGame matchGame, string timezone, bool isDaylight, CancellationToken cancellationToken);
		IEnumerable<MatchGame> GetAllForAccount(Guid accountUid);
		Task<IEnumerable<MatchGame>> GetByAccountAsync(Guid accountUid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchGame>> GetByCampaignAsync(Guid campaignUid, CancellationToken cancellationToken);
		Task<MatchGame> GetAsync(Guid matchGameUid, CancellationToken cancellationToken);
		Task SetAsGeneratedAsync(MatchGame game, CancellationToken cancellationToken);
		Task SetAsReadyToBeGeneratedAsync(MatchGame game, CancellationToken cancellationToken);
		Task UpdateAsync(MatchGame matchGame, string timezone, bool isDaylight, CancellationToken cancellationToken);
		MatchGame Get(Guid matchGameUid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchGame>> GetGameReadyForGenerationAsync(CancellationToken token);
		Task<IEnumerable<Models.Rule>> GetRulesAsync(CancellationToken cancellationToken);
	}

	public class MatchGamesRepository : IMatchGamesRepository
	{
		private readonly IDatabase _database;

		public MatchGamesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(MatchGame matchGame, string timezone, bool isDaylight, CancellationToken cancellationToken)
		{
			var startDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(matchGame.StartDate, TimeZoneInfo.Local.Id, timezone);
			var endDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(matchGame.EndDate, TimeZoneInfo.Local.Id, timezone);
			var generateBoardDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(matchGame.GenerateBoardDate, TimeZoneInfo.Local.Id, timezone);

			if (!isDaylight)
			{
				startDateUtc = startDateUtc.AddHours(-2);
				endDateUtc = endDateUtc.AddHours(-2);
				generateBoardDateUtc = generateBoardDateUtc.AddHours(-2);
			}

			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_game_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("campaignId", matchGame.CampaignId)
				.AddParameter("matchGameUid", matchGame.Uid)
				.AddParameter("matchGameName", matchGame.Name)
				.AddParameter("matchGameDescription", matchGame.Description)
				.AddParameter("numberOfBoards", matchGame.NumberOfBoards)
				.AddParameter("tileCountPerBoard", matchGame.TileCountPerBoard)
				.AddParameter("numberOfWinnableBoards", matchGame.NumberOfWinnableBoards)
				.AddParameter("winProbabilityPerBoard", matchGame.WinProbability)
				.AddParameter("startDate", matchGame.StartDate)
				.AddParameter("startDateUtc", startDateUtc)
				.AddParameter("endDate", matchGame.EndDate)
				.AddParameter("endDateUtc", endDateUtc)
				.AddParameter("generateBoardDate", matchGame.GenerateBoardDate)
				.AddParameter("generateBoardDateUtc", generateBoardDateUtc);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public IEnumerable<MatchGame> GetAllForAccount(Guid accountUid)
		{
			var matchGames = new List<MatchGame>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_games_get_by_account_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", accountUid);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					matchGames.Add(CreateFrom(reader));
				}
			}

			return matchGames;
		}

		public async Task<IEnumerable<MatchGame>> GetByAccountAsync(Guid accountUid, CancellationToken cancellationToken)
		{
			var matchGames = new List<MatchGame>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_games_get_by_account_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", accountUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchGames.Add(CreateFrom(reader));
				}
			}

			return matchGames;
		}

		public async Task<IEnumerable<MatchGame>> GetByCampaignAsync(Guid campaignUid, CancellationToken cancellationToken)
		{
			var matchGames = new List<MatchGame>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_games_get_by_campaign_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("campaignUid", campaignUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchGames.Add(CreateFrom(reader));
				}
			}

			return matchGames;
		}

		public async Task<MatchGame> GetAsync(Guid matchGameUid, CancellationToken cancellationToken)
		{
			var matchGame = default(MatchGame);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_game_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("matchGameUid", matchGameUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchGame = CreateFrom(reader);
				}
			}

			return matchGame;
		}

		public async Task SetAsGeneratedAsync(MatchGame matchGame, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_game_set_as_generated";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("matchGameUid", matchGame.Uid);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task SetAsReadyToBeGeneratedAsync(MatchGame matchGame, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_game_set_as_ready_to_be_generated";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("matchGameUid", matchGame.Uid);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task UpdateAsync(MatchGame matchGame, string timezone, bool isDaylight, CancellationToken cancellationToken)
		{
			var startDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(matchGame.StartDate, TimeZoneInfo.Local.Id, timezone);
			var endDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(matchGame.EndDate, TimeZoneInfo.Local.Id, timezone);

			if (!isDaylight)
			{
				startDateUtc = startDateUtc.AddHours(-2);
				endDateUtc = endDateUtc.AddHours(-2);
			}

			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_game_update";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("matchGameUid", matchGame.Uid)
				.AddParameter("matchGameName", matchGame.Name)
				.AddParameter("matchGameDescription", matchGame.Description)
				.AddParameter("numberOfBoards", matchGame.NumberOfBoards)
				.AddParameter("tileCountPerBoard", matchGame.TileCountPerBoard)
				.AddParameter("numberOfWinnableBoards", matchGame.NumberOfWinnableBoards)
				.AddParameter("winProbabilityPerBoard", matchGame.WinProbability)
				.AddParameter("startDate", matchGame.StartDate)
				.AddParameter("startDateUtc", startDateUtc)
				.AddParameter("endDate", matchGame.EndDate)
				.AddParameter("endDateUtc", endDateUtc);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		private MatchGame CreateFrom(MySqlDataReader reader)
		{
			return new MatchGame
			{
				Id = reader.GetInt32("match_game_id"),
				Uid = reader.GetGuid("match_game_uid"),
				CampaignId = reader.GetInt32("campaign_id"),
				CampaignUid = reader.GetGuid("campaign_uid"),
				Name = reader.GetString("match_game_name"),
				Description = reader.GetString("match_game_description"),
				NumberOfBoards = reader.GetInt32("number_of_boards"),
				TileCountPerBoard = reader.GetInt32("tile_count_per_board"),
				NumberOfWinnableBoards = reader.GetInt32("number_of_winnable_boards"),
				WinProbability = reader.GetInt32("win_probability_per_board"),
				StartDate = reader.GetDateTime("start_date"),
				StartDateUtc = reader.GetDateTime("start_date_utc"),
				EndDate = reader.GetDateTime("end_date"),
				EndDateUtc = reader.GetDateTime("end_date_utc"),
				HasBeenGenerated = reader.GetBoolean("is_generated"),
				ReadyToBeGenerated = reader.GetBoolean("ready_to_generate"),
				GenerateBoardDate = reader.GetDateTime("generate_board_date"),
				GenerateBoardDateUtc = reader.GetDateTime("generate_board_date_utc")
			};
		}

		public MatchGame Get(Guid matchGameUid, CancellationToken cancellationToken)
		{
			var matchGame = default(MatchGame);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_game_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("matchGameUid", matchGameUid);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					matchGame = CreateFrom(reader);
				}
			}

			return matchGame;
		}

		public async Task<IEnumerable<MatchGame>> GetGameReadyForGenerationAsync(CancellationToken cancellationToken)
		{
			var matchGames = new List<MatchGame>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_games_get_ready_for_generation";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchGames.Add(CreateFrom(reader));
				}
			}

			return matchGames;
		}

		public async Task<IEnumerable<Models.Rule>> GetRulesAsync(CancellationToken cancellationToken)
		{
			var matchGameRules = new List<MatchGameRule>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_games_get_rules";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchGameRules.Add(CreateRuleFrom(reader));
				}
			}

			return matchGameRules;
		}

		private MatchGameRule CreateRuleFrom(MySqlDataReader reader)
		{
			return new MatchGameRule
			{
				RuleId = reader.GetInt32("rule_id"),
				RuleUid = reader.GetGuid("rule_uid"),
				Campaign = new Campaign
				{
					CampaignId = reader.GetInt32("campaign_id"),
					CampaignUid = reader.GetGuid("campaign_uid"),
					CampaignName = reader.GetStringValue("campaign_name"),
					Description = reader.GetStringValue("campaign_description"),
					ActiveInd = reader.GetBoolean("campaign_active_ind"),
					StartDate = reader.GetDateTime("campaign_start_date"),
					EndDate = reader.GetDateTime("campaign_end_date"),
					StartDateUtc = reader.GetDateTime("campaign_start_date_utc"),
					EndDateUtc	= reader.GetDateTime("campaign_end_date_utc"),
					CampaignType = new CampaignType
					{
						CampaignTypeCode = Enum.Parse<CampaignTypeCode>(reader.GetStringValue("campaign_type_code")),
						CampaignTypeName = reader.GetStringValue("campaign_type_name")
					},
					Account = new Account
					{
						AccountUid = reader.GetGuid("account_uid"),
						EmailAddress = reader.GetStringValue("email_address"),
						DisplayName = reader.GetStringValue("display_name"),
						LastName = reader.GetStringValue("last_name"),
						FirstName = reader.GetStringValue("first_name")
					}
				},
				WatchValue = reader.GetStringValue("watch_value"),
				ActiveInd = reader.GetBoolean("active_ind"),
				ScheduledInd = reader.GetBoolean("scheduled_ind"),
				MustBeFollowing = reader.GetBoolean("must_be_following"),
				NotifyWinners = reader.GetBoolean("notify_winners"),
				ExternalId = reader.GetStringValue("external_id"),
				MatchGame = new MatchGame
				{
					Id = reader.GetInt32("match_game_id"),
					Name = reader.GetStringValue("match_game_name"),
					Uid = reader.GetGuid("match_game_uid"),
					StartDate = reader.GetDateTime("start_date"),
					StartDateUtc = reader.GetDateTime("start_date_utc"),
					EndDate = reader.GetDateTime("end_date"),
					EndDateUtc = reader.GetDateTime("end_date_utc")
				},
				Game = new Game
				{
					Id = reader.GetInt32("match_game_id"),
					Name = reader.GetStringValue("match_game_name"),
					Uid = reader.GetGuid("match_game_uid"),
					StartDate = reader.GetDateTime("start_date"),
					StartDateUtc = reader.GetDateTime("start_date_utc"),
					EndDate = reader.GetDateTime("end_date"),
					EndDateUtc = reader.GetDateTime("end_date_utc")
				},
				ExternalClientType = Enum.Parse<ExternalClientType>(reader.GetString("provider_name"))
			};
		}
	}
}
