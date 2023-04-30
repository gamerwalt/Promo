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
		Task<Rule> GetRuleAsync(Guid ruleUid, CancellationToken cancellationToken);
		IEnumerable<Rule> GetRulesMatching(string watchValue);
		Task<IEnumerable<Rule>> GetRulesMatchingAsync(string watchValue, CancellationToken cancellationToken);
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

		public async Task<Rule> GetRuleAsync(Guid ruleUid, CancellationToken cancellationToken)
			=> await _matchGameRulesRepository.GetAsync(ruleUid);

		public IEnumerable<Rule> GetRulesMatching(string watchValue)
			=> _matchGameRulesRepository.GetRulesMatching(watchValue);

		public Task<IEnumerable<Rule>> GetRulesMatchingAsync(string watchValue, CancellationToken cancellationToken)
			=> _matchGameRulesRepository.GetRulesMatchingAsync(watchValue, cancellationToken);
	}
}
