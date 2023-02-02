using Bogus;
using FluentAssertions;
using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests.MatchGames;
using hrvster.Web.Models;
using hrvster.Web.Mvc.Common.Steps;
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
			_feature.GivenACleanSlate();

			var matchGameUid = Guid.NewGuid();
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

			await _feature.Mediator.Send(CreateACreateMatchGameRequest(matchGameUid, account, model));

			var matchGames = await _feature.MatchGamesService.GetCampaignMatchGamesAsync(campaign.CampaignUid, default);

			matchGames.FirstOrDefault().Name.Should().Be(model.Name);
		}

		[Fact]
		public async Task Should_Not_Be_Able_To_Create_A_MatchGame_With_A_Name_That_Already_Exists()
		{
			_feature.GivenACleanSlate();

			var matchGameUid = Guid.NewGuid();
			var matchGameUid2 = Guid.NewGuid();
			var firstName = "FirstName";

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
			model.Name = firstName;
			await _feature.Mediator.Send(CreateACreateMatchGameRequest(matchGameUid, account, model));

			var matchGames = await _feature.MatchGamesService.GetCampaignMatchGamesAsync(campaign.CampaignUid, default);
			matchGames.Count().Should().Be(1);

			var model2 = createMatchGameFaker.Generate();
			model2.Name = firstName;
			await _feature.Mediator.Send(CreateACreateMatchGameRequest(matchGameUid2, account, model2));

			matchGames = await _feature.MatchGamesService.GetCampaignMatchGamesAsync(campaign.CampaignUid, default);
			matchGames.Count().Should().Be(1);
		}

		[Fact]
		public async Task Should_Be_Able_To_Return_A_Match_Game_From_A_Uid()
		{
			_feature.GivenACleanSlate();

			var matchGameUid = Guid.NewGuid();
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
			await _feature.Mediator.Send(CreateACreateMatchGameRequest(matchGameUid, account, model));

			var retrievedModel = _feature.MatchGamesService.GetMatchGameAsync(matchGameUid, default);

			var matchGame = await _feature.Mediator.Send(new GetMatchGameRequest(matchGameUid));

			matchGame.Should().NotBeNull();
			matchGame.Name.Should().Be(model.Name);
			matchGame.Uid.Should().Be(matchGameUid);
		}
	}
}
