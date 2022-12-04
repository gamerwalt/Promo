using FluentAssertions;
using hrvster.Core;
using hrvster.Core.Requests.WebRequests;
using hrvster.Web.Models;
using hrvster.Web.Mvc.Common;
using hrvster.Web.Mvc.Common.Steps;
using hrvster.Web.Mvc.Common.Steps.Authentication;
using hrvster.Web.Mvc.Common.Steps.Promotion;
using Moq;
using NToastNotify;
using System;
using System.Net;
using TestUtilities.Builders.Resources;
using TestUtilities.Common;
using TestUtilities.Common.Steps;
using TestUtilities.Data;
using TestUtilities.Extensions;
using TestUtilities.Mocks.Authentication;
using TestUtilities.Mocks.Notifications;
using Xunit;

namespace ControllerTests
{
    [Collection("Sequential")]
    public class PromotionControllerTests : IClassFixture<Feature>
    {
        private readonly Feature _feature;

        public PromotionControllerTests(Feature feature)
            => _feature = feature;

        [Fact]
        public void When_I_Load_The_Promotions_Page_We_Should_Get_A_200()
        {
            _feature
                .GiveACleanSlate()
                .Initialize(additionalServiceConfiguration:
                services =>
                {
                    services.MockAuthenticiation();
                })
                .WhenILoadThePromotionsPage()
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, view =>
                {
                    view.Should().NotBeNullOrEmpty();
                    view.Should().Contain("Promotions");
                    view.Should().Contain("0 promotion(s) so far");
                    view.Should().Contain("Add Promotion");
                });
        }

        [Fact]
        public void When_I_Load_The_Create_Promotions_Page_We_Should_Get_A_200()
        {
            _feature
                .GiveACleanSlate()
                .Initialize(additionalServiceConfiguration:
                services =>
                {
                    services.MockAuthenticiation();
                })
                .WhenIloadThePromotionCreationPage()
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, view =>
                {
                    view.Should().NotBeNullOrEmpty();
                    view.Should().Contain("Create Promotion");
                    view.Should().Contain("Promotion Name");
                    view.Should().Contain("Description");
                    view.Should().Contain("Start Date");
                    view.Should().Contain("End Date");
                    view.Should().Contain("Promotion Type");
                });
        }

        [Fact]
        public void When_I_Create_A_Promotion_We_Should_See_The_PromotionCreatedSuccessfully()
        {
            var form = new SigninForm() { Email = TestData.EmailAddress, Password = TestData.Password };

            var registerForm = new RegisterBuilder(TestData.EmailAddress, TestData.UserName, TestData.PhoneNumber, TestData.Password)
                .WithFirstName(TestData.FirstName)
                .WithLastName(TestData.LastName)
                .WithAcceptedTerms(true)
                .Build();

            var createPromotionForm = new CreatePromotion
            {
                CampaignTypeUid = TestData.CampaignTypeUid,
                Name = TestData.PromotionName,
                Description = TestData.PromotionName,
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(30)
            };

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

                    services.MockFirebaseAuthProvider(mock =>
                    {
                        mock.SetupSignInWithEmailAndPasswordAsyncSuccess(TestData.EmailAddress, TestData.Password);
                    });

                    services.MockAuthenticiation();
                })
                .WhenILoadTheSignUpPage()
                .AndExtractTheAntiForgeryValues()
                .WhenISignUpUsing(registerForm)
                .WhenILoadTheSignInForm()
                .AndExtractTheAntiForgeryValues()
                .WhenISignInUsing(form)
                .AndUpdateTheAccountUid(TestData.UserUid)
                .WhenIloadThePromotionCreationPage()
                .AndExtractTheAntiForgeryValues()
                .AndCreateAPromotionUsing(createPromotionForm)
                .ThenMockedToastNotificationShouldAssert(_feature.MockedToastNotification, result =>
                {
                    result.Verify(c => c.AddSuccessToastMessage(
                        It.Is<string>(s => s.Contains(ToastMessages.PromotionCreatedSuccessfully)),
                        It.IsAny<LibraryOptions>()),
                        Times.Once);
                })
                .ThenTheRedirectedLocationShouldContain(RouteList.ViewPromotion)
                .ThenTheCampaignsTableShouldContainPromotionName(createPromotionForm.Name);
        }

        [Fact]
        public void When_I_Create_The_Same_Promotion_Page_We_Should_See_An_Error_Message()
        {
            var form = new SigninForm() { Email = TestData.EmailAddress, Password = TestData.Password };

            var registerForm = new RegisterBuilder(TestData.EmailAddress, TestData.UserName, TestData.PhoneNumber, TestData.Password)
                .WithFirstName(TestData.FirstName)
                .WithLastName(TestData.LastName)
                .WithAcceptedTerms(true)
                .Build();

            var createPromotionForm = new CreatePromotion
            {
                CampaignTypeUid = TestData.CampaignTypeUid,
                Name = TestData.PromotionName,
                Description = TestData.PromotionName,
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(30)
            };

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

                    services.MockFirebaseAuthProvider(mock =>
                    {
                        mock.SetupSignInWithEmailAndPasswordAsyncSuccess(TestData.EmailAddress, TestData.Password);
                    });

                    services.MockAuthenticiation();
                })
                .WhenILoadTheSignUpPage()
                .AndExtractTheAntiForgeryValues()
                .WhenISignUpUsing(registerForm)
                .WhenILoadTheSignInForm()
                .AndExtractTheAntiForgeryValues()
                .WhenISignInUsing(form)
                .AndUpdateTheAccountUid(TestData.UserUid)
                .WhenIloadThePromotionCreationPage()
                .AndExtractTheAntiForgeryValues()
                .AndCreateAPromotionUsing(createPromotionForm)
                .AndCreateAPromotionUsing(createPromotionForm)
                .ThenMockedToastNotificationShouldAssert(_feature.MockedToastNotification, result =>
                {
                    result.Verify(c => c.AddErrorToastMessage(
                        It.Is<string>(s => s.Contains("The promotion already exists")),
                        It.IsAny<LibraryOptions>()),
                        Times.Once);
                })
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, view =>
                {
                    view.Should().NotBeNull();
                });
        }
    }
}
