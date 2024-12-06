using hrvster.Infrastructure.SpinToWinRepository;
using hrvster.Models.SpinToWin;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.SpinToWin
{
	public interface ISpinToWinService
	{
		Task<IEnumerable<SpinToWinGame>> GetAllForAccountAsync(Guid userUid);
		IEnumerable<SpinToWinGame> GetAllForAccount(Guid userUid);
		Task CreateSpinToWinGameAsync(SpinToWinGame spinToWinGame, CancellationToken cancellationToken);
	}

	public class SpinToWinService : ISpinToWinService
	{
		private readonly ILogger<SpinToWinService> _logger;
		private readonly ISpinToWinRepository _repository;

		public SpinToWinService(ILoggerFactory loggerFactory, ISpinToWinRepository repository)
		{
			_logger = loggerFactory.CreateLogger<SpinToWinService>();
			_repository = repository;
		}

		public Task CreateSpinToWinGameAsync(SpinToWinGame spinToWinGame, CancellationToken cancellationToken)
		{
			return _repository.CreateSpinToWinGameAsync(spinToWinGame, cancellationToken);
		}

		public IEnumerable<SpinToWinGame> GetAllForAccount(Guid userUid)
		{
			return _repository.GetAllForAccount(userUid);
		}

		public async Task<IEnumerable<SpinToWinGame>> GetAllForAccountAsync(Guid userUid)
		{
			return await _repository.GetAllForAccountAsync(userUid);
		}
	}
}