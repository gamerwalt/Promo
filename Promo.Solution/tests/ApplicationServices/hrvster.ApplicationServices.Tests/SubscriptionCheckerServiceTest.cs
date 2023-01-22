using FluentAssertions;
using hrvster.ApplicationServices.Tests.Common;
using hrvster.Models;
using hrvster.Web.Mvc.Common.Steps;
using TestUtilities;
using TestUtilities.Extensions;
using Xunit;

namespace hrvster.ApplicationServices.Tests
{
    [Collection("Sequential")]
    public class SubscriptionCheckerServiceTest : BuilderClassAbstract, IClassFixture<SubscriptionFeature>
    {
        private readonly SubscriptionFeature _feature;

        public SubscriptionCheckerServiceTest(SubscriptionFeature feature)
            => _feature = feature;

        [Fact]
        public async Task Should_Return_No_Subscription_Data_For_The_Account()
        {
            _feature.GivenACleanSlate();
            var account = await GetAccount();

            var model = await _feature.AccountSubscriptionService.GetAccountSubscriptionAsync(account.AccountUid);

            model.Should().BeNull();
        }

        [Fact]
        public async Task Should_Return_An_AccountSubscription_For_The_Account()
        {
            _feature
                .GivenACleanSlate()
                .Initialize(additionalServiceConfiguration:
                services =>
                {
                    services.MockAuthenticiation();
                });

            var account = await GetAccount();

            var plans = await _feature.PlansRepository.GetPlansAsync();
            var plan = plans.FirstOrDefault();

            await _feature.SubscriptionService.SubscribeUserToAsync(plan, account.AccountUid);

            var model = await _feature.AccountSubscriptionService.GetAccountSubscriptionAsync(account.AccountUid);

            model.Should().NotBeNull();
            model.Account.AccountUid.Should().Be(account.AccountUid);
            model.Plan.Should().NotBeNull();
            model.Plan.PlanUid.Should().Be(plan.PlanUid);
        }

        private async Task<Account> GetAccount()
        {
            var account = BuildNewAccount();
            await _feature.AccountRepository.CreateAsync(account);
            account = await _feature.AccountRepository.GetAsync(account.AccountUid);
            return account;
        }
    }
}
