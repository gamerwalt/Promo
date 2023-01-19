using hrvster.Core.Clients.Common;
using hrvster.Core.Clients.Publishers;
using hrvster.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System;

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
            services.RemoveAll<TwitterClient>();

            var mock = new Mock<TwitterClient>(loggerFactory, twitterAccount);

            mock.Setup(x => x.PublishAsync(It.IsAny<Message>()))
                .ReturnsAsync("896986896");
            mock.Setup(x => x.PublishResponse(It.IsAny<Message>(), It.IsAny<string>()))
                .ReturnsAsync("6797634");

            services.AddSingleton(mock);
            services.AddSingleton(mock.Object);

            return services;
        }
    }
}
