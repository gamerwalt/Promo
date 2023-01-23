using hrvster.Core.Authentication;
using hrvster.Models;
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

            services.AddSingleton<HrvsterUser>();
            var user = services.BuildServiceProvider().GetRequiredService<HrvsterUser>();
            user.UserUid = TestData.AccountUid;
            user.EmailAddress = TestData.EmailAddress;
            user.Token = TestData.FirebaseToken;
            user.DisplayName = TestData.DisplayName;
            user.Locale = TestData.Locale;
            user.WindowsTimeZone = TestData.WindowsTimezone;
            user.IsDaylight = false;

            services.MockSessionManager(mock =>
            {
                mock.SetupContainsKey(SessionConstants.UserToken, true);

                mock.SetupGetValue(SessionConstants.UserToken, TestData.FirebaseToken);
                mock.SetupGetValue(SessionConstants.UserUid, TestData.UserUid);
                mock.SetupGetValue(SessionConstants.UserDisplayName, TestData.DisplayName);
                mock.SetupGetValue(SessionConstants.UserEmail, TestData.EmailAddress);
                mock.SetupGetValue(SessionConstants.IsDaylight, false.ToString());
                mock.SetupGetValue(SessionConstants.Locale, TestData.Locale);
                mock.SetupGetValue(SessionConstants.WindowsLocale, TestData.WindowsTimezone);

                mock.Setup(m => m.GetUserInformation())
                    .Returns(user);
            });
        }
    }
}
