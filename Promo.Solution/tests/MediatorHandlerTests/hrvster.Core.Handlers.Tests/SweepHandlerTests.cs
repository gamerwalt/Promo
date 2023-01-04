using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Web.Mvc.Common.Steps;
using TestUtilities;
using TestUtilities.Common.Steps;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
	[Collection("Sequential")]
	public class SweepHandlerTests : BuilderClassAbstract, IClassFixture<MediatorHandlerFeature>
	{
		private readonly MediatorHandlerFeature _feature;

		public SweepHandlerTests(MediatorHandlerFeature feature)
			=> _feature = feature;

		[Fact]
		public async Task Should_Be_Able_To_Mark_A_Sweep_As_Draw_Attempted()
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

			var markSweepAsDrawAttemptedRequest = new MarkSweepAsDrawAttemptedRequest(sweep.SweepUid);
			await _feature.Mediator.Send(markSweepAsDrawAttemptedRequest);

			_feature.SweepsTableShouldContain(SweepTableFields.SweepName, sweep.Name);
			_feature.SweepsTableShouldContain(SweepTableFields.DrawAttempted, "1");
		}


	}
}