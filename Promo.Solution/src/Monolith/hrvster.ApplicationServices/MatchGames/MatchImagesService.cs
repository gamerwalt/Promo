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
	public interface IMatchImageService
	{
		Task<IEnumerable<MatchImage>> GetByMatchGameUid(Guid matchGameUid, CancellationToken cancellationToken);
	}

	public class MatchImagesService : IMatchImageService
	{
		private readonly ILogger<MatchImagesService> _logger;
		private readonly IMatchImagesRepository _repository;

		public MatchImagesService(
			ILoggerFactory loggerFactory,
			IMatchImagesRepository repository)
		{ 
			_logger = loggerFactory.CreateLogger<MatchImagesService>();
			_repository = repository;
		}

		public async Task<IEnumerable<MatchImage>> GetByMatchGameUid(Guid matchGameUid, CancellationToken cancellationToken)
			=> await _repository.GetForMatchGamesAsync(matchGameUid, cancellationToken);
	}
}
