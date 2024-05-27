using hrvster.ApplicationServices.JobContracts;
using hrvster.ApplicationServices.MatchGames;

namespace hrvster.Services.BoardAndPrizeSeeder
{
	public class BoardAndPrizeGenerator : IService
	{
		private readonly ILogger<BoardAndPrizeGenerator> _logger;
		private readonly IMatchGameReadinessService _matchGameReadinessService;
		private readonly IMatchGamesService _matchGamesService;
		private readonly IMatchGameBoardService _matchGameBoardService;
		private readonly IConfiguration _config;

		public BoardAndPrizeGenerator(
			ILoggerFactory loggerFactory,
			IMatchGameReadinessService matchGameReadinessService,
			IMatchGamesService matchGamesService,
			IMatchGameBoardService matchGameBoardService,
			IConfiguration config)
		{
			_logger = loggerFactory.CreateLogger<BoardAndPrizeGenerator>();
			_matchGameReadinessService = matchGameReadinessService;
			_matchGamesService = matchGamesService;
			_matchGameBoardService = matchGameBoardService;
			_config = config;
		}

		public async Task ProcessService(CancellationToken token)
		{
			try
			{
				_logger.LogInformation("Processing the Match Game Board Service.");

				bool.TryParse(_config["LoadOnlyBoardsToRedis"], out var loadOnlyBoards);

				if(!loadOnlyBoards)
				{
					var matchGames = await _matchGamesService.GetMatchGamesToGenerateBoardAsync(token);

					_logger.LogInformation("The number of games ready for board generation is {Total}", matchGames.Count());

					foreach (var matchGame in matchGames)
					{
						_logger.LogInformation("Processing for match game with uid {MatchGameUid} and name {MatchGameName}",
						matchGame.Uid,
						matchGame.Name);

						if (await _matchGameReadinessService.IsReadyToGenerateBoardsAsync(matchGame.Uid, token))
						{
							_logger.LogInformation("The game with uid {GameUid} and name {MatchGameName} is ready to generate boards.",
								matchGame.Uid,
							matchGame.Name);

							if (!matchGame.HasBeenGenerated)
							{
								await _matchGameBoardService.GenerateBoardsWithNoTilesAsync(matchGame, token);

								_logger.LogInformation("Boards generated for game with uid {GameUid} and name {MatchGameName}",
									matchGame.Uid,
									matchGame.Name);
							}

							continue;
						}

						_logger.LogWarning("The game with uid {MatchGameUid} and name {MatchGameName} is NOT ready to generate boards.",
							matchGame.Uid,
							matchGame.Name);

						//TODO: Notify account holder about the issue for this game

						_logger.LogInformation("Finished processing for match game with uid {MatchGameUid} and name {MatchGameName}",
							matchGame.Uid,
							matchGame.Name);
					}

					_logger.LogInformation("Finished processing the Game Board Service");
				}
				else
				{
					var gameIds = _config["GameIdsToLoadIntoRedis"];
					if(string.IsNullOrEmpty(gameIds))
					{
						return;
					}

					var GameIdsToLoad = gameIds.Split(',').Select(int.Parse).ToList();

					foreach(var gameId in GameIdsToLoad)
					{
						_logger.LogInformation("Loading the boards for the game {GameId}", gameId);

						await _matchGameBoardService.LoadBoardUidsIntoRedisAsync(gameId, token);

						_logger.LogInformation("Loading the boards for the game {GameId} completed.", gameId);
					}
				}
				
			}
			catch (Exception ex)
			{
				_logger.LogError("An exception happened while processing the service for {ServiceName}", nameof(BoardAndPrizeGenerator));
				_logger.LogError("Exception Message: {Message}", ex.Message);
				_logger.LogError("Inner Exception Message: {Message}", ex.InnerException?.Message);

				throw;
			}
		}
	}
}
