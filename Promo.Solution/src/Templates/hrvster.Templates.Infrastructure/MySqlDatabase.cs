using Microsoft.Extensions.Configuration;

namespace hrvster.Templates.Infrastructure
{
	public class MySqlDatabase : IDatabase
	{
		private readonly string ConnectionString;
		private readonly IConfiguration _config;

		public MySqlDatabase(string mainConnectionName, IConfiguration config)
		{
			if (string.IsNullOrWhiteSpace(mainConnectionName)) throw new ArgumentNullException(nameof(mainConnectionName));
			if (config is null) throw new ArgumentNullException(nameof(config));

			_config = config;

			if (string.IsNullOrWhiteSpace(_config.GetConnectionString(mainConnectionName))) throw new Exception("Connection string needs to be set.");
		}

		public string GetConnectionString()
		{
			return ConnectionString;
		}
	}
}
