using hrvster.Core.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;

namespace TestUtilities.Mocks.Authentication
{
    public static class MockSessionManagerExtensions
    {
        public static IServiceCollection MockSessionManager(
            this IServiceCollection services,
            Action<Mock<ISessionManager>> mockSetup = null)
        {
            services.RemoveAll<ISessionManager>();

            var mock = new Mock<ISessionManager>();
            mockSetup?.Invoke(mock);

            services.AddSingleton(mock);
            services.AddSingleton(mock.Object);

            return services;
        }

        public static Mock<ISessionManager> SetupContainsKey(this Mock<ISessionManager> mock, string keyName, bool value)
        {
            mock.Setup(s => s.ContainsKey(It.Is<string>(c => c.Equals(keyName))))
                .Returns(value);

            return mock;
        }

        public static Mock<ISessionManager> SetupGetValue(this Mock<ISessionManager> mock, string keyName, string valueToGet)
        {
            mock.Setup(s => s.GetValue(It.Is<string>(c => c.Equals(keyName))))
                .Returns(valueToGet);

            return mock;
        }

        public static Mock<ISessionManager> SetupSetKeyValue(this Mock<ISessionManager> mock, string keyName, string valueToSet)
        {
            mock.Setup(s => s.SetKeyValue(It.IsAny<string>(), It.IsAny<string>()));

            return mock;
        }
    }
}
