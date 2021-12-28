using MySqlConnector;
using Promo.Core;
using Promo.Core.Extensions;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Promo.Harvester.Infrastructure
{
    public interface IHarvesterRepository
    {
        Task CreateHarvestedRecordAsync(string json);
    }

    public class HarvesterRepository : IHarvesterRepository
    {
        private readonly IDatabase _database;

        public HarvesterRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task CreateHarvestedRecordAsync(string json)
        {
            using var conn = new MySqlConnection(_database.GetConnectionString());
            var command = conn.CreateCommand();
            command.CommandText = "harvested_record_create";
            command.CommandType = CommandType.StoredProcedure;
            command
                .AddParameter("harvestedUid", Guid.NewGuid())
                .AddParameter("harvestedValue", json);

            await conn.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
