using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.Models;
using Promo.Harvester.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Harvester.Core.RequestHandlers
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
		private readonly ISweepPrizeRepository _repository;

		public CreateSweepPrizeRequestHandler(ILoggerFactory loggerFactory, ISweepPrizeRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepPrizeRequestHandler>();
			_repository = repository;
		}

		public async Task<SweepPrize> Handle(CreateSweepPrize request, CancellationToken cancellationToken)
		{
			await _repository.CreateSweepPrizeAsync(new Infrastructure.Models.SweepPrize
			{
				PrizeUid = request.SweepPrize.PrizeUid,
				PrizePosition = request.SweepPrize.PrizePosition,
				SweepUid = request.SweepUid,
				Enabled = request.SweepPrize.Enabled,
				PrizeName = request.SweepPrize.PrizeName,
				PrizeValue = request.SweepPrize.PrizeValue
			});

			return request.SweepPrize;
		}
	}
}
