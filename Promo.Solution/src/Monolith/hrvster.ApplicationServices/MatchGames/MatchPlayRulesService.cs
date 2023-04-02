using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.MatchGames
{
	public interface IMatchPlayRulesService
	{
		Task CreateAsync(Guid uid, int playPerUser, int playPerTweet, CancellationToken cancellationToken);
		Task<MatchPlayRule> GetAllForMatchGameAsync(Guid uid, CancellationToken cancellationToken);
	}

	public class MatchPlayRulesService : IMatchPlayRulesService
	{
		private readonly ILogger<MatchPlayRulesService> _logger;
		private readonly IMatchPlayRulesRepository _repository;

		public MatchPlayRulesService(
			ILoggerFactory loggerFactory,
			IMatchPlayRulesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<MatchPlayRulesService>();
			_repository = repository;
		}

		public Task CreateAsync(Guid uid, int playPerUser, int playPerTweet, CancellationToken cancellationToken)
			=> _repository.CreateAsync(matchGameUid: uid, playPerUser, playPerTweet, cancellationToken);

		public Task<MatchPlayRule> GetAllForMatchGameAsync(Guid uid, CancellationToken cancellationToken)
			=> _repository.GetAsync(uid, cancellationToken);
	}
}
