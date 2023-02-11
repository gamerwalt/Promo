using FluentAssertions;
using hrvster.Infrastructure.Tests.Common;
using hrvster.Web.Mvc.Common.Steps;
using TestUtilities;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Infrastructure.Tests
{
	public class AccountSubscriptionRepositoryTests : BuilderClassAbstract, IClassFixture<RepositoryFeature>
	{
		private readonly RepositoryFeature _feature;

		public AccountSubscriptionRepositoryTests(RepositoryFeature feature)
			=> _feature = feature;

		[Fact]
		public async Task Should_Be_Able_To_Update_The_AccountSubscriptionDates()
		{
			_feature.GivenACleanSlate();

			var account = BuildNewAccount();
			await _feature.AccountsRepository.CreateAsync(account);
			var plan = await _feature.PlansRepository.GetPlanByUidAsync(TestData.BasicPlanUid, default);
			await _feature.AccountSubscriptionsRepository.CreateAsync(new Models.Subscriptions.AccountSubscription
			{
				Account = account,
				Plan = plan,
				SubscriptionStatus = Models.SubscriptionStatus.Subscribed,
				StartDate = DateTime.Now,
				StartDateUtc = DateTime.UtcNow,
				EndDate = DateTime.Now.AddDays(30),
				EndDateUtc = DateTime.UtcNow.AddDays(30)
			});

			var accountSubscription = await _feature.AccountSubscriptionsRepository.GetAccountSubscriptionAsync(account.AccountUid);

			var startDate = accountSubscription.EndDate.AddDays(1);
			var startDateUtc = accountSubscription.EndDateUtc.AddDays(1);
			var endDate = startDate.AddDays(30);
			var endDateUtc = startDateUtc.AddDays(30);

			await _feature.AccountSubscriptionsRepository.UpdateNewDatesAsync(
				accountSubscription.SubscriptionUid,
				startDate,
				startDateUtc,
				endDate,
				endDateUtc);

			accountSubscription = await _feature.AccountSubscriptionsRepository.GetAccountSubscriptionAsync(account.AccountUid);
			accountSubscription.Should().NotBeNull();
			accountSubscription.StartDate.Should().Be(startDate);
			accountSubscription.StartDateUtc.Should().Be(startDateUtc);
			accountSubscription.EndDate.Should().Be(endDate);
			accountSubscription.EndDateUtc.Should().Be(endDateUtc);
		}
	}
}
