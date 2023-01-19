using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using hrvster.Infrastructure.Extensions;
using hrvster.Models.Sweepstakes;
using System.Threading;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace hrvster.Infrastructure
{
	public interface ISweepsRepository
	{
		Task CreateAsync(Sweep sweep, string timezone, bool isDaylight);
		Task<Sweep> GetAsync(Guid sweepUid);
		Task<IEnumerable<Sweep>> GetAllForCampaignAsync(Guid campaignUid);
		Task UpdateAsync(Guid sweepUid, Sweep sweep, string timezone, bool isDaylight);
		Task<IEnumerable<SweepDrawPropertiesAndRule>> GetSweepsRequiringADrawAsync();
		Task UpdateSweepAsDrawnAsync(Guid sweepUid);
		Task<IEnumerable<Sweep>> GetAllForAccountAsync(Guid accountUid);
		IEnumerable<Sweep> GetAllForAccount(Guid accountUid);
		IEnumerable<Sweep> GetAllForCampaign(Guid campaignUid);
		Task<IEnumerable<Sweep>> GetAllGoingLiveInHoursAsync(int hourValue, CancellationToken cancellationToken);
		Task<IEnumerable<Sweep>> GetLiveSweepsThatHaveNotBeenNotifiedAsync(CancellationToken cancellationToken);
	}

	public class SweepsRepository : ISweepsRepository
	{
		private readonly IDatabase _database;
		private readonly ILogger<SweepsRepository> _logger;

		public SweepsRepository(IDatabase database, ILoggerFactory loggerFactory)
		{
			_database = database;
			_logger = loggerFactory.CreateLogger<SweepsRepository>();
		}

		public async Task CreateAsync(Sweep sweep, string timezone, bool isDaylight)
		{
			var startDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(sweep.StartDate, TimeZoneInfo.Local.Id, timezone);
			var endDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(sweep.EndDate, TimeZoneInfo.Local.Id, timezone);
			var drawDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(sweep.DrawDate.GetValueOrDefault(), TimeZoneInfo.Local.Id, timezone);

			if (!isDaylight)
			{
				startDateUtc = startDateUtc.AddHours(-2);
				endDateUtc = endDateUtc.AddHours(-2);
				drawDateUtc = drawDateUtc.AddHours(-2);
			}

			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("sweepUid", sweep.SweepUid)
				.AddParameter("accountUid", sweep.Account?.AccountUid)
				.AddParameter("campaignUid", sweep.Campaign?.CampaignUid)
				.AddParameter("sweepName", sweep.Name)
				.AddParameter("startDate", sweep.StartDate)
				.AddParameter("startDateUtc", startDateUtc)
				.AddParameter("endDate", sweep.EndDate)
				.AddParameter("endDateUtc", endDateUtc)
				.AddParameter("drawDate", sweep.DrawDate.GetValueOrDefault())
				.AddParameter("drawDateUtc", drawDateUtc);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<IEnumerable<Sweep>> GetAllForCampaignAsync(Guid campaignUid)
		{
			var sweeps = new List<Sweep>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweeps_get_by_campaignUid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(campaignUid), campaignUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						sweeps.Add(CreateSweepFrom(reader));
					}
				}
			}

			return sweeps;
		}

		public async Task<IEnumerable<Sweep>> GetAllForAccountAsync(Guid accountUid)
		{
			var sweeps = new List<Sweep>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweeps_get_by_accountUid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(accountUid), accountUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						sweeps.Add(CreateSweepFrom(reader));
					}
				}
			}

			return sweeps;
		}

		public async Task<Sweep> GetAsync(Guid sweepUid)
		{
			var sweep = default(Sweep);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("sweepUid", sweepUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						sweep = CreateSweepFrom(reader);
					}
				}
			}

			return sweep;
		}

		public async Task<IEnumerable<SweepDrawPropertiesAndRule>> GetSweepsRequiringADrawAsync()
		{
			var sweeps = new List<SweepDrawPropertiesAndRule>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweeps_requiring_draw_get";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						sweeps.Add(CreateSweepDrawRuleFrom(reader));
					}
				}
			}

			return sweeps;
		}

		private SweepDrawPropertiesAndRule CreateSweepDrawRuleFrom(MySqlDataReader reader)
		{
			return new SweepDrawPropertiesAndRule
			{
				Sweep = new Sweep
				{
					SweepId = reader.GetInt32("sweep_id"),
					SweepUid = reader.GetGuid("sweep_uid"),
					Name = reader.GetStringValue("sweep_name"),
					RulesUrl = reader.GetStringValue("sweep_rule_url"),
					StartDate = reader.GetNullableDateTimeValue("sweep_start_date").GetValueOrDefault(),
					StartDateUtc = reader.GetNullableDateTimeValue("sweep_start_date_utc").GetValueOrDefault(),
					EndDate = reader.GetNullableDateTimeValue("sweep_end_date").GetValueOrDefault(),
					EndDateUtc = reader.GetNullableDateTimeValue("sweep_end_date_utc").GetValueOrDefault(),
					DrawDate = reader.GetNullableDateTimeValue("sweep_draw_date").GetValueOrDefault(),
					DrawDateUtc = reader.GetNullableDateTimeValue("sweep_draw_date_utc").GetValueOrDefault(),
					Drawn = reader.GetBoolean("sweep_drawn_indicator"),
					DateDrawn = reader.GetNullableDateTimeValue("sweep_date_drawn"),
					DrawAttempted = reader.GetBoolean("draw_attempted")
				},
				SweepRule = new Models.Rule
				{
					WatchValue = reader.GetStringValue("sweep_rule_watch_value"),
					ActiveInd = reader.GetBoolean("sweep_rule_active_indidcator"),
					ExternalId = reader.GetLongValue("external_id"),
					MustBeFollowing = reader.GetBoolean("must_be_following"),
					NotifyWinners = reader.GetBoolean("notify_winners")
				},
				Campaign = new Campaign
				{
					CampaignId = reader.GetInt32("campaign_id"),
					CampaignUid = reader.GetGuid("campaign_uid"),
					CampaignName = reader.GetStringValue("campaign_name"),
					Description = reader.GetStringValue("campaign_description"),
					ActiveInd = reader.GetBoolean("campaign_active_ind"),
					StartDate = reader.GetNullableDateTimeValue("campaign_start_date").GetValueOrDefault(),
					EndDate = reader.GetNullableDateTimeValue("campaign_end_date").GetValueOrDefault(),
					CampaignType = new CampaignType
					{
						CampaignTypeCode = (CampaignTypeCode)Enum.Parse(typeof(CampaignTypeCode), reader.GetString("campaign_type_code")),
						CampaignTypeName = reader.GetStringValue("campaign_type_name"),
						CamapaignTypeDescription = reader.GetStringValue("campaign_type_description")
					}
				},
				DrawRule = new DrawRule
				{
					SweepDrawruleId = reader.GetInt32("sweep_draw_rule_id"),
					SweepDrawRuleUid = reader.GetGuid("sweep_draw_rule_uid"),
					WinnerCount = reader.GetInt32("winner_count"),
					WinnerBackupCount = reader.GetInt32("winner_backup_count")
				},
				WatchType = new WatchType
				{
					WatchTypeId = reader.GetInt32("watch_type_id"),
					WatchTypeCode = reader.GetString("watch_type_code"),
					WatchTypeName = reader.GetString("watch_type_name")
				},
				Account = new Account
				{
					AccountId = reader.GetInt32("account_id"),
					AccountUid = reader.GetGuid("account_uid"),
					EmailAddress = reader.GetString("email_address")
				}
			};
		}

		public async Task UpdateAsync(Guid sweepUid, Sweep sweep, string timezone, bool isDaylight)
		{
			var startDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(sweep.StartDate, TimeZoneInfo.Local.Id, timezone);
			var endDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(sweep.EndDate, TimeZoneInfo.Local.Id, timezone);
			var drawDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(sweep.DrawDate.GetValueOrDefault(), TimeZoneInfo.Local.Id, timezone);

			if (!isDaylight)
			{
				startDateUtc = startDateUtc.AddHours(-2);
				endDateUtc = endDateUtc.AddHours(-2);
				drawDateUtc = drawDateUtc.AddHours(-2);
			}

			_logger.LogInformation("Updating the sweepstake {sweepName} with start date of {startDate} and utc {startDateUtc} and end date of {endDate} and utc {endDateUtc}.",
				sweep.Name,
				sweep.StartDate,
				startDateUtc,
				sweep.EndDate,
				endDateUtc);

			_logger.LogInformation("Current timezone info is {timezoneInfo}", timezone);

			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_update";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("sweepUid", sweepUid)
				.AddParameter("sweepName", sweep.Name)
				.AddParameter("startDate", sweep.StartDate)
				.AddParameter("startDateUtc", startDateUtc)
				.AddParameter("endDate", sweep.EndDate)
				.AddParameter("endDateUtc", endDateUtc)
				.AddParameter("drawDate", sweep.DrawDate.Value)
				.AddParameter("drawDateUtc", drawDateUtc);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task UpdateSweepAsDrawnAsync(Guid sweepUid)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_update_as_drawn";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("sweepUid", sweepUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		private Sweep CreateSweepFrom(MySqlDataReader reader)
		{
			return new Sweep
			{
				SweepId = reader.GetInt32("sweep_id"),
				Campaign = new Campaign
				{
					CampaignUid = reader.GetGuid("campaign_uid"),
					CampaignName = reader.GetString("campaign_name"),
					ActiveInd = reader.GetBoolean("active_ind"),
					StartDate = reader.GetDateTime("campaign_start_date"),
					EndDate = reader.GetDateTime("campaign_end_date"),
					CampaignType = new CampaignType
					{
						CampaignTypeCode = (CampaignTypeCode)Enum.Parse(typeof(CampaignTypeCode), reader.GetString("campaign_type_code")),
						CampaignTypeName = reader.GetString("campaign_type_name")
					}
				},
				Account = new Account
				{
					AccountUid = reader.GetGuid("account_uid")
				},
				SweepUid = reader.GetGuid("sweep_uid"),
				Name = reader.GetString("sweep_name"),
				RulesUrl = GetStringValue(reader, "rules_url"),
				StartDate = reader.GetDateTime("start_date"),
				StartDateUtc = reader.GetDateTime("start_date_utc"),
				EndDate = reader.GetDateTime("end_date"),
				EndDateUtc = reader.GetDateTime("end_date_utc"),
				DrawDate = GetNullableDateTimeValue(reader, "draw_date"),
				DrawDateUtc = reader.GetNullableDateTimeValue("draw_date_utc").GetValueOrDefault(),
				DrawAttempted = reader.GetBoolean("draw_attempted"),
				NotEnoughUniqueEntrants = reader.GetBoolean("not_enough_unique_entrants"),
				NotificationSentGoingLive = reader.GetBoolean("notification_sent_going_live")
			};
		}

		private string GetStringValue(MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}

			return reader[fieldName].ToString();
		}

		private DateTime? GetNullableDateTimeValue(MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			return reader.GetDateTime(fieldName);
		}

		public IEnumerable<Sweep> GetAllForAccount(Guid accountUid)
		{
			var sweeps = new List<Sweep>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweeps_get_by_accountUid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(accountUid), accountUid);

				conn.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						sweeps.Add(CreateSweepFrom(reader));
					}
				}
			}

			return sweeps;
		}

		public IEnumerable<Sweep> GetAllForCampaign(Guid campaignUid)
		{
			var sweeps = new List<Sweep>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweeps_get_by_campaignUid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(campaignUid), campaignUid);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					sweeps.Add(CreateSweepFrom(reader));
				}
			}

			return sweeps;
		}

		public async Task<IEnumerable<Sweep>> GetAllGoingLiveInHoursAsync(int hourValue, CancellationToken cancellationToken)
		{
			var sweeps = new List<Sweep>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweeps_going_live_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(hourValue), hourValue);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync())
				{
					sweeps.Add(CreateSweepFrom(reader));
				}
			}

			return sweeps;
		}

		public async Task<IEnumerable<Sweep>> GetLiveSweepsThatHaveNotBeenNotifiedAsync(CancellationToken cancellationToken)
		{
			var sweeps = new List<Sweep>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweeps_live_not_notified";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync())
				{
					sweeps.Add(CreateRecordFrom(reader));
				}
			}

			return sweeps;
		}

		private Sweep CreateRecordFrom(MySqlDataReader reader)
		{
			return new Sweep
			{
				SweepId = reader.GetInt32("sweep_id"),
				Campaign = new Campaign
				{
					CampaignUid = reader.GetGuid("campaign_uid"),
					CampaignName = reader.GetString("campaign_name"),
					ActiveInd = reader.GetBoolean("active_ind"),
					StartDate = reader.GetDateTime("campaign_start_date"),
					EndDate = reader.GetDateTime("campaign_end_date"),
					CampaignType = new CampaignType
					{
						CampaignTypeCode = (CampaignTypeCode)Enum.Parse(typeof(CampaignTypeCode), reader.GetString("campaign_type_code")),
						CampaignTypeName = reader.GetString("campaign_type_name")
					}
				},
				Account = new Account
				{
					AccountUid = reader.GetGuid("account_uid")
				},
				SweepUid = reader.GetGuid("sweep_uid"),
				Name = reader.GetString("sweep_name"),
				RulesUrl = GetStringValue(reader, "rules_url"),
				StartDate = reader.GetDateTime("start_date"),
				StartDateUtc = reader.GetDateTime("start_date_utc"),
				EndDate = reader.GetDateTime("end_date"),
				EndDateUtc = reader.GetDateTime("end_date_utc"),
				DrawDate = GetNullableDateTimeValue(reader, "draw_date"),
				DrawDateUtc = reader.GetNullableDateTimeValue("draw_date_utc").GetValueOrDefault(),
				DrawAttempted = reader.GetBooleanRecord("draw_attempted"),
				SweepNotification = new SweepNotification
				{
					NotEnoughUniqueEntrants = reader.GetBooleanRecord("not_enough_unique_entrants"),
					SweepsGoingLive = reader.GetBooleanRecord("sweeps_going_live"),
					SweepsLive = reader.GetBooleanRecord("sweeps_live"),
					DrawAttemptHappened = reader.GetBooleanRecord("draw_attempted"),
					DrawHappened = reader.GetBooleanRecord("draw_happened")
				}
			};
		}
	}
}
