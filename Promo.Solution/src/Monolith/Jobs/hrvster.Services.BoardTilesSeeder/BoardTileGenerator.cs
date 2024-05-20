using hrvster.ApplicationServices.JobContracts;
using hrvster.ApplicationServices.MatchGames;

namespace hrvster.Services.BoardTilesSeeder
{
	public class BoardTileGenerator : IService
	{
		private readonly ILogger<BoardTileGenerator> _logger;
		private readonly IMatchGamesService _matchGamesService;
		private readonly ApplicationServices.MatchGames.V2.IMatchGameBoardService _matchGameBoardService;

		public BoardTileGenerator(
			ILoggerFactory loggerFactory, 
			IMatchGamesService matchGamesService,
			ApplicationServices.MatchGames.V2.IMatchGameBoardService matchGameBoardService)
		{
			_logger = loggerFactory.CreateLogger<BoardTileGenerator>();
			_matchGamesService = matchGamesService;
			_matchGameBoardService = matchGameBoardService;
		}

		public async Task ProcessService(CancellationToken token)
		{
			_logger.LogInformation("Processing started for BoardTileGenerator.");

			var matchGames = await _matchGamesService.GetGamesReadyForTileCreationAsync(token);

			foreach(var matchGame in matchGames)
			{
				_logger.LogInformation("Generating tiles for the game {GameName}", matchGame.Name);

				await _matchGameBoardService.GenerateTilesForBoardAsync(matchGame, token);

				await _matchGamesService.SetAsTilesGeneratedAsync(matchGame, token);

				_logger.LogInformation("Finished generating tiles for the game {GameName}", matchGame.Name);
			}

			_logger.LogInformation("Processing completed for BoardTileGenerator.");
		}
	}
}
