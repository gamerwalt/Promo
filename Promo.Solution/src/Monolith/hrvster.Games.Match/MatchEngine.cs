using hrvster.Core;
using hrvster.Games.Match.Models;
using hrvster.Games.Match.Models.Utilities;

namespace hrvster.Games.Match
{
	public class MatchEngine
	{
		private readonly IRandomizer _randomizer;

		public MatchEngine(IRandomizer randomizer) 
		{ 
			_randomizer = randomizer;
		}

		public IEnumerable<MatchBoard> Create(int boardCount, int tileCount, int winCount)
		{
			if(!tileCount.IsPerfectSquare())
			{
				throw new ArgumentException("Tilecount is not a perfect square.");
			}

			var boards = new List<MatchBoard>();
			var rowTileCount = (int)Math.Sqrt(tileCount);
			var columnTileCount = (int)Math.Sqrt(tileCount);
			var allWins = boardCount == winCount;

			for (var index = 0; index < boardCount; index++)
			{
				var board = new MatchBoard(rowTileCount, columnTileCount);
				board.Uid = Guid.NewGuid();
				if(allWins)
				{
					board.IsWinnable = true;
				}
				else
				{
					if (_randomizer.GetRandomNumber(0, boardCount - 1) % 2 == 0 && winCount > 0)
					{
						board.IsWinnable = true;
						winCount--;
					}
				}

				boards.Add(board);
			}

			do
			{
				var unWinnableBoards = boards.Where(b => !b.IsWinnable).ToList();
				foreach (var board in unWinnableBoards)
				{
					if (_randomizer.GetRandomNumber(0, boardCount - 1) % 2 == 0 && winCount > 0)
					{
						board.IsWinnable = true;
						winCount--;
					}
				}
			} while (winCount > 0);
			
			return boards;
		}

		public void SetTileWinPossibility(
			List<MatchBoard> boards, 
			int possibility, 
			Dictionary<int, MatchPrizeCount> prizeWins)
		{
			var rowCount = GetTileRowCount(boards);

			foreach (var board in boards)
			{
				if (board.IsWinnable)
				{
					var prizeWin = prizeWins.FirstOrDefault(p => p.Value.Count > 0);
					var prizeWinKey = prizeWin.Key;
					var matchPrizeCount = prizeWin.Value;

					SetTileWinIndicators(board.Tiles, rowCount, possibility, matchPrizeCount.MatchPrize);

					prizeWins[prizeWinKey].Count--;
				}
			}
		}

		private static int GetTileRowCount(List<MatchBoard> boards)
		{
			var tiles = boards.FirstOrDefault().Tiles;

			return tiles.Length;
		}

		private void SetTileWinIndicators(
			MatchTile[][] tiles, int rowCount, int possiblity, MatchPrize matchPrize)
		{
			if(rowCount * rowCount == possiblity)
			{
				for(var rowIndex = 0; rowIndex < rowCount; rowIndex++)
				{
					for(var colIndex = 0; colIndex < rowCount; colIndex++)
					{
						SetTileWinnabilityAndPrizeImage(tiles, matchPrize, rowIndex, colIndex);
					}
				}

				return;
			}

			while(possiblity > 0)
			{
				for (var index = 0; index < rowCount; index++)
				{
					var winnableColumnIndex = _randomizer.GetRandomNumber(0, rowCount);
					var winnableRowIndex = _randomizer.GetRandomNumber(0, rowCount);
					if (winnableColumnIndex >= rowCount) winnableColumnIndex = rowCount - 1;
					if (winnableRowIndex >= rowCount) winnableRowIndex = rowCount - 1;

					var tileWinnable = tiles[winnableRowIndex][winnableColumnIndex].WinIndicator;
					if (!tileWinnable && possiblity > 0)
					{
						SetTileWinnabilityAndPrizeImage(tiles, matchPrize, winnableRowIndex, winnableColumnIndex);
						possiblity--;
					}
				}
			}
		
		}

		private void SetTileWinnabilityAndPrizeImage(MatchTile[][] tiles, MatchPrize matchPrize, int rowIndex, int colIndex)
		{
			SetTilePrizeAndImage(tiles, rowIndex, colIndex, matchPrize);

			
		}

		private static void SetTilePrizeAndImage(
			MatchTile[][] tiles, int rowIndex, int colIndex, MatchPrize matchPrize)
		{
			tiles[rowIndex][colIndex].WinIndicator = true;
			tiles[rowIndex][colIndex].Prize = matchPrize;
			tiles[rowIndex][colIndex].Image = matchPrize.Image;
		}
	}
}