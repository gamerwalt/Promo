using hrvster.Games.Match.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.MatchGames
{
	public interface IMatchGameBoardService
	{
		Task GenerateBoardsAsync(MatchGame matchGame, CancellationToken cancellationToken);
	}

	public class MatchGameBoardService : IMatchGameBoardService
	{
		private readonly ILogger<MatchGameBoardService> _logger;
		private readonly IMatchPrizeService _matchPrizeService;

		private const double MinWinProbability = 16.6666;
		private const int MaxWinProbability = 6;

		public MatchGameBoardService(
			ILoggerFactory loggerFactory,
			IMatchPrizeService matchPrizeService)
		{
			_logger = loggerFactory.CreateLogger<MatchGameBoardService>();
			_matchPrizeService = matchPrizeService;
		}

		public async Task GenerateBoardsAsync(MatchGame matchGame, CancellationToken cancellationToken)
		{
			var numberOfBoards = matchGame.NumberOfBoards;
			var numberOfWinnableBoards = matchGame.NumberOfWinnableBoards;
			var random = new Random();
			var generatedBoards = new List<MatchBoard>(numberOfBoards);
			var prizes = await _matchPrizeService.GetAllForMatchGameAsync(matchGame.Uid, cancellationToken);
			var prizeList = prizes.ToList();

			for(var i = 0; i <  numberOfWinnableBoards; i++)
			{
				double winProbability = 
					MinWinProbability + random.NextDouble() * 
					((matchGame.WinProbability/ MaxWinProbability) * 100) - MinWinProbability;

				var randomPrize = prizeList[random.Next(prizeList.Count)];

				generatedBoards.Add(new MatchBoard 
				{ 
					Id = i,
					Uid = Guid.NewGuid(), 
					IsWinnable = true, 
					WinProbability = winProbability, 
					Prize = randomPrize,
					MatchGame = matchGame,
				});
			}
		}
	}
}
