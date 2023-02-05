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
    public class MatchGamesRepositoryTests : RepositoryAbstractModelBuilder, IClassFixture<RepositoryFeature>
    {
        private readonly RepositoryFeature _feature;
        private Faker<MatchGame> MatchGameFaker;
        private Campaign Campaign;

        public MatchGamesRepositoryTests(RepositoryFeature feature)
        {
            _feature = feature;
        }


        [Fact]
        public async Task Should_Return_0_Match_Games_For_Account()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);

            var matchGames = await _feature.MatchGameRepository.GetByAccountAsync(account.AccountUid, default);
            matchGames.Count().Should().Be(0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task Should_Be_Able_To_Create_1_Match_Game_And_Retrieve_It(int numberOfGames)
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);

            var matchGames = MatchGameFaker.Generate(numberOfGames);

            foreach(var matchGame in matchGames)
            {
                await _feature.MatchGameRepository.CreateAsync(matchGame, TestData.WindowsTimezone, false, default(CancellationToken));
            }            

            var games = await _feature.MatchGameRepository.GetByAccountAsync(account.AccountUid, default);
            games.Count().Should().Be(numberOfGames);
        }

        [Fact]
        public async Task Should_Be_Able_To_Retrieve_A_Match_Game_By_Uid()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);

            var matchGame= MatchGameFaker.Generate();
            await _feature.MatchGameRepository.CreateAsync(matchGame, TestData.WindowsTimezone, false, default);

            var game = await _feature.MatchGameRepository.GetAsync(matchGame.Uid, default);
            game.Should().NotBeNull();
            game.Uid.Should().Be(matchGame.Uid);
        }

        [Fact]
        public async Task Should_Be_Able_To_Update_The_Match_Game()
        {
            _feature.GivenACleanSlate();

            var updatedName = "MatchGameName";
            var updatedNumberOfBoards = 10;

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);

            var matchGame = MatchGameFaker.Generate();
            await _feature.MatchGameRepository.CreateAsync(matchGame, TestData.WindowsTimezone, false, default);

            var game = await _feature.MatchGameRepository.GetAsync(matchGame.Uid, default);

            game.Name = updatedName;
            game.NumberOfBoards = updatedNumberOfBoards;

            await _feature.MatchGameRepository.UpdateAsync(game, TestData.WindowsTimezone, false, default(CancellationToken));

            game = await _feature.MatchGameRepository.GetAsync(game.Uid, default);
            game.Name.Should().Be(updatedName);
            game.NumberOfBoards.Should().Be(updatedNumberOfBoards);
        }

        [Fact]
        public async Task Should_Be_Able_To_Set_A_Game_As_Generated()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);

            var matchGame = MatchGameFaker.Generate();
            await _feature.MatchGameRepository.CreateAsync(matchGame, TestData.WindowsTimezone, false, default);

            var game = await _feature.MatchGameRepository.GetAsync(matchGame.Uid, default);

            await _feature.MatchGameRepository.SetAsGeneratedAsync(game, default);

            game = await _feature.MatchGameRepository.GetAsync(game.Uid, default);
            game.HasBeenGenerated.Should().BeTrue();
        }

        [Fact]
        public async Task When_MatchGame_Is_Generated_We_Should_Not_Be_Able_To_Update_It()
        {
            _feature.GivenACleanSlate();

            var updatedName = "MatchGameName";
            var updatedNumberOfBoards = 10;

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);

            var matchGame = MatchGameFaker.Generate();
            await _feature.MatchGameRepository.CreateAsync(matchGame, TestData.WindowsTimezone, false, default);

            var game = await _feature.MatchGameRepository.GetAsync(matchGame.Uid, default);
            var createdName = game.Name;
            var createdNumberOfBoards = game.NumberOfBoards;

            await _feature.MatchGameRepository.SetAsGeneratedAsync(game, default(CancellationToken));

            game = await _feature.MatchGameRepository.GetAsync(game.Uid, default);
            game.Name = updatedName;
            game.NumberOfBoards = updatedNumberOfBoards;

            await _feature.MatchGameRepository.UpdateAsync(game, TestData.WindowsTimezone, false, default);

            game = await _feature.MatchGameRepository.GetAsync(game.Uid, default);
            game.Name.Should().Be(createdName);
            game.NumberOfBoards = createdNumberOfBoards;
        }

        [Fact]
        public async Task Should_Retrieve_2_MatchGames_From_Campaign1()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign1 = await BuildAndCreateCampaignAsync(_feature, account);
            var campaign2 = await BuildAndCreateCampaignAsync2(_feature, account);
            CreateFakeMatchGame(campaign1);

            var matchGame1 = MatchGameFaker.Generate();
            var matchGame2 = MatchGameFaker.Generate();

            await _feature.MatchGameRepository.CreateAsync(matchGame1, TestData.WindowsTimezone, false, default);
            await _feature.MatchGameRepository.CreateAsync(matchGame2, TestData.WindowsTimezone, false, default);

            CreateFakeMatchGame(campaign2);

            var matchGame3 = MatchGameFaker.Generate();
            await _feature.MatchGameRepository.CreateAsync(matchGame3, TestData.WindowsTimezone, false, default);

            var matchGames1 = await _feature.MatchGameRepository.GetByCampaignAsync(campaign1.CampaignUid, default);
            var matchGames2 = await _feature.MatchGameRepository.GetByCampaignAsync(campaign2.CampaignUid, default);

            matchGames1.Count().Should().Be(2);
            matchGames2.Count().Should().Be(1);
        }

        [Fact]
        public async Task Can_Set_A_MatchGame_Ready_To_Be_Generated()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);

            var matchGame = MatchGameFaker.Generate();
            await _feature.MatchGameRepository.CreateAsync(matchGame, TestData.WindowsTimezone, false, default);

            var game = await _feature.MatchGameRepository.GetAsync(matchGame.Uid, default);

            await _feature.MatchGameRepository.SetAsReadyToBeGeneratedAsync(game, default);

            var updatedGame = await _feature.MatchGameRepository.GetAsync(matchGame.Uid, default);

            updatedGame.ReadyToBeGenerated.Should().BeTrue();
        }

        [Fact]
        public async Task A_Generated_Match_Game_Should_Not_Be_Set_As_Ready_To_Be_Generated()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);

            var matchGame = MatchGameFaker.Generate();
            await _feature.MatchGameRepository.CreateAsync(matchGame, TestData.WindowsTimezone, false, default);

            var game = await _feature.MatchGameRepository.GetAsync(matchGame.Uid, default);

            await _feature.MatchGameRepository.SetAsGeneratedAsync(game, default);
            await _feature.MatchGameRepository.SetAsReadyToBeGeneratedAsync(game, default);

            game = await _feature.MatchGameRepository.GetAsync(game.Uid, default);

            game.HasBeenGenerated.Should().BeTrue();
            game.ReadyToBeGenerated.Should().BeFalse();
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
