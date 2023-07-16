using FluentAssertions;
using hrvster.Core.Handlers.Tests.Common;
using hrvster.Web.Mvc.Common.Steps;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
    [Collection("Sequential")]
    public class SweepRulesCreationHandlerTests : MediatorHandlerTestBase, IClassFixture<MediatorHandlerFeature>
    {
        private readonly MediatorHandlerFeature _feature;

        public SweepRulesCreationHandlerTests(MediatorHandlerFeature feature)
            => _feature = feature;

        [Fact]
        public async Task A_Subscribed_User_Should_Be_Able_To_Create_A_WatchValue()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var profile = BuildNewTwitterProfile(account);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            var sweep = await BuildAndCreateSweepAsync(_feature, account, campaign);

            var ruleTweetPublished = CreateSweepRuleTweetPublished(sweep, profile);

            await _feature.Mediator.Publish(ruleTweetPublished);

            var watchValues = await _feature.SweepRulesRepository.GetSweepRuleAsync(sweep.SweepUid);
            watchValues.Count().Should().Be(1);
        }

        [Fact]
        public async Task A_Subscribed_User_Should_Not_Be_Able_To_Create_A_WatchValue_If_We_Reached_The_Max()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var profile = BuildNewTwitterProfile(account);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            var sweep = await BuildAndCreateSweepAsync(_feature, account, campaign);
            var plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.BasicPlanUid, default);
            var maxHashtags = plan.HashtagsPerMonth;

            var ruleTweetPublished = CreateSweepRuleTweetPublished(sweep, profile);
            var ruleTweetPublished2 = CreateSweepRuleTweetPublished2(sweep, profile);
            var ruleTweetPublished3 = CreateSweepRuleTweetPublished3(sweep, profile);

            await _feature.Mediator.Publish(ruleTweetPublished);
            await _feature.Mediator.Publish(ruleTweetPublished2);
            await _feature.Mediator.Publish(ruleTweetPublished3);

            var watchValues = await _feature.SweepRulesRepository.GetSweepRuleAsync(sweep.SweepUid);
            watchValues.Count().Should().Be(maxHashtags);
        }
    }
}
