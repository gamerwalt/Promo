using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using hrvster.Models.Sweepstakes;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class GetSweepstakesRequiringADraw : IRequestHandler<GetSweepstakesRequiringADrawRequest, IEnumerable<SweepDrawRule>>
	{
		private readonly ILogger<GetSweepstakesRequiringADraw> _logger;
		private readonly ISweepsRepository _sweepsRepository;

		public GetSweepstakesRequiringADraw(
			ILoggerFactory loggerFactory,
			ISweepsRepository sweepsRepository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepstakesRequiringADraw>();
			_sweepsRepository = sweepsRepository;
		}

		public async Task<IEnumerable<SweepDrawRule>> Handle(GetSweepstakesRequiringADrawRequest request, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Getting sweepstakes requiring a draw.");

			var sweeps = await _sweepsRepository.GetSweepsRequiringADrawAsync();

			_logger.LogDebug("Retrieved {count} sweeps requiring a draw.", sweeps.Count());

			if(sweeps == null || sweeps.Count() == 0)
				return new List<SweepDrawRule>();

			return new List<SweepDrawRule>();
		}
	}
}
