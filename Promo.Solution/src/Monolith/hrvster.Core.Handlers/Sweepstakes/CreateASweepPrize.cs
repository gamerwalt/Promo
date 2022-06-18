using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Extensions;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class CreateASweepPrize : AsyncRequestHandler<CreateASweepPrizeRequest>
	{
		private readonly ILogger<CreateASweepPrize> _logger;
		private readonly ISweepPrizesRepository _repository;

		public CreateASweepPrize(ILoggerFactory loggerFactory, ISweepPrizesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateASweepPrize>();
			_repository = repository;
		}

		protected override async Task Handle(CreateASweepPrizeRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Creating a prize for the sweepstake with uid {sweepUid}. Request: {request}",
				request.SweepUid,
				request.ToJsonString());

			var prizeUid = Guid.NewGuid();
			await _repository.CreateSweepPrizeAsync(new Models.SweepPrize
			{
				PrizeUid = prizeUid,
				PrizeName = request.PrizeName,
				PrizeDescription = request.PrizeDescription,
				PrizeValue = request.MonetaryValue,
				PrizePosition = request.Position,
				SweepUid = request.SweepUid,
				Enabled = true,
				Locale = CultureInfo.CurrentCulture.EnglishName
			});
		}
	}
}
