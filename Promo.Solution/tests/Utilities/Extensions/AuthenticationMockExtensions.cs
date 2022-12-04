using hrvster.Core.Authentication;
using Microsoft.Extensions.DependencyInjection;
using TestUtilities.Data;
using TestUtilities.Mocks.Authentication;

namespace TestUtilities.Extensions
{
    public static class AuthenticationMockExtensions
    {
        public static void MockAuthenticiation(this IServiceCollection services)
        {
            services.MockFirebaseAuthManager(mock =>
            {
                mock.SetupVerifyToken(true);
            });

            services.MockSessionManager(mock =>
            {
                mock.SetupContainsKey(SessionConstants.UserToken, true);

                mock.SetupGetValue(SessionConstants.UserToken, TestData.FirebaseToken);
                mock.SetupGetValue(SessionConstants.UserUid, TestData.UserUid);
                mock.SetupGetValue(SessionConstants.UserDisplayName, TestData.DisplayName);
                mock.SetupGetValue(SessionConstants.UserEmail, TestData.EmailAddress);
            });
        }
    }
}
