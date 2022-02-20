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
			return _config.GetConnectionString(ConnectionStrings.Fulfillment);
		}

		public string GetGamesConnectionstring()
		{
			return _config.GetConnectionString(ConnectionStrings.Games);
		}

		public string GetMembershipConnectionString()
		{
			return _config.GetConnectionString(ConnectionStrings.Membership);
		}

		public string GetNotificationConnectionString()
		{
			return _config.GetConnectionString(ConnectionStrings.Notification);
		}
	}
}
