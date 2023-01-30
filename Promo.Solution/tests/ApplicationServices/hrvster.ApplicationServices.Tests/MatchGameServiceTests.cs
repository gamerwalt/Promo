using Bogus;
using FluentAssertions;
using hrvster.ApplicationServices.Tests.Common;
using hrvster.Games.Match.Models;
using hrvster.Models;
using hrvster.Web.Mvc.Common.Steps;
using TestUtilities.Data;
using Xunit;

namespace hrvster.ApplicationServices.Tests
{
    [Collection("Sequential")]
    public class MatchGameServiceTests : MatchGameTestsAbstractBase, IClassFixture<MatchGameFeature>
    {
        private readonly MatchGameFeature _feature;
        private Faker<MatchGame> MatchGameFaker;

        public MatchGameServiceTests(MatchGameFeature feature)
            => _feature = feature;

        [Fact]
        public async Task Should_Be_Able_To_Retrieve_MatchGames_Created_Under_A_Campaign()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);

            //CreateFakeMatchGame(campaign);
            //var matchGame = MatchGameFaker.Generate();

            var matchGames = await _feature.MatchGamesService.GetCampaignMatchGamesAsync(campaign.CampaignUid, default);
            matchGames.Count().Should().Be(0);
        }

        [Fact]
        public async Task Should_Be_Able_To_Add_A_MatchGame_Using_The_Service()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);
            var matchGame = MatchGameFaker.Generate();
            var matchGame2 = MatchGameFaker.Generate();
            await _feature.MatchGamesService.CreateMatchGameAsync(matchGame, TestData.WindowsTimezone, false, default);
            await _feature.MatchGamesService.CreateMatchGameAsync(matchGame2, TestData.WindowsTimezone, false, default);

            var matchGames = await _feature.MatchGamesService.GetCampaignMatchGamesAsync(campaign.CampaignUid, default);
            matchGames.Count().Should().Be(2);

            matchGames = await _feature.MatchGamesService.GetCampaignMatchGamesAsync(Guid.NewGuid(), default);
            matchGames.Count().Should().Be(0);
        }

        [Fact]
        public async Task Should_Be_Able_To_Update_A_MatchGame()
        {
            _feature.GivenACleanSlate();

            var updatedName = "NewName";
            var updatedDescription = "New Description";
            var updatedNumberOfBoards = 111;

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);
            var matchGame = MatchGameFaker.Generate();

            await _feature.MatchGamesService.CreateMatchGameAsync(matchGame, TestData.WindowsTimezone, false, default);
            matchGame.Name = updatedName;
            matchGame.Description = updatedDescription;
            matchGame.NumberOfBoards = updatedNumberOfBoards;
            await _feature.MatchGamesService.UpdateMatchGameAsync(matchGame, TestData.WindowsTimezone, false, default);
            var updatedMatchGame = await _feature.MatchGamesService.GetMatchGameAsync(matchGame.Uid, default);

            updatedMatchGame.Name.Should().Be(updatedName);
            updatedMatchGame.Description.Should().Be(updatedDescription);
            updatedMatchGame.NumberOfBoards.Should().Be(updatedNumberOfBoards);
        }

        [Fact]
        public async Task Should_Be_Able_To_Set_A_MatchGame_As_Processed()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);
            var matchGame = MatchGameFaker.Generate();
            await _feature.MatchGamesService.CreateMatchGameAsync(matchGame, TestData.WindowsTimezone, false, default);

            await _feature.MatchGamesService.SetMatchGameAsProcessedAsync(matchGame, default);

            var updatedMatchGame = await _feature.MatchGamesService.GetMatchGameAsync(matchGame.Uid, default);
            updatedMatchGame.HasBeenGenerated.Should().BeTrue();
        }

        [Fact]
        public async Task When_A_MatchGame_Is_Generated_We_Should_Not_Be_Able_To_Update_It()
        {
            _feature.GivenACleanSlate();

            var updatedName = "Updated Name";
            var originalName = string.Empty;

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);
            var matchGame = MatchGameFaker.Generate();
            originalName = matchGame.Name;
            await _feature.MatchGamesService.CreateMatchGameAsync(matchGame, TestData.WindowsTimezone, false, default);

            await _feature.MatchGamesService.SetMatchGameAsProcessedAsync(matchGame, default);

            var game = await _feature.MatchGamesService.GetMatchGameAsync(matchGame.Uid, default);
            game.Name = updatedName;
            await _feature.MatchGamesService.UpdateMatchGameAsync(matchGame, TestData.WindowsTimezone, false, default);

            var updatedGame = await _feature.MatchGamesService.GetMatchGameAsync(matchGame.Uid, default);

            updatedGame.Name.Should().Be(originalName);
        }

        [Fact]
        public async Task Should_Be_Able_To_Set_A_Match_Game_As_Ready_To_Be_Processed()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);
            var matchGame = MatchGameFaker.Generate();
            await _feature.MatchGamesService.CreateMatchGameAsync(matchGame, TestData.WindowsTimezone, false, default);

            await _feature.MatchGamesService.SetAsReadyToBeProcessedAsync(matchGame, default);

            var game = await _feature.MatchGamesService.GetMatchGameAsync(matchGame.Uid, default);
            game.ReadyToBeGenerated.Should().BeTrue();
        }

        [Fact]
        public async Task If_The_Match_Game_Is_Already_Generated_It_Should_Not_Set_It_As_Ready_To_Be_Generated()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);
            var matchGame = MatchGameFaker.Generate();
            await _feature.MatchGamesService.CreateMatchGameAsync(matchGame, TestData.WindowsTimezone, false, default);

            await _feature.MatchGamesService.SetAsReadyToBeProcessedAsync(matchGame, default);
            await _feature.MatchGamesService.SetMatchGameAsProcessedAsync(matchGame, default);

            var game = await _feature.MatchGamesService.GetMatchGameAsync(matchGame.Uid, default);
            game.ReadyToBeGenerated.Should().BeFalse();
        }

        [Fact]
        public async Task If_The_Match_Game_Is_Already_Generated_It_Should_Not_Set_It_As_Ready_To_Be_Generated2()
        {
            _feature.GivenACleanSlate();

            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            CreateFakeMatchGame(campaign);
            var matchGame = MatchGameFaker.Generate();
            await _feature.MatchGamesService.CreateMatchGameAsync(matchGame, TestData.WindowsTimezone, false, default);

            await _feature.MatchGamesService.SetAsReadyToBeProcessedAsync(matchGame, default);
            await _feature.MatchGamesService.SetMatchGameAsProcessedAsync(matchGame, default);
            await _feature.MatchGamesService.SetAsReadyToBeProcessedAsync(matchGame, default);

            var game = await _feature.MatchGamesService.GetMatchGameAsync(matchGame.Uid, default);
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
