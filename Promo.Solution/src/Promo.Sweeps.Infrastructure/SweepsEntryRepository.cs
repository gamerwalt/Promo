using MySqlConnector;
using Promo.Core;
using Promo.Core.Extensions;
using Promo.Sweeps.Infrastructure.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Promo.Sweeps.Infrastructure
{
    public interface ISweepsEntryRepository
    {
        Task CreateAsync(SweepEntry sweepEntry);
        Task<SweepEntry> GetAsync(Guid sweepEntryGuid);
    }

    public class SweepsEntryRepository : ISweepsEntryRepository
    {
        private readonly IDatabase _database;

        public SweepsEntryRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task CreateAsync(SweepEntry sweepEntry)
        {
            using var conn = new MySqlConnection(_database.GetConnectionString());
            var command = conn.CreateCommand();
            command.CommandText = "sweep_add_entry";
            command.CommandType = CommandType.StoredProcedure;
            command
                .AddParameter("sweepEntryUid", sweepEntry.SweepEntryUid)
                .AddParameter("sweepUid", sweepEntry.Sweep.SweepUid)
                .AddParameter("userIdentifier", sweepEntry.UserIdentifier)
                .AddParameter("userIdentifierType", sweepEntry.UserIdentifierType.ToString())
                .AddParameter("acceptedTerms", sweepEntry.AcceptedTerms);

            await conn.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<SweepEntry> GetAsync(Guid sweepEntryGuid)
        {
            var sweepEntry = default(SweepEntry);

            using (var conn = new MySqlConnection(_database.GetConnectionString()))
            {
                var command = conn.CreateCommand();
                command.CommandText = "sweep_entry_get_by_uid";
                command.CommandType = CommandType.StoredProcedure;
                command.AddParameter(nameof(sweepEntryGuid), sweepEntryGuid);

                await conn.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        sweepEntry = CreateSweepEntryFrom(reader);
                    }
                }
            }

            return sweepEntry;
        }

        private SweepEntry CreateSweepEntryFrom(MySqlDataReader reader)
        {
            return new SweepEntry
            {
                SweepEntryUid = reader.GetGuid("sweep_entry_uid"),
                SweepEntryId = reader.GetInt32("sweep_entry_id"),
                UserIdentifier = reader.GetString("user_identifier"),
                UserIdentifierType = (UserIdentifierType)Enum.Parse(typeof(UserIdentifierType), reader.GetString("user_identifier_type")),
                AcceptedTerms = reader.GetBoolean("accepted_terms"),
                CreatedDate = reader.GetDateTime("created_date"),
                Sweep = new Sweep
                {
                    SweepId = reader.GetInt32("sweep_id"),
                    SweepUid = reader.GetGuid("sweep_uid"),
                    CampaignUid = reader.GetGuid("campaign_uid"),
                    Name = reader.GetString("sweep_name"),
                    StartDate = reader.GetDateTime("start_date"),
                    EndDate = reader.GetDateTime("end_date"),
                    DrawDate = reader.GetDateTime("draw_date"),
                    CreatedDate = reader.GetDateTime("sweep_created_date")
                }
            };
        }
    }
}
