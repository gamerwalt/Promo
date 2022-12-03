using hrvster.Core.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;

namespace TestUtilities.Mocks.Authentication
{
    public static class MockFirebaseAuthManagerExtensions
    {
        public static IServiceCollection MockFirebaseAuthManager(
            this IServiceCollection services,
            Action<Mock<IFirebaseAuthManager>> mockSetup = null)
        {
            services.RemoveAll<IFirebaseAuthManager>();

            var mock = new Mock<IFirebaseAuthManager>();
            mockSetup?.Invoke(mock);

            services.AddSingleton(mock);
            services.AddSingleton(mock.Object);

            return services;
        }

        public static Mock<IFirebaseAuthManager> SetupVerifyToken(this Mock<IFirebaseAuthManager> mock, bool returnValue = true)
        {
            mock.Setup(f => f.VerifyToken(It.IsAny<string>()))
                .Returns(returnValue);

            return mock;
        }
    }
}
