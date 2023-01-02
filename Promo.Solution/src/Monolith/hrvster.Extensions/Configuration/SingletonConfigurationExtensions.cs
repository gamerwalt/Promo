using hrvster.Core;
using hrvster.Core.Subscriptions;
using hrvster.Models;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class SingletonConfigurationExtensions
	{
		public static IServiceCollection AddSingletons(this IServiceCollection services)
			=> services.AddHrvsterUser()
				.AddLinkBuilder()
				.AddPrizePositionFinder();

		public static IServiceCollection AddHrvsterUser(this IServiceCollection services)
			=> services.AddTransient<HrvsterUser>();

		public static IServiceCollection AddLinkBuilder(this IServiceCollection services)
			=> services.AddSingleton<LinkBuilder>();

		public static IServiceCollection AddPrizePositionFinder(this IServiceCollection services)
			=> services.AddSingleton<PrizePositionFinder>();
	}
}
