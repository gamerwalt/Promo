using FluentAssertions;
using hrvster.Core.Requests.WebRequests;
using hrvster.Web.Mvc.Common.Steps;
using hrvster.Web.Mvc.Common.Steps.Promotion;
using hrvster.Web.Mvc.Common.Steps.Sweeps;
using System.Net;
using TestUtilities;
using TestUtilities.Common;
using TestUtilities.Data;
using TestUtilities.Extensions;
using TestUtilities.Mocks.Subscriptions;
using Xunit;

namespace ControllerTests
{
	[Collection("Sequential")]
	public class SubscriptionTests : IClassFixture<Feature>
	{
		private readonly Feature _feature;

		public SubscriptionTests(Feature feature)
		{
			_feature = feature;
		}

		[Fact]
		public void When_We_Visit_The_Promotions_Page_And_We_Are_Not_Subscribed_We_Should_Be_Redirected()
		{
			_feature
				.GiveACleanSlate()
				.Initialize(additionalServiceConfiguration:
				services =>
				{
					services.MockAuthenticiation();
					services.MockSubscription(mock =>
					{
						mock.MockIsNotSubscribed();
					});
				})
				.WhenILoadThePromotionsPage()
				.ThenTheRedirectedLocationShouldContain("Unsubscribed");
		}

		[Fact]
		public void When_We_Visit_The_Promotions_Page_And_We_Are_Subscribed_We_Should_Good()
		{
			_feature
				.GiveACleanSlate()
				.Initialize(additionalServiceConfiguration:
				services =>
				{
					services.MockAuthenticiation();
					services.MockSubscription(mock =>
					{
						mock.MockIsSubscribed();
					});
				})
				.WhenILoadThePromotionsPage()
				.ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, view =>
				{
					view.Should().NotBeNullOrEmpty();
					view.Should().Contain("Promotions");
					view.Should().Contain("Add Promotion");
				});
		}

		[Fact]
		public void When_We_Visit_The_Sweeps_Page_And_We_Are_Not_Subscribed_We_Should_Be_Redirected()
		{
			_feature
				.GiveACleanSlate()
				.Initialize(additionalServiceConfiguration:
				services =>
				{
					services.MockAuthenticiation();
					services.MockSubscription(mock =>
					{
						mock.MockIsNotSubscribed();
					});
				})
				.WhenILoadTheSweepsPage()
				.ThenTheRedirectedLocationShouldContain("Unsubscribed");
		}

		[Fact]
		public void When_We_Visit_The_Sweeps_Page_And_We_Are_Subscribed_We_Should_Be_Good()
		{
			_feature
				.GiveACleanSlate()
				.Initialize(additionalServiceConfiguration:
				services =>
				{
					services.MockAuthenticiation();
					services.MockSubscription(mock =>
					{
						mock.MockIsSubscribed();
					});
				})
				.WhenILoadTheSweepsPage()
				.ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, view =>
				{
					view.Should().NotBeNullOrEmpty();
					view.Should().Contain("Sweepstakes");
				});
		}
	}
}
