using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class UpdateSweepPrizeHandler : AsyncRequestHandler<UpdateSweepPrize>
	{
		private readonly ILogger<UpdateSweepPrizeHandler> _logger;
		private readonly ISweepPrizesRepository _repository;

		public UpdateSweepPrizeHandler(ILoggerFactory loggerFactory, ISweepPrizesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpdateSweepPrizeHandler>();
			_repository = repository;
		}
		protected override async Task Handle(UpdateSweepPrize request, CancellationToken cancellationToken)
		{
			await _repository.UpdateSweepPrize(request.PrizeUid, request.PrizeName, request.PrizeDescription, request.MonetaryValue);
		}
	}
}
