using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Prizes.Core.Notifications;
using Promo.Prizes.Core.Request;
using Promo.Prizes.Infrastructure;
using Promo.Prizes.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Prizes.Core.RequestHandlers
{
	public class CreateSweepPrizeRequest : IRequest<IEnumerable<SweepPrize>>
	{
		private List<CreateSweepPrize> sweepPrizes;
		private Guid sweepUid;

		public CreateSweepPrizeRequest(Guid sweepUid, List<CreateSweepPrize> sweepPrizes)
		{
			this.sweepUid = sweepUid;
			this.sweepPrizes = sweepPrizes;
		}

		public List<CreateSweepPrize> SweepPrizes
		{
			get { return sweepPrizes; }
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}

	public class CreateSweepPrizeRequestHandler : IRequestHandler<CreateSweepPrizeRequest, IEnumerable<SweepPrize>>
	{
		private readonly ISweepPrizesRepository _repository;
		private readonly IMediator _mediator;
		private readonly ILogger<CreateSweepPrizeRequestHandler> _logger;

		public CreateSweepPrizeRequestHandler(ISweepPrizesRepository repository, IMediator mediator, ILoggerFactory loggerFactory)
		{
			_repository = repository;
			_mediator = mediator;
			_logger = loggerFactory.CreateLogger<CreateSweepPrizeRequestHandler>();
		}

		public async Task<IEnumerable<SweepPrize>> Handle(CreateSweepPrizeRequest request, CancellationToken cancellationToken)
		{
			var sweepPrizes = new List<SweepPrize>();

			foreach(var item in request.SweepPrizes)
			{
				_logger.LogInformation("Creating prize {prizeName} for Sweep with uid {sweepUid}.", item.PrizeName, request.SweepUid);
				
				var prizeUid = Guid.NewGuid();
				await CreateSweepPrizeAsync(request.SweepUid, prizeUid, item);

				_logger.LogInformation("Prize {prizeName} created successfully for sweep with uid {sweepUid}", item.PrizeName, request.SweepUid);

				sweepPrizes.Add(await _repository.GetSweepPrizeAsync(prizeUid));
			}

			return sweepPrizes;
		}

		private async Task CreateSweepPrizeAsync(Guid sweepUid, Guid prizeUid, CreateSweepPrize item)
		{
			var sweepPrize = new SweepPrize
			{
				PrizeUid = prizeUid,
				SweepUid = sweepUid,
				PrizePosition = item.PrizePosition,
				Enabled = item.Enabled,
				PrizeName = item.PrizeName,
				PrizeDescription = item.PrizeDescription,
				PrizeValue = item.PrizeValue,
				Locale = item.Locale
			};

			await _repository.CreateSweepPrizeAsync(sweepPrize);

			await _mediator.Publish(new SweepPrizeCreated(sweepPrize));
		}
	}
}
