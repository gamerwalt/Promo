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
    public class SessionControllerTests : SessionControllerBaseTest, IClassFixture<Feature>
    {
        private readonly Feature _feature;

        public SessionControllerTests(Feature feature)
        {
            _feature = feature;
        }

        [Fact]
        public void Loading_The_SignInForm_Returns_An_OK_With_The_Word_Sign_In()
        {
            _feature
                .Initialize(additionalServiceConfiguration:
                services =>
                {
                    //services.MockProfileRepository(mock =>
                    //{
                    //    mock.GetRecordReturnsException(Guid.NewGuid(), hrvster.Models.ProfileType.Twitter);
                    //});

                    //we have to mock FirebaseAuthProvider here
                })
                .GiveACleanSlate()
                .WhenILoadTheSignInForm()
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, view =>
                {
                    view.Should().NotBeNull();
                    view.Should().Contain("Sign In");
                    view.Should().Contain("Sign up");
                });
        }

        [Fact]
        public void Supplying_Empty_Credentials_Should_Return_A_Validation_Error()
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
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, view =>
                {
                    view.Should().NotBeNull();
                    view.Should().Contain("must not be empty.");
                    view.Should().Contain("Error");
                });
        }

        [Fact]
        public void Supply_Valid_Credentials_Should_Return()
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
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, view =>
                {
                    view.Should().NotBeNull();
                });
        }
    }
}