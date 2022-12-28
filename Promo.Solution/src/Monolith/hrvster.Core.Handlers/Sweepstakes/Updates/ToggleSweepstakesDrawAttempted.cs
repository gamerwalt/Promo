using hrvster.Core.Requests.WebRequests.Sweeps.Updates;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes.Updates
{
	public class ToggleSweepstakesDrawAttempted : AsyncRequestHandler<ToggleSweepstakesDrawAttemptedRequest>
	{
		private readonly ILogger<ToggleSweepstakesDrawAttempted> _logger;
		private readonly ISweepsRepository _repository;

		public ToggleSweepstakesDrawAttempted(ILoggerFactory loggerFactory, ISweepsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<ToggleSweepstakesDrawAttempted>();
			_repository = repository;
		}

		protected override async Task Handle(ToggleSweepstakesDrawAttemptedRequest request, CancellationToken cancellationToken)
		{
			await _repository.ToggleSweepDrawAttemptedAsync(request.SweepUid, request.Value, cancellationToken);
		}
	}
}
