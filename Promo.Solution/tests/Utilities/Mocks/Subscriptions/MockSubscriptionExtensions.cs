using hrvster.ApplicationServices.Subscription;
using hrvster.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;

namespace TestUtilities.Mocks.Subscriptions
{
	public static class MockSubscriptionExtensions
	{
		public static IServiceCollection MockSubscription(
			this IServiceCollection services,
			Action<Mock<ISubscriptionService>> mockSetup = null)
		{
			services.RemoveAll<ISubscriptionService>();

			var mock = new Mock<ISubscriptionService>();
			mockSetup?.Invoke(mock);

			services.AddSingleton(mock);
			services.AddSingleton(mock.Object);

			return services;
		}

		public static Mock<ISubscriptionService> MockIsNotSubscribed(this Mock<ISubscriptionService> mock)
		{
			mock.Setup(s => s.IsAccountSubscribed(It.IsAny<Guid>()))
				.Returns(SubscriptionStatus.UnSubscribed);

			return mock;
		}

		public static Mock<ISubscriptionService> MockIsSubscribed(this Mock<ISubscriptionService> mock)
		{
			mock.Setup(s => s.IsAccountSubscribed(It.IsAny<Guid>()))
				.Returns(SubscriptionStatus.Subscribed);

			return mock;
		}
	}
}
