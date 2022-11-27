using Firebase.Auth;
using hrvster.Infrastructure.Accounts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities.Data;

namespace TestUtilities.Mocks.Authentication
{
    public static class MockFirebaseAuthProviderExtensions
    {
        public static IServiceCollection MockFirebaseAuthProvider(
            this IServiceCollection services,
            Action<Mock<IFirebaseAuthProvider>> mockSetup = null)
        {
            services.RemoveAll<IFirebaseAuthProvider>();

            var mock = new Mock<IFirebaseAuthProvider>();
            mockSetup?.Invoke(mock);

            services.AddSingleton(mock);
            services.AddSingleton(mock.Object);

            return services;
        }

        public static Mock<IFirebaseAuthProvider> SetupSignInWithEmailAndPasswordAsyncSuccess(
            this Mock<IFirebaseAuthProvider> mock,
            string email,
            string password)
        {
            mock.Setup(
                f => f.SignInWithEmailAndPasswordAsync(
                    It.Is<string>(c => c.Equals(email)),
                    It.Is<string>(c => c.Equals(password)),
                    It.IsAny<string>())
                ).ReturnsAsync(new FirebaseAuthLink(mock.Object, new FirebaseAuth())
                {
                    FirebaseToken = TestData.FirebaseToken,
                    ExpiresIn = TestData.ExpiresIn,
                    RefreshToken = TestData.RefreshToken,
                    User = new User
                    {
                        Email = email,
                        DisplayName = email,
                        IsEmailVerified = true,
                        LocalId = TestData.UserUid,
                    }
                });

            return mock;
        }

        public static Mock<IFirebaseAuthProvider> SetupSignInWithEmailAndPasswordAsyncFailure(
            this Mock<IFirebaseAuthProvider> mock,
            string email,
            string password)
        {
            mock.Setup(
                f => f.SignInWithEmailAndPasswordAsync(
                    It.Is<string>(c => c.Equals(email)),
                    It.Is<string>(c => c.Equals(password)),
                    It.IsAny<string>())
                ).Throws(new FirebaseAuthException(string.Empty, string.Empty, TestData.AuthError, new Exception()));

            return mock;
        }
    }
}
