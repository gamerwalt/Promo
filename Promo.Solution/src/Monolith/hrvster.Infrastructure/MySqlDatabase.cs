using Microsoft.Extensions.Configuration;
using System;

namespace hrvster.Infrastructure
{
	public class MySqlDatabase : IDatabase
	{
		private readonly string ConnectionString;
		private readonly string ConnectionName;

		private readonly IConfiguration _config;
		public MySqlDatabase(string connectionName, IConfiguration config)
		{
			if (string.IsNullOrWhiteSpace(connectionName)) throw new ArgumentNullException(nameof(connectionName));
			if (config is null) throw new ArgumentNullException(nameof(config));

			ConnectionName = connectionName;
			_config = config;

			if (string.IsNullOrWhiteSpace(_config.GetConnectionString(connectionName))) throw new Exception("Connection string needs to be set.");

			ConnectionString = _config.GetConnectionString(connectionName);
		}

		public void DeleteTestData()
		{
			throw new Exception("Need To connect and delete the database data");
		}

		public string GetConnectionName()
		{
			return ConnectionName;
		}

		public string GetConnectionString()
		{
			return ConnectionString;
		}
	}
}
