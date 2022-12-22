using Microsoft.Extensions.DependencyInjection;
using TestUtilities.Mocks.Subscriptions;

namespace TestUtilities.Extensions
{
	public static class SubscriptionMockExtensions
	{
		public static void MockIsSubscribedSubscription(this IServiceCollection services)
		{
			services.MockSubscription(mock =>
			{
				mock.MockIsSubscribed();
			});
		}
	}
}
