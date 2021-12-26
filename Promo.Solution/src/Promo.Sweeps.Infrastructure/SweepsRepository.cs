using MySqlConnector;
using Promo.Core;
using Promo.Core.Extensions;
using Promo.Sweeps.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Promo.Sweeps.Infrastructure
{
    public interface ISweepsRepository
    {
        Task CreateAsync(Sweep sweep);
        Task<Sweep> GetAsync(Guid sweepUid);
        Task<IEnumerable<Sweep>> GetAllForCampaignAsync(Guid campaignUid);
        Task UpdateAsync(Guid sweepUid, Sweep sweep);
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
                .AddParameter("campaignUid", sweep.CampaignUid)
                .AddParameter("sweepName", sweep.Name)
                .AddParameter("startDate", sweep.StartDate)
                .AddParameter("endDate", sweep.EndDate);

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

        public async Task UpdateAsync(Guid sweepUid, Sweep sweep)
        {
            using var conn = new MySqlConnection(_database.GetConnectionString());
            var command = conn.CreateCommand();
            command.CommandText = "sweep_update";
            command.CommandType = CommandType.StoredProcedure;
            command
                .AddParameter("sweepUid", sweep.SweepUid)
                .AddParameter("sweepName", sweep.Name)
                .AddParameter("startDate", sweep.StartDate)
                .AddParameter("endDate", sweep.EndDate)
                .AddParameter("drawDate", sweep.DrawDate.Value);

            await conn.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        private Sweep CreateSweepFrom(MySqlDataReader reader)
        {
            return new Sweep
            {
                SweepId = reader.GetInt32("sweep_id"),
                CampaignUid = reader.GetGuid("campaign_uid"),
                SweepUid = reader.GetGuid("sweep_uid"),
                Name = reader.GetString("sweep_name"),
                StartDate = reader.GetDateTime("start_date"),
                EndDate = reader.GetDateTime("end_date"),
                DrawDate = GetNullableDateTimeValue(reader, "draw_date"),
                CreatedDate = reader.GetDateTime("created_date"),
                UpdatedDate = GetNullableDateTimeValue(reader, "updated_date"),
            };
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
    }
}
