using Microsoft.Extensions.Configuration;
using System;

namespace hrvster.Infrastructure
{
	public class MySqlDatabase : IDatabase
	{
		private readonly string ConnectionString;
		private readonly string HarvesterConnectionString;
		private readonly string ReportingConnectionString;

		private readonly string ConnectionName;
		private readonly IConfiguration _config;

		public MySqlDatabase(
			string mainConnectionName, 
			IConfiguration config, 
			string harvesterConnectionName = "", 
			string reportingConnectioName = "")
		{
			if (string.IsNullOrWhiteSpace(mainConnectionName)) throw new ArgumentNullException(nameof(mainConnectionName));
			if (config is null) throw new ArgumentNullException(nameof(config));

			ConnectionName = mainConnectionName;
			_config = config;

			if (string.IsNullOrWhiteSpace(_config.GetConnectionString(mainConnectionName))) throw new Exception("Connection string needs to be set.");

			ConnectionString = _config.GetConnectionString(mainConnectionName);
			if(!string.IsNullOrWhiteSpace(harvesterConnectionName))
				HarvesterConnectionString = _config.GetConnectionString(harvesterConnectionName);
			if(!string.IsNullOrWhiteSpace(reportingConnectioName))
				ReportingConnectionString = _config.GetConnectionString(reportingConnectioName);
		}

		public string GetHarvesterConnectionString() => HarvesterConnectionString;
		public string GetReportingConnectionString() => ReportingConnectionString;

		public string GetConnectionName()
		{
			return ConnectionName;
		}

		public string GetConnectionString()
		{
			return ConnectionString;
		}

		public void DeleteTestData()
		{
			throw new Exception("Need To connect and delete the database data");
		}
	}
}
