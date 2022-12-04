using FluentAssertions;
using hrvster.Core;
using hrvster.Web.Mvc.Common;
using hrvster.Web.Mvc.Common.Steps;
using hrvster.Web.Mvc.Common.Steps.Authentication;
using Moq;
using NToastNotify;
using System.Net;
using TestUtilities.Builders.Resources;
using TestUtilities.Common;
using TestUtilities.Common.Steps;
using TestUtilities.Data;
using TestUtilities.Mocks.Authentication;
using TestUtilities.Mocks.Notifications;
using Xunit;

namespace ControllerTests
{
    [Collection("Sequential")]
    public class RegisterControllerTests : IClassFixture<Feature>
    {
        private readonly Feature _feature;

        public RegisterControllerTests(Feature feature)
            => _feature = feature;

        [Fact]
        public void When_I_Load_The_SignUp_Page_We_Should_Get_A_200()
        {
            _feature
                .GiveACleanSlate()
                .Initialize()
                .WhenILoadTheSignUpPage()
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, view =>
                {
                    view.Should().NotBeNullOrEmpty();
                    view.Should().Contain("Create Account");
                    view.Should().Contain("Email Address");
                    view.Should().Contain("Username");
                    view.Should().Contain("Phone");
                    view.Should().Contain("Password");
                    view.Should().Contain("First Name");
                    view.Should().Contain("Last Name");
                });
        }

        [Fact]
        public void When_I_Create_An_Account_I_Should_See_Account_created_successfully_And_Be_Redirected_To_The_SignIn_Page()
        {
            var registerForm = new RegisterBuilder(TestData.EmailAddress, TestData.UserName, TestData.PhoneNumber, TestData.Password)
                .WithFirstName(TestData.FirstName)
                .WithLastName(TestData.LastName)
                .WithAcceptedTerms(true)
                .Build();

            _feature
                .GiveACleanSlate()
                .Initialize(additionalServiceConfiguration:
                services =>
                {
                    services.MockFirebaseAuthManager(mock =>
                    {
                        mock.SetupCreateuserSuccess();
                        mock.SetupGenerateEmailVerificationLinkAsync();
                    });

                    services.MockToastNotification(mock =>
                    {
                        mock.SetupAddSuccessToastMessage(ToastMessages.AccountCreatedSuccessfully);
                    });
                })
                .WhenILoadTheSignUpPage()
                .AndExtractTheAntiForgeryValues()
                .WhenISignUpUsing(registerForm)
                .ThenMockedToastNotificationShouldAssert(_feature.MockedToastNotification, result =>
                {
                    result.Verify(c => c.AddSuccessToastMessage(
                        It.Is<string>(s => s.Equals(ToastMessages.AccountCreatedSuccessfully)),
                        It.IsAny<LibraryOptions>()),
                        Times.Once);
                })
                .ThenTheAccountsTableShouldContainEmailOf(registerForm.EmailAddress)
                .ThenTheMailTableShouldContainAnEmailNotificationFor(registerForm.EmailAddress)
                .ThenTheRedirectedLocationShouldBe(RouteList.SignIn);
        }

        [Fact]
        public void When_A_Duplicate_Account_Creation_Is_Attempted()
        {
            var registerForm = new RegisterBuilder(TestData.EmailAddress, TestData.UserName, TestData.PhoneNumber, TestData.Password)
                .WithFirstName(TestData.FirstName)
                .WithLastName(TestData.LastName)
                .WithAcceptedTerms(true)
                .Build();

            var duplicateRegisterForm = registerForm;

            _feature
                .GiveACleanSlate()
                .Initialize(additionalServiceConfiguration:
                services =>
                {
                    services.MockFirebaseAuthManager(mock =>
                    {
                        mock.SetupCreateuserSuccess();
                        mock.SetupGenerateEmailVerificationLinkAsync();
                    });
                })
                .WhenILoadTheSignUpPage()
                .AndExtractTheAntiForgeryValues()
                .WhenISignUpUsing(registerForm)
                .WhenILoadTheSignUpPage()
                .AndExtractTheAntiForgeryValues()
                .WhenISignUpUsing(duplicateRegisterForm)
                .ThenMockedToastNotificationShouldAssert(_feature.MockedToastNotification, result =>
                {
                    result.Verify(c => c.AddErrorToastMessage(
                        It.Is<string>(s => s.Contains(ToastMessages.ValidationMessages.EmailAlreadyExists)), 
                        It.IsAny<LibraryOptions>()), 
                        Times.Once);
                });
        }
    }
}
