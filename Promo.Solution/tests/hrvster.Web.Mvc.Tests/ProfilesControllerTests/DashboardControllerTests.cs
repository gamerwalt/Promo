using FluentAssertions;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests;
using hrvster.Web.Mvc.Common;
using hrvster.Web.Mvc.Common.Steps;
using hrvster.Web.Mvc.Common.Steps.Authentication;
using System.Net;
using TestUtilities.Common;
using TestUtilities.Data;
using TestUtilities.Mocks.Authentication;
using Xunit;

namespace ControllerTests
{
    [Collection("Sequential")]
    public class DashboardControllerTests : IClassFixture<Feature>
    {
        private readonly Feature _feature;

        public DashboardControllerTests(Feature feature)
        {
            _feature = feature;
        }

        [Fact]
        public void When_Not_Authorized_We_Are_Redirected_To_The_Sign_In_Page()
        {
            _feature
                .GiveACleanSlate()
                .Initialize()
                .WhenILoadTheDashboardPage()
                .ThenTheResponseShouldMatch(HttpStatusCode.Redirect)
                .ThenTheRedirectedLocationShouldContain(RouteList.SignIn);
        }

        [Fact]
        public void When_Authorized_We_Load_The_Dashboard_Route()
        {
            var form = new SigninForm() { Email = TestData.EmailAddress, Password = TestData.Password };

            _feature
                .GiveACleanSlate()
                .Initialize(additionalServiceConfiguration:
                services =>
                {
                    services.MockFirebaseAuthManager(mock =>
                    {
                        mock.SetupVerifyToken(true);
                    });

                    services.MockFirebaseAuthProvider(mock =>
                    {
                        mock.SetupSignInWithEmailAndPasswordAsyncSuccess(TestData.EmailAddress, TestData.Password);
                    });

                    services.MockSessionManager(mock =>
                    {
                        mock.SetupContainsKey(SessionConstants.UserToken, true);

                        mock.SetupGetValue(SessionConstants.UserToken, TestData.FirebaseToken);
                        mock.SetupGetValue(SessionConstants.UserUid, TestData.UserUid);
                        mock.SetupGetValue(SessionConstants.UserDisplayName, TestData.DisplayName);
                        mock.SetupGetValue(SessionConstants.UserEmail, TestData.EmailAddress);
                    });
                })
                .WhenILoadTheSignInForm()
                .AndExtractTheAntiForgeryValues()
                .WhenISignInUsing(form)
                .WhenILoadTheDashboardPage()
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, view =>
                {
                    view.Should().NotBeNull();
                    view.Should().Contain(TestData.DisplayName);
                    view.Should().Contain("Dashboard");
                });
        }
    }
}
