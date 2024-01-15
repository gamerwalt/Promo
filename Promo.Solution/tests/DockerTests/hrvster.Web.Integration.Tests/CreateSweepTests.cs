using FluentAssertions;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Exceptions;
using TestUtilities.Data;

namespace hrvster.Web.Integration.Tests
{
    public class CreateSweepTests : IClassFixture<Feature>
    {
        private Feature _feature;
        public CreateSweepTests(Feature feature)
        {
            _feature = feature;
        }

        [Fact]
        public async Task Should_Throw_An_AccountIsNotSubscribedException_When_We_Try_To_Create_A_Sweepstake_Without_A_Subscription()
        {
            var request = new CreateSweepRequest(
                TestData.SweepName,
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(30),
                DateTime.UtcNow.AddDays(29),
                TestData.CampaignUid,
                TestData.AccountUid,
                TestData.WindowsTimezone,
                true);

            var exception = await Assert.ThrowsAsync<AccountIsNotSubscribedException>(() => _feature.TestMediator.Send(request));

            exception.Message.Should().Contain(TestData.AccountUid.ToString());
        }
    }
}
