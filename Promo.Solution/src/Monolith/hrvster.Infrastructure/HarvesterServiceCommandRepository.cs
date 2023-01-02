using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface IHarvesterServiceCommandRepository
	{
		Task<HarvesterServiceCommandModel> GetMostRecent(CancellationToken cancellationToken);
		Task CreateAsync(HarvesterServiceCommandModel model, CancellationToken cancellationToken);
		Task DeleteAllAsync(CancellationToken cancellationToken);
	}

	public class HarvesterServiceCommandRepository : IHarvesterServiceCommandRepository
	{
		private readonly IDatabase _database;

		public HarvesterServiceCommandRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(HarvesterServiceCommandModel model, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "harvester_service_command_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("uid", model.Uid)
				.AddParameter("command", model.Command);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task DeleteAllAsync(CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "harvester_service_command_delete_all";
			command.CommandType = CommandType.StoredProcedure;

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<HarvesterServiceCommandModel> GetMostRecent(CancellationToken cancellationToken)
		{
			var harvesterServiceCommandModels = default(HarvesterServiceCommandModel);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "harvester_service_command_get_most_recent";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync(cancellationToken);
				using (var reader = await command.ExecuteReaderAsync(cancellationToken))
				{
					while (await reader.ReadAsync(cancellationToken))
					{
						harvesterServiceCommandModels = CreateFrom(reader);
					}
				}
			}

			return harvesterServiceCommandModels;
		}

		private HarvesterServiceCommandModel CreateFrom(MySqlDataReader reader)
		{
			return new HarvesterServiceCommandModel
			{
				Id = reader.GetInt32("id"),
				Uid = reader.GetGuid("uid"),
				Command = reader.GetString("command"),
				CreatedDate = reader.GetDateTime("created_date")
			};
		}
	}
}
