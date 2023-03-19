using hrvster.ApplicationServices;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class ProvidersConfigurationExtension
	{
		public static IServiceCollection AddProvidersService(this IServiceCollection services)
		{
			services.AddSingleton<IProvidersService, ProvidersService>();

			return services;
		}
	}
}
