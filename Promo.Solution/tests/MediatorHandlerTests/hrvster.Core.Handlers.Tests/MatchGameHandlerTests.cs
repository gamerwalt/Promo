using Bogus;
using FluentAssertions;
using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests.MatchGames;
using hrvster.Web.Models;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
	[Collection("Sequential")]
	public class MatchGameHandlerTests : MediatorHandlerTestBase, IClassFixture<MediatorHandlerFeature>
	{
		private readonly MediatorHandlerFeature _feature;

		public MatchGameHandlerTests(MediatorHandlerFeature feature)
			=> _feature = feature;

		[Fact]
		public async Task Should_Be_Able_To_Create_A_Match_Game()
		{
			var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
			var campaign = await BuildAndCreateCampaignAsync(_feature, account);

			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.PromotionUid, f => campaign.CampaignUid)
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(1))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinnableBoards, f => 90)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)));
			var model = createMatchGameFaker.Generate();

			var request = new CreateMatchGameRequest(
				accountUid: account.AccountUid,
				promotionUid: model.PromotionUid,
				name: model.Name,
				description: model.Description,
				numberOfBoards: model.NumberOfBoards,
				winnableBoards: model.WinnableBoards,
				winProbability: model.WinProbability,
				startDate: model.StartDate,
				endDate: model.EndDate,
				TestData.WindowsTimezone,
				false
				);

			await _feature.Mediator.Send(request);

			var matchGames = await _feature.MatchGamesService.GetCampaignMatchGamesAsync(campaign.CampaignUid, default);

			matchGames.FirstOrDefault().Name.Should().Be(model.Name);
		}
	}
}
