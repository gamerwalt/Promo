using hrvster.Models;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Core
{
	public interface IWatchTypesRepository
	{
		Task<IEnumerable<WatchType>> GetWatchTypesAsync();
	}
	public class WatchTypesRepository : IWatchTypesRepository
	{
		private readonly IDatabase _database;
		public WatchTypesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<IEnumerable<WatchType>> GetWatchTypesAsync()
		{
			var watchTypes = new List<WatchType>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "watch_types_get";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					watchTypes.Add(CreateRecordFrom(reader));
				}
			}

			return watchTypes;
		}

		private WatchType CreateRecordFrom(MySqlDataReader reader)
		{
			return new WatchType
			{
				WatchTypeId = reader.GetInt32("watch_type_id"),
				WatchTypeCode = reader.GetString("watch_type_code"),
				WatchTypeName = reader.GetString("watch_type_name")
			};
		}
	}
}
