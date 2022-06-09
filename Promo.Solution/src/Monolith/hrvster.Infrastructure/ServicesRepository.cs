using hrvster.Models.Services;
using MySqlConnector;
using System.Data;

namespace hrvster.Infrastructure
{
	public interface IServicesRepository
	{
		ServiceContainer GetServicesEnabledAsync();
	}

	public class ServicesRepository : IServicesRepository
	{
		private readonly IDatabase _database;

		public ServicesRepository(IDatabase database)
		{
			_database = database;
		}

		public ServiceContainer GetServicesEnabledAsync()
		{
			var serviceContainer = new ServiceContainer();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "cron_services_enabled_get";
				command.CommandType = CommandType.StoredProcedure;

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					serviceContainer.Services.Add(CreateServiceFrom(reader));
				}
			}

			return serviceContainer;
		}

		private Service CreateServiceFrom(MySqlDataReader reader)
		{
			return new Service
			{
				Name = reader.GetString("name"),
				FQN = reader.GetString("fqn"),
				Enabled = reader.GetBoolean("enabled")
			};
		}
	}
}
