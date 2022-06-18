using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Web.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetSweepPrizesBySweepUid : IRequestHandler<GetSweepPrizesBySweepUidRequest, SweepPrizes>
	{
		private readonly ILogger<GetSweepPrizesBySweepUid> _logger;
		private readonly ISweepPrizesRepository _repository;

		public GetSweepPrizesBySweepUid(ILoggerFactory loggerFactory, ISweepPrizesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepPrizesBySweepUid>();
			_repository = repository;
		}

		public async Task<SweepPrizes> Handle(GetSweepPrizesBySweepUidRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling {className} to retrieve prizes for sweeps with uid {sweepUid}",
				nameof(GetSweepPrizesBySweepUid),
				request.SweepUid);

			var prizes = await _repository.GetSweepPrizesAsync(request.SweepUid);

			return new SweepPrizes
			{
				Prizes = prizes.OrderBy(c => c.PrizePosition).ToList(),
				SweepUid = request.SweepUid
			};
		}
	}
}
