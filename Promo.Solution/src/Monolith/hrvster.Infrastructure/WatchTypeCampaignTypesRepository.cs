using hrvster.Models;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface IWatchTypeCampaignTypesRepository
	{
		Task<IEnumerable<WatchTypeCampaignType>> GetWatchTypeCampaignTypesAsync();
	}

	public class WatchTypeCampaignTypesRepository : IWatchTypeCampaignTypesRepository
	{
		private readonly IDatabase _database;
		public WatchTypeCampaignTypesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<IEnumerable<WatchTypeCampaignType>> GetWatchTypeCampaignTypesAsync()
		{
			var results = new List<WatchTypeCampaignType>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "watch_type_campaign_types_get";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					results.Add(CreateRecordFrom(reader));
				}
			}

			return results;
		}

		private WatchTypeCampaignType CreateRecordFrom(MySqlDataReader reader)
		{
			return new WatchTypeCampaignType
			{
				WatchTypeCode = reader.GetString("watch_type_code"),
				WatchTypeName = reader.GetString("watch_type_name"),
				CampaignTypeUid = reader.GetGuid("campaign_type_uid")
			};
		}
	}
}
