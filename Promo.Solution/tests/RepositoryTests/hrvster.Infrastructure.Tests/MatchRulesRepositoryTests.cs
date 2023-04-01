using Bogus;
using FluentAssertions;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Tests.Common;
using hrvster.Models;
using hrvster.Web.Mvc.Common.Steps;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Infrastructure.Tests
{
	[Collection("Sequential")]
	public class MatchRulesRepositoryTests : RepositoryAbstractModelBuilder, IClassFixture<RepositoryFeature>
	{
		private readonly RepositoryFeature _feature;
		private Faker<MatchGame> MatchGameFaker;
		private Faker<MatchGameRule> MatchGameRuleFaker;

		public MatchRulesRepositoryTests(RepositoryFeature feature)
		{
			_feature = feature;
		}

		[Fact]
		public async Task Should_Return_0_MatchGame_Rules()
		{
			_feature.GivenACleanSlate();
			var matchGame = await CreateAccountCampaignAndMatchGameAsync();

			matchGame = await _feature.MatchGameRepository.GetAsync(matchGame.Uid, default);
			
			var matchGameRules = await _feature.MatchGameRulesRepository.GetAllRulesAsync(matchGame.Uid, default);

			matchGameRules.Count().Should().Be(0);
		}

		[Fact]
		public async Task Should_Return_1_Match_Game_Rule()
		{
			_feature.GivenACleanSlate();
			var matchGame = await CreateAccountCampaignAndMatchGameAsync();

			matchGame = await _feature.MatchGameRepository.GetAsync(matchGame.Uid, default);

            MatchGameRuleFaker = new Faker<MatchGameRule>();
			CreateFakeMatchGameRule(matchGame, WatchTypeCode.Hashtag);
            var matchGameRule = MatchGameRuleFaker.Generate();

			await _feature.MatchGameRulesRepository.CreateAsync(matchGameRule, default);

			var matchGameRules = await _feature.MatchGameRulesRepository.GetAllRulesAsync(matchGame.Uid, default);

			matchGameRules.Count().Should().Be(1);
		}

		private async Task<MatchGame> CreateAccountCampaignAndMatchGameAsync()
		{
			var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
			var campaign = await BuildAndCreateCampaignAsync(_feature, account);
			CreateFakeMatchGame(campaign);

			var matchGame = MatchGameFaker.Generate();
			await _feature.MatchGameRepository.CreateAsync(matchGame, TestData.WindowsTimezone, false, default);
			matchGame = await _feature.MatchGameRepository.GetAsync(matchGame.Uid, default);
			return matchGame;
		}

		private void CreateFakeMatchGameRule(MatchGame matchGame, WatchTypeCode watchTypeCode)
			=> MatchGameRuleFaker = new Faker<MatchGameRule>()
				.RuleFor(m => m.Uid, f => Guid.NewGuid())
				.RuleFor(m => m.MatchGame, f => matchGame)
				.RuleFor(m => m.WatchTypeCode, f => WatchTypeCode.Hashtag)
				.RuleFor(m => m.WatchValue, f => f.Random.Word())
				.RuleFor(m => m.MustBeFollowing, f => f.Random.Bool())
				.RuleFor(m => m.NotifyWinners, f => f.Random.Bool())
				.RuleFor(m => m.ScheduledInd, f => f.Random.Bool())
				.RuleFor(m => m.ActiveInd, f => f.Random.Bool())
				.RuleFor(m => m.ExternalId, f => f.Random.Long().ToString());

		private void CreateFakeMatchGame(Campaign campaign)
			=> MatchGameFaker = new Faker<MatchGame>()
				.RuleFor(m => m.Uid, f => Guid.NewGuid())
				.RuleFor(m => m.CampaignId, f => campaign.CampaignId)
				.RuleFor(m => m.CampaignUid, f => campaign.CampaignUid)
				.RuleFor(m => m.Name, f => f.Lorem.Word())
				.RuleFor(m => m.Description, f => f.Lorem.Sentence(2))
				.RuleFor(m => m.NumberOfBoards, f => f.Random.Number(1, 100))
				.RuleFor(m => m.TileCountPerBoard, f => 9)
				.RuleFor(m => m.NumberOfWinnableBoards, f => f.Random.Number(1, 100))
				.RuleFor(m => m.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(m => m.StartDate, f => DateTime.Now)
				.RuleFor(m => m.EndDate, f => DateTime.Now.AddDays(30));
	}
}
