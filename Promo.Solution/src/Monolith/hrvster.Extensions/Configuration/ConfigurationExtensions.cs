using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Harvester;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace hrvster.Extensions.Configuration
{
	public static class ConfigurationExtensions
	{
		public static IServiceCollection AddServiceContainer(this IServiceCollection services, ServiceContainer serviceContainer)
			=> services.AddSingleton(c => BuildServiceContainer(c, serviceContainer));
		

		private static ServiceContainer BuildServiceContainer(IServiceProvider serviceProvider, ServiceContainer serviceContainer)
			=> serviceContainer;

		public static IServiceCollection AddServicesRepositories(this IServiceCollection services)
			=> services.AddTransient<IServicesRepository, ServicesRepository>();

		public static IServiceCollection AddSweepsRepositories(this IServiceCollection services)
			=> services
				.AddTransient<ISweepsRepository, SweepsRepository>()
				.AddTransient<ISweepRulesRepository, SweepRulesRepository>()
				.AddTransient<ISweepEntryRulesRepository, SweepEntryRulesRepository>()
				.AddTransient<ISweepPrizesRepository, SweepPrizesRepository>()
				.AddTransient<ISweepsEntriesRepository, SweepsEntriesRepository>();

		public static IServiceCollection AddScheduledRepositories(this IServiceCollection services)
			=> services
				.AddTransient<IScheduledSweepRuleTweetsRepository, ScheduledSweepRuleTweetsRepository>();

		public static IServiceCollection AddAccountRepositories(this IServiceCollection services)
			=> services
				.AddTransient<IProfilesRepository, ProfilesRepository>()
				.AddTransient<IAccountsRepository, AccountsRepository>();

		public static IServiceCollection AddBlacklistedRepository(this IServiceCollection services)
			=> services
				.AddTransient<IBlacklistedSweepsUsersRepository, BlacklistedSweepsUsersRepository>();

		public static IServiceCollection AddHarvesterAndReportingRepositories(this IServiceCollection services)
			=> services
				.AddTransient<IHarvesterRepository, HarvesterRepository>()
				.AddTransient<IReportingRepository, ReportingRepository>()
				.AddTransient<IIncomingTweetsRepository, IncomingTweetsRepository>();

		public static IServiceCollection AddEmailRepository(this IServiceCollection services)
			=> services
				.AddTransient<IMailRepository, MailRepository>();
	}
}
