using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.MatchGames;
using hrvster.Infrastructure.Reporting;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Reporting
{
	public interface IReportingService
	{
		Task SaveMatchGameAsync(Guid gameUid, Guid gameplayUid, Guid boardUid, UserIdentifierType userIdentifierType, string screenName, string externalId, CancellationToken cancellationToken);
	}

	public class ReportingService : IReportingService
	{
		private readonly ILogger<ReportingService> _logger;
		private readonly IMatchGamePlayReportingRepository _matchGamePlayReportingRepository;
		private readonly IMatchGamesService _matchGameService;

		public ReportingService(
			ILoggerFactory loggerFactory,
			IMatchGamePlayReportingRepository matchGamePlayReportingRepository,
			IMatchGamesService matchGamesService)
		{
			_logger = loggerFactory.CreateLogger<ReportingService>();
			_matchGamePlayReportingRepository = matchGamePlayReportingRepository;
			_matchGameService = matchGamesService;
		}

		public async Task SaveMatchGameAsync(
			Guid gameUid, 
			Guid gameplayUid, 
			Guid boardUid,
			UserIdentifierType userIdentifierType,
			string screenName,
			string externalId,
			CancellationToken cancellationToken)
		{
			var game = await _matchGameService.GetMatchGameAsync(gameUid, cancellationToken);
			if(game == null)
			{
				_logger.LogWarning("Could not find game with uid {gameUid} while trying to save the MatchGame played into the reporting system.", gameUid);

				return;
			}

			await _matchGamePlayReportingRepository.CreateAsync(
				gameUid, 
				game.CampaignUid, 
				gameplayUid, 
				boardUid, 
				screenName, 
				externalId,
				userIdentifierType
				);
		}
	}
}
