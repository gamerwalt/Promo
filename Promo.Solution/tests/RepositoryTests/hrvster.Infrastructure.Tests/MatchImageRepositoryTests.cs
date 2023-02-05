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
	public class MatchImageRepositoryTests : RepositoryAbstractModelBuilder, IClassFixture<RepositoryFeature>
	{
		private readonly RepositoryFeature _feature;
		private Faker<MatchGame> MatchGameFaker;
		private Faker<MatchImage> MatchImageFaker;

		public MatchImageRepositoryTests(RepositoryFeature feature)
		{
			_feature = feature;
		}

		[Fact]
		public async Task Should_Return_0_MatchImages_For_MatchGame()
		{
			_feature.GivenACleanSlate();

			var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
			var campaign = await BuildAndCreateCampaignAsync(_feature, account);
			
			var matchImages = await _feature.MatchImageRepository.GetForMatchGamesAsync(Guid.NewGuid(), default);

			matchImages.Count().Should().Be(0);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		public async Task Should_Return_MatchImages_For_MatchGame(int numberOfImages)
		{
			_feature.GivenACleanSlate();

			var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
			var campaign = await BuildAndCreateCampaignAsync(_feature, account);
			CreateFakeMatchGame(campaign);
			var matchGame = MatchGameFaker.Generate();
			await _feature.MatchGameRepository.CreateAsync(matchGame, TestData.WindowsTimezone, false, default);
			matchGame = await _feature.MatchGameRepository.GetAsync(matchGame.Uid, default);

			CreateFakerMatchImage(matchGame);
			var matchGameImages = MatchImageFaker.Generate(numberOfImages);

			foreach(var image in matchGameImages)
			{
				await _feature.MatchImageRepository.CreateAsync(image, default);
			}

			var matchImages = await _feature.MatchImageRepository.GetForMatchGamesAsync(matchGame.Uid, default);

			matchImages.Count().Should().Be(numberOfImages);
		}

		[Fact]
		public async Task Should_Be_Able_To_Retrieve_A_Match_Image_By_The_ImageUid()
		{
			_feature.GivenACleanSlate();

			var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
			var campaign = await BuildAndCreateCampaignAsync(_feature, account);
			CreateFakeMatchGame(campaign);
			var matchGame = MatchGameFaker.Generate();
			await _feature.MatchGameRepository.CreateAsync(matchGame, TestData.WindowsTimezone, false, default);
			matchGame = await _feature.MatchGameRepository.GetAsync(matchGame.Uid, default);

			CreateFakerMatchImage(matchGame);
			var matchGameImage = MatchImageFaker.Generate();
			await _feature.MatchImageRepository.CreateAsync(matchGameImage, default);

			var matchImage = await _feature.MatchImageRepository.GetAsync(matchGameImage.Uid, default);

			matchImage.Uid.Should().Be(matchGameImage.Uid);
			matchImage.LocationUrl.Should().Be(matchImage.LocationUrl);
		}

		public void CreateFakerMatchImage(MatchGame matchGame)
		{
			MatchImageFaker = new Faker<MatchImage>()
				.RuleFor(m => m.Uid, f => Guid.NewGuid())
				.RuleFor(m => m.MatchGame, f => matchGame)
				.RuleFor(m => m.LocationUrl, f => f.Internet.Url());
		}

		private void CreateFakeMatchGame(Campaign campaign)
		{
			MatchGameFaker = new Faker<MatchGame>()
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
}
