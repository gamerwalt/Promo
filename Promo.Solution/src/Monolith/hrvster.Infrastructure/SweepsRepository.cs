using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using hrvster.Infrastructure.Extensions;
using hrvster.Models.Sweepstakes;

namespace hrvster.Infrastructure
{
	public interface ISweepsRepository
	{
		Task CreateAsync(Sweep sweep);
		Task<Sweep> GetAsync(Guid sweepUid);
		Task<IEnumerable<Sweep>> GetAllForCampaignAsync(Guid campaignUid);
		Task UpdateAsync(Guid sweepUid, Sweep sweep);
		Task<IEnumerable<SweepDrawPropertiesAndRule>> GetSweepsRequiringADrawAsync();
		Task UpdateSweepAsDrawnAsync(Guid sweepUid);
		Task<IEnumerable<Sweep>> GetAllForAccountAsync(Guid accountUid);
		IEnumerable<Sweep> GetAllForAccount(Guid accountUid);
		IEnumerable<Sweep> GetAllForCampaign(Guid campaignUid);
	}

	public class SweepsRepository : ISweepsRepository
	{
		private readonly IDatabase _database;

		public SweepsRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(Sweep sweep)
		{
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
				.AddParameter("endDate", sweep.EndDate)
				.AddParameter("drawDate", sweep.DrawDate);

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
					EndDate = reader.GetNullableDateTimeValue("sweep_end_date").GetValueOrDefault(),
					DrawDate = reader.GetNullableDateTimeValue("sweep_draw_date").GetValueOrDefault(),
					Drawn = reader.GetBoolean("sweep_drawn_indicator"),
					DateDrawn = reader.GetNullableDateTimeValue("sweep_date_drawn")
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
				}
			};
		}

		public async Task UpdateAsync(Guid sweepUid, Sweep sweep)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_update";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("sweepUid", sweepUid)
				.AddParameter("sweepName", sweep.Name)
				.AddParameter("startDate", sweep.StartDate)
				.AddParameter("endDate", sweep.EndDate)
				.AddParameter("drawDate", sweep.DrawDate.Value);

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
				EndDate = reader.GetDateTime("end_date"),
				DrawDate = GetNullableDateTimeValue(reader, "draw_date")
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
	}
}
