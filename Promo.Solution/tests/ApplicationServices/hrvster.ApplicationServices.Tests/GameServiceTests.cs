using Bogus;
using FluentAssertions;
using hrvster.ApplicationServices.Tests.Common;
using hrvster.Core.Clients.Common;
using hrvster.Games.Match.Models;
using hrvster.Models;
using hrvster.Web.Mvc.Common.Steps;
using Microsoft.AspNetCore.Http;
using Moq;
using TestUtilities.Data;
using TestUtilities.Mocks.Publishers;
using Xunit;

namespace hrvster.ApplicationServices.Tests
{
    [Collection("Sequential")]
    public class GameServiceTests : MatchGameTestsAbstractBase, IClassFixture<GameFeature>
    {
        private readonly GameFeature _feature;
        private Faker<MatchGame> MatchGameFaker;


        public GameServiceTests(GameFeature feature)
            => _feature = feature;

        [Fact]
        public async Task Should_Be_Able_To_Publish_A_Tweet_When_We_CreateARuleTweet()
        {
            _feature.GivenACleanSlate();

            var matchGame = await SetupMatchGameAsync();

            var emptyFile = new FormFile(Stream.Null, 0, 0, null, "");

            await _feature.GameService.CreateRulePostAsync(
                ExternalClientType.Twitter.ToString(),
                _feature.hrvsterUser,
                GameTypesEnum.MatchGames.ToString(),
                matchGame.Uid,
                matchGame.Id,
                TestData.WatchTypeHashtagId,
                TestData.Hashtag,
                TestData.TweetText,
                true,
                DateTime.UtcNow,
                emptyFile,
                emptyFile,
                emptyFile,
                emptyFile,
                TestData.PageId,
                true);

            _feature.MockedTwitterClient.Verify(m => m.PublishAsync(It.IsAny<Message>()), Times.Exactly(1));
        }

        [Fact]
        public async Task Should_Save_The_Profile_UserExternalId_Into_The_Blacklisted_Users_Table()
        {
            _feature.GivenACleanSlate();

            var matchGame = await SetupMatchGameAsync();

            var emptyFile = new FormFile(Stream.Null, 0, 0, null, "");

            await _feature.GameService.CreateRulePostAsync(
                ExternalClientType.Twitter.ToString(),
				_feature.hrvsterUser,
                GameTypesEnum.MatchGames.ToString(),
                matchGame.Uid,
                matchGame.Id,
                TestData.WatchTypeHashtagId,
                TestData.Hashtag,
                TestData.TweetText,
                true,
                DateTime.UtcNow,
                emptyFile,
                emptyFile,
                emptyFile,
                emptyFile,
				TestData.PageId,
				true);

            var blacklistedUsers = await _feature.BlacklistedUsersRepository.GetBlacklistedUsersAsync(matchGame.Uid);

            blacklistedUsers.Count().Should().Be(1);
        }

        [Fact]
        public async Task Should_Save_The_Match_Game_Rule()
        {
            _feature.GivenACleanSlate();

            var matchGame = await SetupMatchGameAsync();

            var emptyFile = new FormFile(Stream.Null, 0, 0, null, "");

            await _feature.GameService.CreateRulePostAsync(
				ExternalClientType.Twitter.ToString(),
				_feature.hrvsterUser,
                GameTypesEnum.MatchGames.ToString(),
                matchGame.Uid,
                matchGame.Id,
                TestData.WatchTypeHashtagId,
                TestData.Hashtag,
                TestData.TweetText,
                true,
                DateTime.UtcNow,
                emptyFile,
                emptyFile,
                emptyFile,
                emptyFile,
				TestData.PageId,
				true);

            var matchGameRules = await _feature.MatchGameRulesRepository.GetAllRulesAsync(matchGame.Uid, default);

            matchGameRules.Should().NotBeNull();
            matchGameRules.Count().Should().Be(1);
        }

        [Fact]
        public async Task Should_Save_The_Match_Game_Rules_And_Should_Be_Able_To_Retrieve_Them()
        {
            _feature.GivenACleanSlate();

            var matchGame = await SetupMatchGameAsync();

            var emptyFile = new FormFile(Stream.Null, 0, 0, null, "");

            await _feature.GameService.CreateRulePostAsync(
				ExternalClientType.Twitter.ToString(),
				_feature.hrvsterUser,
                GameTypesEnum.MatchGames.ToString(),
                matchGame.Uid,
                matchGame.Id,
                TestData.WatchTypeHashtagId,
                TestData.Hashtag,
                TestData.TweetText,
                true,
                DateTime.UtcNow,
                emptyFile,
                emptyFile,
                emptyFile,
                emptyFile,
				TestData.PageId,
				true);

            await _feature.GameService.CreateRulePostAsync(
				ExternalClientType.Twitter.ToString(),
				_feature.hrvsterUser,
                GameTypesEnum.MatchGames.ToString(),
                matchGame.Uid,
                matchGame.Id,
                TestData.WatchTypeHashtagId,
                TestData.Hashtag2,
                TestData.TweetText2,
                true,
                DateTime.UtcNow,
                emptyFile,
                emptyFile,
                emptyFile,
                emptyFile,
				TestData.PageId,
				true);

            var matchGameRules = await _feature.MatchGameRulesRepository.GetAllRulesAsync(matchGame.Uid, default);

            matchGameRules.Should().NotBeNull();
            matchGameRules.Count().Should().Be(2);
        }

        private async Task<MatchGame> SetupMatchGameAsync()
        {
            var account = await BuildAccountAndSubscribeToBasicPlan(_feature);
            var campaign = await BuildAndCreateCampaignAsync(_feature, account);
            var profile = BuildNewTwitterProfile(account);
            await _feature.ProfilesRepository.UpsertProfileAsync(profile);

            CreateFakeMatchGame(campaign);
            var matchGame = MatchGameFaker.Generate();

            await _feature.MatchGamesService.CreateMatchGameAsync(matchGame, TestData.WindowsTimezone, false, default);
            matchGame = await _feature.MatchGamesService.GetMatchGameAsync(matchGame.Uid, default);

            _feature.MockedTwitterClient.Reset();
            _feature.MockedTwitterClient.SetupPublishAsync();

            return matchGame;
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
