using Bogus;
using FluentAssertions;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Tests.Common;
using hrvster.Models;
using hrvster.Web.Mvc.Common.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Infrastructure.Tests
{
    [Collection("Sequential")]
    public class MatchGamesRepositoryTests : 
        RepositoryAbstractModelBuilder, IClassFixture<RepositoryFeature>
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

        [Fact]
        public async Task Should_Be_Able_To_Create_A_Match_Game()
        {
            //MatchGameFaker = new Faker<MatchGame>()
            //    .RuleFor(m => m.Uid, f => Guid.NewGuid())
            //    .RuleFor(m => m.CampaignId, f => campaign.CampaignId)
            //    .RuleFor(m => m.Name, f => f.Lorem.Word())
            //    .RuleFor(m => m.Description, f => f.Lorem.Sentence(2))
            //    .RuleFor(m => m.)
        }
    }
}
