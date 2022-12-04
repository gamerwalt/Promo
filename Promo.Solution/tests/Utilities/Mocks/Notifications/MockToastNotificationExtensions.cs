using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NToastNotify;
using System;

namespace TestUtilities.Mocks.Notifications
{
    public static class MockToastNotificationExtensions
    {
        public static IServiceCollection MockToastNotification(
            this IServiceCollection services,
            Action<Mock<IToastNotification>> mockSetup = null)
        {
            services.RemoveAll<IToastNotification>();

            var mock = new Mock<IToastNotification>();
            mockSetup?.Invoke(mock);

            services.AddSingleton(mock);
            services.AddSingleton(mock.Object);

            return services;
        }

        public static Mock<IToastNotification> SetupAddSuccessToastMessage(this Mock<IToastNotification> mock, string message)
        {
            mock.Setup(t => t.AddSuccessToastMessage(It.Is<string>(s => s.Equals(message)), It.IsAny<LibraryOptions>()));

            return mock;
        }
    }
}
