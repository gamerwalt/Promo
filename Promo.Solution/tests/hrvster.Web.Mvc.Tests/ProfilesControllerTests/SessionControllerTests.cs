using FluentAssertions;
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
    public class SessionControllerTests : IClassFixture<Feature>
    {
        private readonly Feature _feature;

        public SessionControllerTests(Feature feature)
        {
            _feature = feature;
        }

        [Fact]
        public void When_I_Load_The_SignIn_Page_We_See_The_Proper_Texts()
        {
            _feature
                .GiveACleanSlate()
                .Initialize(additionalServiceConfiguration:
                services =>
                {
                    //services.MockFirebaseAuthProvider(mock =>
                    //{
                    //    mock.SetupSignInWithEmailAndPasswordAsyncSuccess(TestData.EmailAddress, TestData.Password);
                    //});
                })
                .WhenILoadTheSignInForm()
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, view =>
                {
                    view.Should().NotBeNullOrEmpty();
                    view.Should().Contain("Sign In");
                    view.Should().Contain("Email");
                    view.Should().Contain("Password");
                });
        }

        [Fact]
        public void When_I_Supply_Empty_Credentials_I_Should_Be_Redirected_To_The_SignInPage()
        {
            var form = new SigninForm() { Email = string.Empty, Password = string.Empty };

            _feature
                .GiveACleanSlate()
                .Initialize(additionalServiceConfiguration:
                services =>
                {
                    //services.MockProfileRepository(mock =>
                    //{
                    //    mock.GetRecordReturnsException(Guid.NewGuid(), hrvster.Models.ProfileType.Twitter);
                    //});

                    //we have to mock FirebaseAuthProvider here
                })
                .WhenILoadTheSignInForm()
                .AndExtractTheAntiForgeryValues()
                .WhenISignInUsing(form)
                .ThenTheResponseShouldReturn(HttpStatusCode.Redirect)
                .ThenTheRedirectedLocationShouldBe(RouteList.SignIn);
        }

        [Fact]
        public void When_I_Supply_InValid_Credentials_I_Should_Be_Redirected_To_The_SignInPage()
        {
            var form = new SigninForm() { Email = TestData.WrongEmailAddress, Password = TestData.WrongPassword };

            _feature
                .GiveACleanSlate()
                .Initialize(additionalServiceConfiguration:
                services =>
                {
                    services.MockFirebaseAuthProvider(mock =>
                    {
                        mock.SetupSignInWithEmailAndPasswordAsyncFailure(TestData.WrongEmailAddress, TestData.WrongPassword);
                    });
                })
                .WhenILoadTheSignInForm()
                .AndExtractTheAntiForgeryValues()
                .WhenISignInUsing(form)
                .ThenTheResponseShouldReturn(HttpStatusCode.Redirect)
                .ThenTheRedirectedLocationShouldBe(RouteList.SignIn);
        }

        [Fact]
        public void When_I_Supply_Valid_Credentials_I_Should_Be_Redirected_To_The_Dashboard()
        {
            var form = new SigninForm() { Email = TestData.EmailAddress, Password = TestData.Password };

            _feature
                .GiveACleanSlate()
                .Initialize(additionalServiceConfiguration:
                services =>
                {
                    services.MockFirebaseAuthProvider(mock =>
                    {
                        mock.SetupSignInWithEmailAndPasswordAsyncSuccess(TestData.EmailAddress, TestData.Password);
                    });
                })
                .WhenILoadTheSignInForm()
                .AndExtractTheAntiForgeryValues()
                .WhenISignInUsing(form)
                .ThenTheResponseShouldReturn(HttpStatusCode.Moved)
                .ThenTheRedirectedLocationShouldBe(RouteList.Dashboard);
        }
    }
}
