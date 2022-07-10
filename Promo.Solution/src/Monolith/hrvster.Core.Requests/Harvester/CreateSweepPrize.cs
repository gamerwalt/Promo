using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.Harvester
{
	public class CreateSweepPrize : IRequest<SweepPrize>
	{
		private SweepPrize sweepPrize;
		private Guid sweepUid;

		public CreateSweepPrize(SweepPrize sweepPrize, Guid sweepUid)
		{
			this.sweepPrize = sweepPrize;
			this.sweepUid = sweepUid;
		}

		public SweepPrize SweepPrize
		{
			get { return sweepPrize; }
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}

	public class CreateSweepPrizeRequestHandler : IRequestHandler<CreateSweepPrize, SweepPrize>
	{
		private readonly ILogger<CreateSweepPrizeRequestHandler> _logger;
		private readonly ISweepPrizesRepository _repository;

		public CreateSweepPrizeRequestHandler(ILoggerFactory loggerFactory, ISweepPrizesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepPrizeRequestHandler>();
			_repository = repository;
		}

		public async Task<SweepPrize> Handle(CreateSweepPrize request, CancellationToken cancellationToken)
		{
			var sweepPrize = new SweepPrize
			{
				PrizeUid = request.SweepPrize.PrizeUid,
				PrizePosition = request.SweepPrize.PrizePosition,
				SweepUid = request.SweepUid,
				Enabled = request.SweepPrize.Enabled,
				PrizeName = request.SweepPrize.PrizeName,
				PrizeValue = request.SweepPrize.PrizeValue
			};
			await _repository.CreateSweepPrizeAsync(sweepPrize);

			return request.SweepPrize;
		}
	}
}
