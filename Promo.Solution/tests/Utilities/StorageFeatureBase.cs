using hrvster.Extensions.Configuration;
using hrvster.Extensions.OptionsConfiguration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System;
using System.Data;
using TestUtilities.Mocks.Authentication;

namespace TestUtilities
{
    public abstract class StorageFeatureBase<TFeature> : FeatureBase<TFeature> where TFeature : class
    {
        public StorageFeatureBase() { Initialize(); }

        protected override Action<IServiceCollection> ConfigureServices =>
            services =>
            {
                services.AddLogging();

				//add the services here
				//services.AddFirebaseServices(Configuration);
                services.AddMediatorAndFluentValidation();
                services.AddDatabaseConfiguration(Configuration);
                services.AddCampaignRepositories();
                services.AddSweepstakesRepositories();
                services.AddAccountRepositories();
                services.AddEmailRepository();
                services.AddSingletons();
                services.AddSessionManager();
                services.AddHarvsterConfigurationOption(Configuration);
                services.AddSweepstakesServices();


				services.MockFirebaseAuthManager();
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
            command.CommandText = "DELETE FROM campaigns WHERE campaign_id > 0; " +
				"DELETE FROM accounts WHERE account_id > 0; " +
				"DELETE FROM emails where email_id > 0; " +
                "DELETE FROM user_activities WHERE activity_id > 0;" +
                "DELETE FROM emails WHERE email_id > 0;" +
                "DELETE FROM sweep WHERE sweep_id > 0;" +
                "DELETE FROM sweep_notifications WHERE sweep_notification_id > 0;";
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
