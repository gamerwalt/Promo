using Microsoft.Extensions.Configuration;
using System;

namespace Promo.Core
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

		public string GetFulfillmentConnectionString()
		{
			return _config.GetConnectionString(Constants.FulfillmentConnectionString);
		}

		public string GetGamesConnectionstring()
		{
			return _config.GetConnectionString(Constants.GamesConnectionString);
		}

		public string GetHarvesterConnectionString()
		{
			return _config.GetConnectionString(Constants.HarvesterConnectionString);
		}

		public string GetMembershipConnectionString()
		{
			return _config.GetConnectionString(Constants.MembershipConnectionString);
		}

		public string GetNotificationConnectionString()
		{
			return _config.GetConnectionString(Constants.NotificationConnectionString);
		}
	}
}
