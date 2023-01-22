using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetSweepWinner : IRequestHandler<GetSweepWinnerRequest, SweepWinner>
	{
		private readonly ILogger<GetSweepWinner> _logger;
		private readonly ISweepWinnersRepository _repository;

		public GetSweepWinner(ILoggerFactory loggerFactory, ISweepWinnersRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepWinner>();
			_repository = repository;
		}

		public async Task<SweepWinner> Handle(GetSweepWinnerRequest request, CancellationToken cancellationToken)
		{
			var winner = await _repository.GetSweepWinnerAsync(request.SweepWinnerUid);
			var backups = await _repository.GetSweepBackupWinnersAsync(request.SweepWinnerUid);

			winner.BackupWinners = backups.ToList();

			return winner;
		}
	}
}
