using Bogus;
using FluentAssertions;
using hrvster.Games.Match.Models;
using hrvster.Games.Match.Tests.Common;
using Xunit;

namespace hrvster.Games.Match.Tests
{
    [Collection("Sequential")]
    public class MatchEngineTests : IClassFixture<MatchGameFeature>
    {
        private readonly MatchGameFeature _feature;

        public MatchEngineTests(MatchGameFeature feature)
            => _feature = feature;

        [Fact]
        public void Should_Be_Able_To_Create_A_Match_Game_With_Board_And_Tiles()
        {
            var boardCount = 10;
            var tileCount = 9;
            var winCount = 9;

            var boards = _feature.MatchEngine.Create(boardCount, tileCount, winCount);
            var board = boards.FirstOrDefault();
            var winnableBoards = boards.Where(b => b.IsWinnable).ToList();

            boards.Count().Should().Be(boardCount);
            board.Tiles.Length.Should().Be((int)Math.Sqrt(tileCount));
            board.Tiles[0].Length.Should().Be((int)Math.Sqrt(tileCount));
            winnableBoards.Count().Should().Be(winCount);
        }

        [Fact]
        public void Should_Be_Able_To_Create_A_Match_Game_With_Board_And_Tiles_And_Some_Winnable_Boards()
        {
            var boardCount = 100;
            var tileCount = 9;
            var winCount = 5;

            var boards = _feature.MatchEngine.Create(boardCount, tileCount, winCount);
            var board = boards.FirstOrDefault();
            var winnableBoards = boards.Where(b => b.IsWinnable).ToList();

            boards.Count().Should().Be(boardCount);
            board.Tiles.Length.Should().Be((int)Math.Sqrt(tileCount));
            board.Tiles[0].Length.Should().Be((int)Math.Sqrt(tileCount));
            winnableBoards.Count().Should().Be(winCount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        public void Should_Be_Able_To_Set_Tile_Win_Possibility(int winPossibility)
        {
            var boardCount = 100;
            var tileCount = 9;
            var winCount = 5;
            var prize1Count = 2;
            var prize2Count = 3;
            var tileCountRow = Math.Sqrt(tileCount);
            var tileCountCol = Math.Sqrt(tileCount);

            var boards = _feature.MatchEngine.Create(boardCount, tileCount, winCount);

            boards.Any(c => c.Tiles.Any(t => t.Any(c => c.WinIndicator))).Should().BeFalse();

            var prizeFaker = new Faker<MatchPrize>()
                .RuleFor(p => p.Uid, f => Guid.NewGuid())
                .RuleFor(p => p.Name, f => f.Lorem.Random.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(3))
                .RuleFor(p => p.Value, f => f.Random.Number())
				.RuleFor(m => m.Quantity, f => 10)
				.RuleFor(p => p.Image, f => new MatchImage());

            var prize1 = prizeFaker.Generate();
            var prize2 = prizeFaker.Generate();

            var prizeWins = new Dictionary<int, MatchPrizeCount>()
            {
                { 1, new MatchPrizeCount { MatchPrize = prize1, Count = prize1Count } },
                { 2, new MatchPrizeCount { MatchPrize = prize2, Count = prize2Count } },
            };

            _feature.MatchEngine.SetTileWinPossibility(boards.ToList(), winPossibility, prizeWins);

            var winnableBoards = boards.Where(b => b.IsWinnable).ToList();
            winnableBoards.Count().Should().Be(5);
            winnableBoards.Any(c => c.Tiles.Any(t => t.Any(c => c.WinIndicator))).Should().BeTrue();

            var firstWinnableBoard = winnableBoards.FirstOrDefault();
            var tilesOfFirstWinnableBoard = firstWinnableBoard.Tiles;
            var totalWinnableTiles = 0;

            for(var rowIndex = 0; rowIndex < tileCountRow; rowIndex++)
            {
                for(var colIndex = 0; colIndex < tileCountCol; colIndex++)
                {
                    if (tilesOfFirstWinnableBoard[rowIndex][colIndex].WinIndicator)
                    {
                        totalWinnableTiles++;
                    }
                }
            }

            totalWinnableTiles.Should().Be(winPossibility);
        }

        [Fact]
        public void Should_Set_The_Number_Of_Images_Per_Prize()
        {
            var boardCount = 100;
            var tileCount = 9;
            var winCount = 5;
            var prize1Count = 2;
            var prize2Count = 3;
            var tileCountRow = Math.Sqrt(tileCount);
            var tileCountCol = Math.Sqrt(tileCount);
            var winPossibility = 9;

            var prizeFaker = new Faker<MatchPrize>()
                .RuleFor(p => p.Uid, f => Guid.NewGuid())
                .RuleFor(p => p.Name, f => f.Lorem.Random.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(3))
                .RuleFor(p => p.Value, f => f.Random.Number())
				.RuleFor(m => m.Quantity, f => 10)
				.RuleFor(p => p.Image, f => new MatchImage());

            var prize1 = prizeFaker.Generate();
            var prize2 = prizeFaker.Generate();

            var prizeWins = new Dictionary<int, MatchPrizeCount>()
            {
                { 1, new MatchPrizeCount { MatchPrize = prize1, Count = prize1Count } },
                { 2, new MatchPrizeCount { MatchPrize = prize2, Count = prize2Count } },
            };

            var boards = _feature.MatchEngine.Create(boardCount, tileCount, winCount);
            _feature.MatchEngine.SetTileWinPossibility(boards.ToList(), winPossibility, prizeWins);

            var winnableBoards = boards.Where(b => b.IsWinnable).ToList();
            winnableBoards.Count().Should().Be(winCount);
            winnableBoards.Any(c => c.Tiles.Any(t => t.Any(c => c.WinIndicator))).Should().BeTrue();

            var firstWinnableBoard = winnableBoards.FirstOrDefault();
            var tilesOfFirstWinnableBoard = firstWinnableBoard.Tiles;
            var totalWinnableTiles = 0;
            var totalPrizes = 0;

            for (var rowIndex = 0; rowIndex < tileCountRow; rowIndex++)
            {
                for (var colIndex = 0; colIndex < tileCountCol; colIndex++)
                {
                    if (tilesOfFirstWinnableBoard[rowIndex][colIndex].WinIndicator)
                    {
                        totalWinnableTiles++;
                    }
                }
            }

            winnableBoards.Count().Should().Be(winCount);
            totalWinnableTiles.Should().Be(winPossibility);
        }
    }
}
