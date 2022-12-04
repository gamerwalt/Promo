using hrvster.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using System;
using System.Data;

namespace TestUtilities
{
    public abstract class StorageFeatureBase<TFeature> : FeatureBase<TFeature> where TFeature : class
    {
        public StorageFeatureBase() { Initialize(); }

        protected override Action<IServiceCollection> ConfigureServices =>
            services =>
            {
                //add the services here
                services.AddFirebaseServices(Configuration);
            };

        protected virtual Action<IServiceCollection> AdditionalServices { get; } = null;

        public override void Dispose()
        {
            DeleteAllTestData();

            base.Dispose();
        }

        public void DeleteAllTestData()
        {
            var hrvsterConnectionString = Configuration["ConnectionStrings:hrvster"];
            var harvesterConnectionString = Configuration["ConnectionStrings:Harvester"];
            var reportingConnectionString = Configuration["ConnectionStrings:Reporting"];

            using var conn = new MySqlConnection(hrvsterConnectionString);
            var command = conn.CreateCommand();
            command.CommandText = "Delete FROM campaigns WHERE campaign_id > 0; Delete FROM accounts WHERE account_id > 0; Delete From emails where email_id > 0";
            command.CommandType = CommandType.Text;
            conn.Open();
            command.ExecuteNonQuery();

            using var harvesterConn = new MySqlConnection(harvesterConnectionString);
            var harvesterCommand = harvesterConn.CreateCommand();
            harvesterCommand.CommandText = "DELETE FROM harvested WHERE harvested_id > 0;";
            harvesterCommand.CommandType = CommandType.Text;
            harvesterConn.Open();
            harvesterCommand.ExecuteNonQuery();

            using var reportingConn = new MySqlConnection(reportingConnectionString);
            var reportingCommand = reportingConn.CreateCommand();
            reportingCommand.CommandText = "DELETE FROM sweep_entries WHERE id > 0;";
            reportingCommand.CommandType = CommandType.Text;
            reportingConn.Open();
            reportingCommand.ExecuteNonQuery();
        }
    }
}
