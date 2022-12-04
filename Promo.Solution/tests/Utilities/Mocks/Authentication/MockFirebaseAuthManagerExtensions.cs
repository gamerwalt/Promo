using FirebaseAdmin.Auth;
using hrvster.Core.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;
using TestUtilities.Data;

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

        public static Mock<IFirebaseAuthManager> SetupCreateuserSuccess(this Mock<IFirebaseAuthManager> mock)
        {
            mock.Setup(f => f.CreateUserAsync(It.IsAny<UserRecordArgs>()))
                .ReturnsAsync(default(UserRecord));

            return mock;
        }

        public static Mock<IFirebaseAuthManager> SetupGenerateEmailVerificationLinkAsync(this Mock<IFirebaseAuthManager> mock)
        {
            mock.Setup(f => f.GenerateEmailVerificationLinkAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.VerifyToken);

            return mock;
        }
    }
}
