using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Caching;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.Communications;
using hrvster.ApplicationServices.Mails;
using hrvster.ApplicationServices.Processors;
using hrvster.ApplicationServices.Reporting;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Company;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class ApplicationConfigurationExtensions
	{
		public static IServiceCollection AddApplicationCronServices(this IServiceCollection services)
			=> services
				.AddTransient<ITweetProcessor, TweetProcessor>()
				.AddTransient<IDrawService, DrawService>()
				.AddTransient<IMailService, MailService>()
				.AddTransient<ICommunicationService, CommunicationService>()
				.AddTransient<IAccountVerificationService, AccountVerificationService>()
				.AddGameService()
				.AddReportingService()
				.AddUserRateLimiterService();

		public static IServiceCollection AddRandomizer(this IServiceCollection services)
			=> services
				.AddSingleton<IRandomizer, Randomizer>();

		public static IServiceCollection AddUserRateLimiterService(this IServiceCollection services)
			=> services.AddTransient<IUserRateLimiterService, UserRateLimiterService>();

		public static IServiceCollection AddSweepstakesServices(this IServiceCollection services)
			=> services.AddTransient<ISweepstakesService, SweepstakesService>()
				.AddTransient<IDrawService, DrawService>()
				.AddSweepstakeUtility();

		public static IServiceCollection AddAccountsService(this IServiceCollection services)
			=> services.AddTransient<IAccountsService, AccountsService>()
				.AddTransient<IAccountCompanyService, AccountCompanyService>()
				.AddTransient<ICompanyImagesService, CompanyImagesService>()
				.AddTransient<ICompanyImagesRepository, CompanyImagesRepository>()
				.AddTransient<IAccountCompanyRepository, AccountCompanyRepository>()
				.AddTransient<IAccountsRepository, AccountsRepository>();

		public static IServiceCollection AddSweepstakeUtility(this IServiceCollection services)
			=> services.AddTransient<SweepstakeUtility>();

		public static IServiceCollection AddCampaignService(this IServiceCollection services)
			=> services.AddTransient<ICampaignService, CampaignService>();

		public static IServiceCollection AddGameService(this IServiceCollection services)
			=> services.AddTransient<IGameService, GameService>();

		public static IServiceCollection AddCacheService(this IServiceCollection services)
			=> services.AddSingleton<ICacheDataService, RedisCacheService>();

		public static IServiceCollection AddSignatureUtility(this IServiceCollection services)
			=> services.AddScoped<SignatureUtility>();

		public static IServiceCollection AddReportingService(this IServiceCollection services)
			=> services.AddTransient<IReportingService, ReportingService>();
	}
}
