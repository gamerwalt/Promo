using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.MatchGames
{
	public interface IMatchRulesService
	{
		Task<IEnumerable<Rule>> GetAsync(Guid gameUid, CancellationToken cancellationToken);
		IEnumerable<Rule> GetRulesMatching(string watchValue);
	}

	public class MatchRulesService : IMatchRulesService
	{
		private readonly ILogger<MatchRulesService> _logger;
		private readonly IMatchGameRulesRepository _matchGameRulesRepository;

		public MatchRulesService(ILoggerFactory loggerFactory, IMatchGameRulesRepository matchGameRulesRepository)
		{
			_logger = loggerFactory.CreateLogger<MatchRulesService>();
			_matchGameRulesRepository = matchGameRulesRepository;
		}

		public async Task<IEnumerable<Rule>> GetAsync(Guid gameUid, CancellationToken cancellationToken)
			=> await _matchGameRulesRepository.GetAllRulesAsync(gameUid, cancellationToken);

		public IEnumerable<Rule> GetRulesMatching(string watchValue)
			=> _matchGameRulesRepository.GetRulesMatching(watchValue);
	}
}
