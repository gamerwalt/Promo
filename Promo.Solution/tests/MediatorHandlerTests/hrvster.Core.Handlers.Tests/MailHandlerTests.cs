using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Web.Mvc.Common.Steps;
using Humanizer;
using TestUtilities;
using TestUtilities.Builders.Resources;
using TestUtilities.Common.Steps;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
	[Collection("Sequential")]
	public class MailHandlerTests : BuilderClassAbstract, IClassFixture<MediatorHandlerFeature>
	{
		private readonly MediatorHandlerFeature _feature;

		public MailHandlerTests(MediatorHandlerFeature feature)
			=> _feature = feature;

		[Fact]
		public async Task Should_Save_Mail_Information_Into_The_Database_When_We_Call_Request()
		{
			_feature.GivenACleanSlate();

			var request = new SweepEntriesNotEnoughMailRequest(TestData.EmailAddress, TestData.SweepName);

			await _feature.Mediator.Send(request);

			_feature.EmailsTableShouldContainFieldEqualTo(MailTableFields.EmailAddress, TestData.EmailAddress);
			_feature.EmailsTableShouldContain(MailTableFields.Parameters, TestData.SweepName);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(4)]
		public async Task Should_Save_Mail_Into_Database_When_We_Call_The_NotifyUserSweepsGoingLiveRequest(int hourValue)
		{
			_feature.GivenACleanSlate();

			var account = BuildNewAccount();
			var campaign = BuildNewCampaign(account, BuildNewCampaignType());
			await _feature.AccountRepository.CreateAsync(account);
			await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);
			account = await _feature.AccountRepository.GetAsync(account.AccountUid);
			campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);
			var sweep = BuildNewSweep(account, campaign);
			await _feature.SweepsRepository.CreateAsync(sweep, TestData.WindowsTimezone, false);

			var request = new NotifyUserSweepsGoingLiveRequest(account.AccountUid, sweep, 1);

			await _feature.Mediator.Send(request);

			_feature.EmailsTableShouldContainFieldEqualTo(MailTableFields.EmailAddress, TestData.EmailAddress);
			_feature.EmailsTableShouldContain(MailTableFields.Parameters, TestData.SweepName);
			_feature.EmailsTableShouldContain(MailTableFields.Parameters, hourValue.ToString());
		}
	}
}
