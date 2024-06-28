using Bogus;
using hrvster.ApplicationServices.Tests.Common;
using hrvster.Games.Match.Models;
using hrvster.Models;
using hrvster.Web.Mvc.Common.Steps;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities.Data;
using TestUtilities.Mocks.Publishers;
using Xunit;

namespace hrvster.ApplicationServices.Tests
{
    [Collection("Sequential")]
    public class MatchGameBoardServiceV1Tests : MatchGameTestsAbstractBase, IClassFixture<GameFeature>
    {
        private readonly GameFeature _feature;
        private Faker<MatchGame> MatchGameFaker;
        private Faker<MatchPrize> MatchPrizeFaker;
        private Faker<MatchImage> MatchImageFaker;

        public MatchGameBoardServiceV1Tests(GameFeature feature)
            => _feature = feature;

        [Fact]
        public async Task Should_Be_Able_To_Publish_A_Tweet_When_We_CreateARuleTweet()
        {
            _feature.GivenACleanSlate();

            var matchGame = await SetupMatchGameAsync();
            var matchImages = await SetupPrizesAsync();

            CreateFakePrize(matchGame, matchImages[0]);
            var matchPrize1 = MatchPrizeFaker.Generate();
            matchPrize1.Name = "1k CASH";
            matchPrize1.Quantity = 20;

            CreateFakePrize(matchGame, matchImages[1]);
            var matchPrize2 = MatchPrizeFaker.Generate();
            matchPrize2.Name = "2.5k CASH";
            matchPrize2.Quantity = 15;

            CreateFakePrize(matchGame, matchImages[2]);
            var matchPrize3 = MatchPrizeFaker.Generate();
            matchPrize3.Name = "5k CASH";
            matchPrize3.Quantity = 10;

            CreateFakePrize(matchGame, matchImages[3]);
            var matchPrize4 = MatchPrizeFaker.Generate();
            matchPrize4.Name = "10k CASH";
            matchPrize4.Quantity = 7;

            CreateFakePrize(matchGame, matchImages[4]);
            var matchPrize5 = MatchPrizeFaker.Generate();
            matchPrize5.Name = "15k CASH";
            matchPrize5.Quantity = 4;

            CreateFakePrize(matchGame, matchImages[5]);
            var matchPrize6 = MatchPrizeFaker.Generate();
            matchPrize6.Name = "20k CASH";
            matchPrize6.Quantity = 1;

            CreateFakePrize(matchGame, matchImages[6]);
            var matchPrize7 = MatchPrizeFaker.Generate();
            matchPrize7.Name = "0 CASH";
            matchPrize7.Quantity = 1;
            matchPrize7.IsFiller = true;

            var prizes = new List<MatchPrize>
            {
                matchPrize1,
                matchPrize2,
                matchPrize3,
                matchPrize4,
                matchPrize5,
                matchPrize6,
                matchPrize7
            };

            foreach (var prize in prizes)
            {
                await _feature.MatchPrizeRepository.CreateAsync(prize, default);
            }

            await _feature.MatchGameBoardService.GenerateBoardsWithNoTilesAsync(matchGame, default);
            
        }

        private async Task<List<MatchImage>> SetupPrizesAsync()
        {
            var matchImages = MatchImageFaker.Generate(7);

            foreach (var image in matchImages)
            {
                await _feature.MatchImagesRepository.CreateAsync(image, default);
            }

            return matchImages;
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

            CreateFakeImage(matchGame);

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
                .RuleFor(m => m.NumberOfBoards, f => 150)
                .RuleFor(m => m.TileCountPerBoard, f => 9)
                .RuleFor(m => m.NumberOfWinnableBoards, f => 57)
                .RuleFor(m => m.WinProbability, f => 6)
                .RuleFor(m => m.StartDate, f => DateTime.Now)
                .RuleFor(m => m.EndDate, f => DateTime.Now.AddDays(30));
        }

        private void CreateFakePrize(
            MatchGame matchGame, 
            MatchImage image)
        {
            MatchPrizeFaker = new Faker<MatchPrize>()
                .RuleFor(m => m.Uid, f => Guid.NewGuid())
                .RuleFor(m => m.IsFiller, f => false)
                .RuleFor(m => m.Description, f => f.Lorem.Sentence(2))
                .RuleFor(m => m.Name, f => f.Lorem.Word())
                .RuleFor(m => m.Quantity, f => f.Random.Number(1, 100))
                .RuleFor(m => m.MatchGame, f => matchGame)
                .RuleFor(m => m.Image, f => image)
                .RuleFor(m => m.Value, f => f.Random.Decimal(1, 100));
        }

        private void CreateFakeImage(MatchGame matchGame)
        {
            MatchImageFaker = new Faker<MatchImage>()
                .RuleFor(m => m.Uid, f => Guid.NewGuid())
                .RuleFor(m => m.Name, f => f.Lorem.Word())
                .RuleFor(m => m.MatchGame, f => matchGame)
                .RuleFor(m => m.LocationUrl, f => f.Lorem.Word());
        }
    }
}
