using hrvster.Core.Subscriptions;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class SubscriptionConfigurationExtensions
	{
		public static IServiceCollection AddSubscriptionService(this IServiceCollection services)
			=> services.AddTransient<ISubscriptionService, SubscriptionService>();
	}
}
