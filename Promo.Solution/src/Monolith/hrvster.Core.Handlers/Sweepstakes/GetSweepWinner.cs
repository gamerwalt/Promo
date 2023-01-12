using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
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
			=> await _repository.GetSweepWinnerAsync(request.SweepWinnerUid);
	}
}
