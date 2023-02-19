using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Core;
using hrvster.Infrastructure.Harvester;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.Scheduling;
using hrvster.Infrastructure.Subscriptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class DatabaseAndRepositoryConfigurationExtensions
	{
		public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
			=> services.AddSingleton<IDatabase>(c =>
				{
					return new MySqlDatabase(DatabaseNames.Main, configuration, DatabaseNames.Harvester, DatabaseNames.Reporting);
				})
				.AddTransient<IActivityRepository, ActivityRepository>();

		public static IServiceCollection AddCampaignRepositories(this IServiceCollection services)
			=> services.AddTransient<ICampaignRepository, CampaignRepository>()
				.AddTransient<ICampaignTypesRepository, CampaignTypesRepository>()
				.AddTransient<IWatchTypeCampaignTypesRepository, WatchTypeCampaignTypesRepository>();

		public static IServiceCollection AddAccountRepositories(this IServiceCollection services)
			=> services.AddTransient<IAccountsRepository, AccountsRepository>()
				.AddTransient<IProfilesRepository, ProfilesRepository>()
				.AddTransient<IAccountUpdateHistoryRepository, AccountUpdateHistoryRepository>();

		public static IServiceCollection AddSubscriptionRepositories(this IServiceCollection services)
			=> services.AddTransient<IAccountSubscriptionsRepository, AccountSubscriptionsRepository>()
				.AddTransient<IPlansRepository, PlansRepository>();

		public static IServiceCollection AddSweepstakesRepositories(this IServiceCollection services)
			=> services.AddTransient<ISweepsRepository, SweepsRepository>()
				.AddTransient<ISweepRulesRepository, SweepRulesRepository>()
				.AddTransient<ISweepsEntriesRepository, SweepsEntriesRepository>()
				.AddTransient<ISweepEntryRulesRepository, SweepEntryRulesRepository>()
				.AddTransient<ISweepDrawRuleRepository, SweepDrawRuleRepository>()
				.AddTransient<ISweepWinnersRepository, SweepWinnersRepository>()
				.AddTransient<ISweepPrizesRepository, SweepPrizesRepository>()
				.AddTransient<ISweepTweetSchedulerRepository, SweepTweetSchedulerRepository>()
				.AddTransient<IScheduledSweepRuleTweetsRepository, ScheduledSweepRuleTweetsRepository>()
				.AddTransient<ISweepNotificationRepository, SweepNotificationRepository>();

		public static IServiceCollection AddWatchTypeRepositories(this IServiceCollection services)
			=> services.AddTransient<IWatchTypesRepository, WatchTypesRepository>()
				.AddTransient<IWatchTypeProvidersRepository, WatchTypeProvidersRepository>();

		public static IServiceCollection AddHarvesterAndReportingRepositories(this IServiceCollection services)
			=> services.AddTransient<IReportingRepository, ReportingRepository>()
				.AddTransient<IBlacklistedSweepsUsersRepository, BlacklistedSweepsUsersRepository>()
				.AddTransient<IHarvesterRepository, HarvesterRepository>()
				.AddTransient<IIncomingTweetsRepository, IncomingTweetsRepository>()
				.AddTransient<IUserLimiterRepository, UserLimiterRepository>();

		public static IServiceCollection AddBlackListedSweepsUserRepository(this IServiceCollection services)
			=> services.AddTransient<IBlacklistedSweepsUsersRepository, BlacklistedSweepsUsersRepository>();

		public static IServiceCollection AddServicesRepositories(this IServiceCollection services)
			=> services.AddTransient<IServicesRepository, ServicesRepository>();

		public static IServiceCollection AddEmailRepository(this IServiceCollection services)
			=> services
				.AddTransient<IMailRepository, MailRepository>();

		public static IServiceCollection AddFlutterwaveEventRepository(this IServiceCollection services)
			=> services.AddTransient<IFlutterwaveEventRepository, FlutterwaveEventRepository>()
				.AddTransient<IFlutterwaveAccountSubscriptionRepository, FlutterwaveAccountSubscriptionRepository>();

		public static IServiceCollection AddHarvesterServiceCommandRepository(this IServiceCollection services)
			=> services.AddTransient<IHarvesterServiceCommandRepository, HarvesterServiceCommandRepository>();

		public static IServiceCollection AddMatchGameRepositories(this IServiceCollection services)
			=> services.AddTransient<IMatchGamesRepository, MatchGamesRepository>()
				.AddTransient<IMatchImagesRepository, MatchImagesRepository>()
				.AddTransient<IMatchPrizesRepository, MatchPrizesRepository>()
				.AddTransient<IMatchBoardsRepository, MatchBoardsRepository>()
				.AddTransient<IMatchTilesRepository, MatchTilesRepository>();
	}
}
