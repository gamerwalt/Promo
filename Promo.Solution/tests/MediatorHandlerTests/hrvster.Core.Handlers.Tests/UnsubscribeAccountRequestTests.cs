using FluentAssertions;
using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests.WebRequests.Subscriptions;
using hrvster.Web.Mvc.Common.Steps;
using Moq;
using TestUtilities.Mocks.Clients;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
	[Collection("Sequential")]
	public class UnsubscribeAccountRequestTests : MediatorHandlerTestBase, IClassFixture<MediatorHandlerFeature>
	{
		private readonly MediatorHandlerFeature _feature;

		public UnsubscribeAccountRequestTests(MediatorHandlerFeature feature)
			=> _feature = feature;

		[Fact]
		public async Task Should_Set_The_SubscriptionAccount_As_Unsubscribed()
		{
			_feature.GivenACleanSlate();
			_feature.MockedFlutterwavePaymentService.Reset();

			_feature.MockedFlutterwavePaymentService
				.MockGetUserSubscriptionAsync()
				.MockUnsubscribeUser();

			var account = await BuildAccountAndSubscribeToStarterPlan(_feature);
			var campaign = await BuildAndCreateCampaignAsync(_feature, account);

			await _feature.Mediator.Send(new UnsubscribeAccountRequest(account.AccountUid));

			var accountSubscription = await _feature.AccountSubscriptionService.GetAccountSubscriptionAsync(account.AccountUid);

			accountSubscription.Should().NotBeNull();
			accountSubscription.SubscriptionStatus.Should().Be(Models.SubscriptionStatus.UnSubscribed);
		}
	}
}
