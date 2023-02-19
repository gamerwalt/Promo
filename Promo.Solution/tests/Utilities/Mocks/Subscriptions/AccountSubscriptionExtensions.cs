using hrvster.ApplicationServices.Subscription;
using hrvster.Models;
using hrvster.Models.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;
using TestUtilities.Data;

namespace TestUtilities.Mocks.Subscriptions
{
    public static class AccountSubscriptionExtensions
    {
        public static IServiceCollection MockAccountSubscriptionService(
            this IServiceCollection services,
            Action<Mock<IAccountSubscriptionService>> mockSetup = null)
        {
            services.RemoveAll<IAccountSubscriptionService>();

            var mock = new Mock<IAccountSubscriptionService>();
            mockSetup?.Invoke(mock);

            services.AddSingleton(mock);
            services.AddSingleton(mock.Object);

            return services;
        }

        public static Mock<IAccountSubscriptionService> SetupGetAccountSubscription(
            this Mock<IAccountSubscriptionService> mock)
        {
            var accountSubscription = new AccountSubscription
            {
                Plan = new Plan
                {
                    PlanUid = TestData.BasicPlanUid,
                    PromotionsPerMonth = 1
                },
                Account = new Account(),
                SubscriptionStatus = SubscriptionStatus.Subscribed,
                StartDate = DateTime.Now,
                StartDateUtc = DateTime.UtcNow,
                EndDate = DateTime.Now.AddDays(30),
                EndDateUtc = DateTime.UtcNow.AddDays(30)
            };

			mock.Setup(x => x.GetAccountSubscriptionAsync(It.IsAny<Guid>()))
                .ReturnsAsync(accountSubscription);

            mock.Setup(x => x.GetAccountSubscription(It.IsAny<Guid>()))
                .Returns(accountSubscription);

            return mock;
        }
    }
}
