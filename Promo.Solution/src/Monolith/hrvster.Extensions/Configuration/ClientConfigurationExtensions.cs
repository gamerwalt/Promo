using hrvster.Core.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class ClientConfigurationExtensions
	{
		public static IServiceCollection AddHttpClients(this IServiceCollection services)
			=> services.AddIdentityClient();

		public static IServiceCollection AddIdentityClient(this IServiceCollection services)
		{
			services.AddHttpClient<IIdentityClient, IdentityClient>();

			return services;
		}
	}
}
