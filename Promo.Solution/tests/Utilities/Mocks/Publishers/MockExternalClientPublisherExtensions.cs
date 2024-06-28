using hrvster.Core.Clients.Common;
using hrvster.Core.Clients.Publishers;
using hrvster.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Tweetinvi.WebLogic;

namespace TestUtilities.Mocks.Publishers
{
    public static class MockExternalClientPublisherExtensions
    {
        public static IServiceCollection MockExternalClient(
            this IServiceCollection services,
            Action<Mock<IExternalClient>> mockSetup = null)
        {
            services.RemoveAll<IExternalClient>();

            var mock = new Mock<IExternalClient>();
            mockSetup?.Invoke(mock);

            services.AddSingleton(mock);
            services.AddSingleton(mock.Object);

            return services;
        }
    }

    public static class MockTwitterClientExtensions
    {
        public static IServiceCollection MockTwitterClient(
            this IServiceCollection services,
            ILoggerFactory loggerFactory,
            TwitterAccount twitterAccount)
        {
            // Remove existing TwitterClient registrations
            services.RemoveAll<TwitterClient>();

            // Create a mock of TwitterClient with the required dependencies
            var mock = new Mock<TwitterClient>(loggerFactory, twitterAccount);

            // Set up the necessary methods and properties of the mock
            mock.SetupPublishAsync();
            mock.SetupPublishResponse();

            return services;
        }

        public static Mock<TwitterClient> SetupPublishAsync(this Mock<TwitterClient> mock)
        {
            mock.Setup(x => x.PublishAsync(It.IsAny<Message>()))
                .ReturnsAsync("896986896");

            return mock;
        }

        public static Mock<TwitterClient> SetupPublishResponse(this Mock<TwitterClient> mock)
        {
            mock.Setup(x => x.PublishResponse(It.IsAny<Message>(), It.IsAny<string>()))
                .ReturnsAsync("6797634");

            return mock;
        }
    }
}
