using hrvster.Web.Mvc.Common;
using hrvster.Web.Mvc.Common.Steps;
using System.Net;
using System.Threading.Tasks;
using TestUtilities;
using TestUtilities.Common;
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
                .GiveACleanSlate()
                .Initialize()
                .WhenILoadMyProfilePage()
                .ThenTheResponseShouldMatch(HttpStatusCode.Redirect);
        }
    }
}