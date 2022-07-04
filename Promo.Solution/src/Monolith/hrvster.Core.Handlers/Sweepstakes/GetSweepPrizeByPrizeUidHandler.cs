using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetSweepPrizeByPrizeUidHandler : IRequestHandler<GetSweepPrizeByPrizeUid, SweepPrize>
	{
		private readonly ILogger<GetSweepPrizeByPrizeUidHandler> _logger;
		private readonly ISweepPrizesRepository _repository;

		public GetSweepPrizeByPrizeUidHandler(ILoggerFactory loggerFactory, ISweepPrizesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepPrizeByPrizeUidHandler>();
			_repository = repository;
		}

		public async Task<SweepPrize> Handle(GetSweepPrizeByPrizeUid request, CancellationToken cancellationToken)
			=> await _repository.GetSweepPrizeAsync(request.PrizeUid);

	}
}
