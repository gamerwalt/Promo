using hrvster.Extensions.Configuration;
using hrvster.Extensions.OptionsConfiguration;
using hrvster.Web.Mapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System;
using System.Data;
using TestUtilities.Extensions;
using TestUtilities.Mocks.Authentication;
using TestUtilities.Mocks.Notifications;
using TestUtilities.Mocks.Publishers;

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
                services.AddSweepstakeUtility();
                services.AddDrawSettingConfiguration(Configuration);
                services.AddRandomizer();
                services.AddAutoMapper(typeof(EntityMap));
                services.AddPublishers();
                services.AddTwitterClient(Configuration);
                services.AddSubscriptionRepositories();
                services.AddSubscriptionService();
                services.AddPaymentServices(Configuration);
                services.AddHttpContextAccessor();
                services.AddSubscriptionService();
                services.AddSubscriptionRepositories();
                services.AddBlackListedSweepsUserRepository();
                services.AddWatchTypeRepositories();
                services.AddHarvesterService();
                services.AddHarvesterServiceCommandRepository();

                services.MockFirebaseAuthManager();
                services.MockExternalClient();
                services.MockToastNotification();
                services.MockAuthenticiation();
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
                "DELETE FROM account_profiles WHERE profile_id > 0;" +
				"DELETE FROM emails where email_id > 0; " +
                "DELETE FROM user_activities WHERE activity_id > 0;" +
                "DELETE FROM emails WHERE email_id > 0;" +
                "DELETE FROM sweep WHERE sweep_id > 0;" +
                "DELETE FROM sweep_rules WHERE rule_id > 0;" +
                "DELETE FROM sweep_notifications WHERE sweep_notification_id > 0;" +
                "DELETE FROM sweep_winners WHERE sweep_winner_id > 0;" +
                "DELETE FROM sweep_prizes WHERE prize_id > 0;" +
                "DELETE FROM sweep_winners_backup WHERE sweep_winner_backup_id > 0;" +
                "DELETE FROM sweep_winners_unfulfilled WHERE sweep_winner_unfulfilled_id > 0;" +
                "DELETE FROM sweep_entry_rules WHERE entry_rule_id > 0;" +
                "DELETE FROM sweep_entry WHERE sweep_entry_id > 0;" +
                "DELETE FROM sweep_draw_rules WHERE sweep_draw_rule_id > 0;" +
                "DELETE FROM account_subscriptions WHERE subscription_id > 0;" +
                "DELETE FROM scheduled_sweep_rule_tweets WHERE scheduled_id > 0;";
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
