using FluentAssertions;
using hrvster.Core.Clients.Common;
using hrvster.Core.Handlers.Tests.Common;
using hrvster.Web.Mvc.Common.Steps;
using Moq;
using TestUtilities.Data;
using TestUtilities.Mocks.Publishers;
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

        [Fact]
        public async Task If_We_Have_Max_WatchValues_We_Should_Not_Be_Able_To_PublishSweepRuleTweetRequest_Past_The_Max()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var profile = BuildNewTwitterProfile(account);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            var sweep = await BuildAndCreateSweepAsync(_feature, account, campaign);
            var plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.BasicPlanUid, default);
            var maxHashtags = plan.HashtagsPerMonth;

            var request = GetPublishSweepRuleTweetRequest(profile, sweep);
            var request2 = GetPublishSweepRuleTweetRequest2(profile, sweep);
            var request3 = GetPublishSweepRuleTweetRequest3(profile, sweep);

            _feature.MockedTwitterClient.Reset();
            _feature.MockedTwitterClient.SetupPublishAsync();

            await _feature.Mediator.Send(request);
            await _feature.Mediator.Send(request2);
            await _feature.Mediator.Send(request3);

            var watchValues = await _feature.SweepRulesRepository.GetSweepRuleAsync(sweep.SweepUid);

            watchValues.Count().Should().Be(maxHashtags);
            _feature.MockedTwitterClient.Verify(m => m.PublishAsync(It.IsAny<Message>()), Times.Exactly(2));
        }

        [Fact]
        public async Task If_We_Have_Max_WatchValues_We_Should_Not_Be_Able_To_SaveScheduledSweepRules_Past_The_Max()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var profile = await BuildAndCreateProfileAsync(_feature, account);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            var sweep = await BuildAndCreateSweepAsync(_feature, account, campaign);
            var plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.BasicPlanUid, default);
            var maxHashtags = plan.HashtagsPerMonth;

            var request = GetPublishSweepRuleTweetRequest(profile, sweep, false);
            var request2 = GetPublishSweepRuleTweetRequest2(profile, sweep, false);
            var request3 = GetPublishSweepRuleTweetRequest3(profile, sweep, false);

            _feature.MockedTwitterClient.Reset();
            _feature.MockedTwitterClient.SetupPublishAsync();

            await _feature.Mediator.Send(request);
            await _feature.Mediator.Send(request2);
            await _feature.Mediator.Send(request3);

            var watchValues = await _feature.SweepRulesRepository.GetSweepRuleAsync(sweep.SweepUid);
            var scheduledTweets = await _feature.ScheduledSweepRuleTweetsRepository.GetSweepScheduledTweetAsync(sweep.SweepUid);

            watchValues.Count().Should().Be(maxHashtags);
            scheduledTweets.Count().Should().Be(maxHashtags);
            _feature.MockedTwitterClient.Verify(m => m.PublishAsync(It.IsAny<Message>()), Times.Exactly(0));
        }
    }
}
