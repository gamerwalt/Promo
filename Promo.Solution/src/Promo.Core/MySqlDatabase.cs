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
