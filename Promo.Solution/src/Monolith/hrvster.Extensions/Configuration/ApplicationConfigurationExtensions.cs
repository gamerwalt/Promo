using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.Harvesters.Twitter;
using hrvster.ApplicationServices.Mails;
using hrvster.ApplicationServices.Processors;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class ApplicationConfigurationExtensions
	{
		public static IServiceCollection AddApplicationCronServices(this IServiceCollection services)
			=> services
				.AddTransient<ITwitterHarvesterService, TwitterHarvesterService>()
				.AddTransient<ITweetProcessor, TweetProcessor>()
				.AddTransient<IDrawService, DrawService>()
				.AddTransient<IMailService, MailService>()
				.AddTransient<IAccountVerificationService, AccountVerificationService>()
				.AddUserRateLimiterService();

		public static IServiceCollection AddRandomizer(this IServiceCollection services)
			=> services
				.AddSingleton<IRandomizer, Randomizer>();

		public static IServiceCollection AddUserRateLimiterService(this IServiceCollection services)
			=> services.AddTransient<IUserRateLimiterService, UserRateLimiterService>();


	}
}
