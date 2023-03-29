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
	public class MatchTileRepositoryTests : RepositoryAbstractModelBuilder, IClassFixture<RepositoryFeature>
	{
		private readonly RepositoryFeature _feature;
		private Faker<MatchGame> MatchGameFaker;
		private Faker<MatchPrize> MatchPrizeFaker;
		private Faker<MatchImage> MatchImageFaker;
		private Faker<MatchBoard> MatchBoardFaker;
		private Faker<MatchTile> MatchTileFaker;

		public MatchTileRepositoryTests(RepositoryFeature feature)
		{
			_feature = feature;
		}

		[Fact]
		public async Task Should_Return_0_Tiles()
		{
			_feature.GivenACleanSlate();
			var matchGame = await CreateAccountCampaignAndMatchGameAsync();

			var tiles = await _feature.MatchTilesRepository.GetForMatchGameAsync(matchGame.Uid, default);

			tiles.Count().Should().Be(0);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(20)]
		public async Task Should_Return_Some_Tiles(int numTiles)
		{
			_feature.GivenACleanSlate();
			var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
			var campaign = await BuildAndCreateCampaignAsync(_feature, account);
			var matchGame = await CreateMatchGameAsync(campaign);
			var matchImage = await CreateMatchImageAsync(matchGame);
			var matchPrize = await CreateMatchPrizeAsync(matchGame, matchImage);
			var matchBoard = await CreateMatchBoardAsync(matchGame, matchPrize);

			CreateFakeMatchTile(matchBoard, matchImage, matchPrize);

			var tiles = MatchTileFaker.Generate(numTiles);
			foreach(var tile in tiles)
			{
				await _feature.MatchTilesRepository.CreateAsync(tile, default);
			}

			var retrievedTiles = await _feature.MatchTilesRepository.GetForBoardAsync(matchBoard.Uid, default);
			retrievedTiles.Should().NotBeNull();
			retrievedTiles.Should().HaveCount(numTiles);
		}

		private void CreateFakeMatchTile(MatchBoard board, MatchImage image, MatchPrize prize)
		{
			var winnableBoard = board.IsWinnable;

			MatchTileFaker = new Faker<MatchTile>()
				.RuleFor(m => m.Uid, f => f.Random.Guid())
				.RuleFor(m => m.Board, f => board)
				.RuleFor(m => m.Image, f => image)
				.RuleFor(m => m.Prize, f => winnableBoard ? prize : null)
				.RuleFor(m => m.WinIndicator, f => winnableBoard ? f.Random.Bool() : false);
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

		private async Task<MatchBoard> CreateMatchBoardAsync(MatchGame matchGame, MatchPrize matchPrize)
		{
			CreateFakerMatchBoard(matchGame, matchPrize);
			var board = MatchBoardFaker.Generate();
			await _feature.MatchBoardsRepository.CreateAsync(board, default);
			return board;
		}

		private async Task<MatchGame> CreateMatchGameAsync(Campaign campaign)
		{
			CreateFakeMatchGame(campaign);
			var matchGame = MatchGameFaker.Generate();
			await _feature.MatchGameRepository.CreateAsync(matchGame, TestData.WindowsTimezone, false, default);
			return matchGame;
		}

		private void CreateFakerMatchBoard(MatchGame matchGame, MatchPrize matchPrize)
		{
			var faker = new Faker();
			var winnable = faker.Random.Bool();

			MatchBoardFaker = new Faker<MatchBoard>()
				.RuleFor(m => m.MatchGame, f => matchGame)
				.RuleFor(m => m.Prize, f => winnable ? matchPrize : new MatchPrize { Uid = Guid.NewGuid() })
				.RuleFor(m => m.Uid, f => Guid.NewGuid())
				.RuleFor(m => m.IsWinnable, f => winnable);
		}
		private async Task<MatchPrize> CreateMatchPrizeAsync(MatchGame matchGame, MatchImage matchImage)
		{
			CreateFakeMatchPrize(matchGame, matchImage);
			var matchPrize = MatchPrizeFaker.Generate();
			await _feature.MatchPrizesRepository.CreateAsync(matchPrize, default);
			return matchPrize;
		}

		private async Task<MatchImage> CreateMatchImageAsync(MatchGame matchGame)
		{
			CreateFakerMatchImage(matchGame);
			var matchImage = MatchImageFaker.Generate();
			await _feature.MatchImageRepository.CreateAsync(matchImage, default);
			return matchImage;
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

		public void CreateFakerMatchImage(MatchGame matchGame)
		{
			MatchImageFaker = new Faker<MatchImage>()
				.RuleFor(m => m.Uid, f => Guid.NewGuid())
				.RuleFor(m => m.Name, f => f.Random.Word())
				.RuleFor(m => m.MatchGame, f => matchGame)
				.RuleFor(m => m.LocationUrl, f => f.Internet.Url());
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
