using hrvster.Extensions.Configuration;
using hrvster.Extensions.OptionsConfiguration;
using hrvster.Web.Mapper;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using System;
using System.Data;
using TestUtilities.Extensions;
using TestUtilities.Mocks.Authentication;
using TestUtilities.Mocks.Clients;
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
				services.AddLogging()
					.AddMediatorAndFluentValidation()
					.AddDatabaseConfiguration(Configuration)
					.AddCampaignRepositories()
					.AddSweepstakesRepositories()
					.AddAccountRepositories()
					.AddEmailRepository()
					.AddSingletons()
					.AddSessionManager()
					.AddHarvsterConfigurationOption(Configuration)
					.AddSweepstakesServices()
					.AddSweepstakeUtility()
					.AddDrawSettingConfiguration(Configuration)
					.AddRandomizer()
					.AddAutoMapper(typeof(EntityMap))
					.AddPublishers()
					.AddTwitterClient(Configuration)
					.AddSubscriptionRepositories()
					.AddSubscriptionService()
					.AddPaymentServices(Configuration)
					.AddHttpContextAccessor()
					.AddSubscriptionService()
					.AddSubscriptionRepositories()
					.AddBlackListedSweepsUserRepository()
					.AddWatchTypeRepositories()
					.AddHarvesterService()
					.AddHarvesterServiceCommandRepository()
					.AddFlutterWaveConfigurations(Configuration)
					.AddHttpClient();

				services.MockFirebaseAuthManager()
					.MockExternalClient()
					.MockPaymentAdaptor()
					.MockToastNotification()
					.MockAuthenticiation();
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
				"DELETE FROM scheduled_sweep_rule_tweets WHERE scheduled_id > 0;" +
				"DELETE FROM sweep_entry WHERE sweep_entry_id > 0;";
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
