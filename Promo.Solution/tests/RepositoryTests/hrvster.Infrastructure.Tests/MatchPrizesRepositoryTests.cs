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
	public class MatchPrizesRepositoryTests : RepositoryAbstractModelBuilder, IClassFixture<RepositoryFeature>
	{
		private readonly RepositoryFeature _feature;
		private Faker<MatchGame> MatchGameFaker;
		private Faker<MatchPrize> MatchPrizeFaker;
		private Faker<MatchImage> MatchImageFaker;

		public MatchPrizesRepositoryTests(RepositoryFeature feature)
		{
			_feature = feature;
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		public async Task Should_Return_MatchPrizes_For_MatchGame(int numberOfPrizes)
		{
			_feature.GivenACleanSlate();
			var matchGame = await CreateAccountCampaignAndMatchGameAsync();
			var matchImage = await CreateMatchImageAsync(matchGame);

			CreateFakeMatchPrize(matchGame, matchImage);
			var matchPrizes = MatchPrizeFaker.Generate(numberOfPrizes);

			foreach(var matchPrize in matchPrizes)
			{
				await _feature.MatchPrizesRepository.CreateAsync(matchPrize, default);
			}

			var prizes = await _feature.MatchPrizesRepository.GetForMatchGameAsync(matchGame.Uid, default);
			prizes.Count().Should().Be(numberOfPrizes);
		}

		private async Task<MatchImage> CreateMatchImageAsync(MatchGame matchGame)
		{
			CreateFakerMatchImage(matchGame);
			var matchImage = MatchImageFaker.Generate();
			await _feature.MatchImageRepository.CreateAsync(matchImage, default);
			matchImage = await _feature.MatchImageRepository.GetAsync(matchImage.Uid, default);
			return matchImage;
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

		private void CreateFakerMatchImage(MatchGame matchGame)
		{
			MatchImageFaker = new Faker<MatchImage>()
				.RuleFor(m => m.Uid, f => Guid.NewGuid())
				.RuleFor(m => m.Name, f => f.Random.Word())
				.RuleFor(m => m.MatchGame, f => matchGame)
				.RuleFor(m => m.LocationUrl, f => f.Internet.Url());
		}

		private void CreateFakeMatchPrize(MatchGame matchGame, MatchImage matchImage)
		{
			MatchPrizeFaker = new Faker<MatchPrize>()
				.RuleFor(m => m.MatchGame, f => matchGame)
				.RuleFor(m => m.Uid, f => Guid.NewGuid())
				.RuleFor(m => m.Name, f => f.Lorem.Word())
				.RuleFor(m => m.Description, f => f.Lorem.Sentence())
				.RuleFor(m => m.Value, f => f.Random.Int())
				.RuleFor(m => m.Quantity, f => 10)
				.RuleFor(m => m.Image, f => matchImage);
		}

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
