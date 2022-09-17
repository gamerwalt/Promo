using FluentAssertions;
using hrvster.Web.Mvc.Common;
using hrvster.Web.Mvc.Common.Steps;
using System.Net;
using TestUtilities.Common;
using TestUtilities.Common.Steps;
using Xunit;

namespace ProfilesControllerTests
{
    [Collection("Sequential")]
    public class ProfileControllerGetTests : IClassFixture<Feature>
    {
        private readonly Feature _feature;

        public ProfileControllerGetTests(Feature feature)
        {
            _feature = feature;
        }

        [Fact]
        public void When_Not_Authorized_We_Are_Redirected()
        {
            _feature
                .Initialize()
                .GiveACleanSlate()
                .WhenILoadMyProfilePage()
                .ThenTheResponseShouldMatch(HttpStatusCode.Redirect);
        }

        [Fact]
        public void When_Authorized_We_Are_Not_Redirected()
        {
            _feature
                .Initialize()
                .GiveACleanSlate()
                .WebAuthorization("SomeToken")
                .WhenILoadMyProfilePage()
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeEmpty();
                    result.Should().Contain("My Profile");
                });
        }
    }
}