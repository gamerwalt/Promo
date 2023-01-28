using FluentAssertions;
using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests;
using hrvster.Web.Mvc.Common.Steps;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
    [Collection("Sequential")]
    public class SweepstakesCreationHandlerTests : MediatorHandlerTestBase, IClassFixture<MediatorHandlerFeature>
    {
        private readonly MediatorHandlerFeature _feature;

        public SweepstakesCreationHandlerTests(MediatorHandlerFeature feature)
            => _feature = feature;

        [Fact]
        public async Task An_Account_WithoutASubscription_ShouldNotBeAllowed_To_Create_A_Sweepstake()
        {
            _feature.GivenACleanSlate();

            var account = await CreateAccountAsync(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);

            await _feature.Mediator.Send(new CreateSweep(
                CreateNewSweepRequest(account, campaign),
                TestData.WindowsTimezone,
                false));

            var sweeps = await _feature.SweepsRepository.GetAllForAccountAsync(account.AccountUid);
            sweeps.Count().Should().Be(0);
        }

        [Fact]
        public async Task An_Account_WithoutASubscription_ShouldNotBeAllowed_To_Create_A_Sweepstake_2()
        {
            _feature.GivenACleanSlate();

            var account = await CreateAccountAsync(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);

            await _feature.Mediator.Send(GetCreateSweepRequest(account, campaign));

            var sweeps = await _feature.SweepsRepository.GetAllForAccountAsync(account.AccountUid);
            sweeps.Count().Should().Be(0);
        }

        [Fact]
        public async Task ShouldNot_Allow_Sweepstakes_CreationOf_2_Sweeptakes_If_Plan_Has_1_Max_Sweepstakes()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToStarterPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);

            var plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.StarterPlanUid, default);
            plan.Should().NotBeNull();
            plan.SweepstakesPerMonth = 1;
            await _feature.PlansRepository.UpdateAsync(plan, default(CancellationToken));
            plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.StarterPlanUid, default);
            plan.Should().NotBeNull();
            plan.SweepstakesPerMonth.Should().Be(1);

            await _feature.Mediator.Send(GetCreateSweepRequest(account, campaign));

            var sweeps = await _feature.SweepsRepository.GetAllForAccountAsync(account.AccountUid);
            sweeps.Count().Should().Be(1);

            await _feature.Mediator.Send(GetCreateSweepRequest2(account, campaign));

            sweeps = await _feature.SweepsRepository.GetAllForAccountAsync(account.AccountUid);
            sweeps.Count().Should().Be(1);
        }

        [Fact]
        public async Task ShouldNot_Allow_Sweepstakes_CreationOf_2_Sweeptakes2_If_Plan_Has_1_Max_Sweepstakes()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToStarterPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);

            var plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.StarterPlanUid, default);
            plan.Should().NotBeNull();
            plan.SweepstakesPerMonth = 1;
            await _feature.PlansRepository.UpdateAsync(plan, default(CancellationToken));
            plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.StarterPlanUid, default);
            plan.Should().NotBeNull();
            plan.SweepstakesPerMonth.Should().Be(1);

            await _feature.Mediator.Send(new CreateSweep(
                CreateNewSweepRequest(account, campaign),
                TestData.WindowsTimezone,
                false));

            var sweeps = await _feature.SweepsRepository.GetAllForAccountAsync(account.AccountUid);
            sweeps.Count().Should().Be(1);

            await _feature.Mediator.Send(new CreateSweep(
                CreateNewSweepRequest2(account, campaign),
                TestData.WindowsTimezone,
                false));

            sweeps = await _feature.SweepsRepository.GetAllForAccountAsync(account.AccountUid);
            sweeps.Count().Should().Be(1);
        }
    }
}
