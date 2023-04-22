using hrvster.Core.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class AuthenticationWatchProviderConfigurationExtensions
	{
		public static IServiceCollection AddAuthenticationForWatchProviders(this IServiceCollection services)
			=> services.AddTransient<IWatchTypeProvider, WatchTypeProvider>()
				.AddTransient<ITwitterWatchTypeProvider, TwitterWatchTypeProvider>()
				.AddTransient<IFacebookWatchTypeProvider, FacebookWatchTypeProvider>();
	}
}
