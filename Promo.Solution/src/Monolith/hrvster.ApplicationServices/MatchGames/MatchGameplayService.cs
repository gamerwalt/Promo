using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MatchGames;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.MatchGames
{
	public interface IMatchGameplayService
	{
		Task<MatchPlay> GetAsync(Guid gameplayUid, CancellationToken cancellationToken);
	}

	public class MatchGameplayService : IMatchGameplayService
	{
		private readonly IMatchGamePlayRepository _repository;

		public MatchGameplayService(IMatchGamePlayRepository repository)
		{
			_repository = repository;
		}

		public async Task<MatchPlay> GetAsync(Guid gameplayUid, CancellationToken cancellationToken)
			=> await _repository.GetAsync(gameplayUid, cancellationToken);
	}
}
