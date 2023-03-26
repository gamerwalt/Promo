using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace hrvster.ApplicationServices.MatchGames
{
	public interface IMatchRulesService
	{
		IEnumerable<MatchGameRule> GetRulesMatching(string watchValue);
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


		public IEnumerable<MatchGameRule> GetRulesMatching(string watchValue)
			=> _matchGameRulesRepository.GetRulesMatching(watchValue);
	}
}
