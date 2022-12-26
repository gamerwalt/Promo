using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
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
	public class MarkSweepAsDrawAttempted : AsyncRequestHandler<MarkSweepAsDrawAttemptedRequest>
	{
		private readonly ILogger<MarkSweepAsDrawAttempted> _logger;
		private readonly ISweepsRepository _repository;

		public MarkSweepAsDrawAttempted(ILoggerFactory loggerFactory, ISweepsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<MarkSweepAsDrawAttempted>();
			_repository = repository;
		}

		protected override async Task Handle(MarkSweepAsDrawAttemptedRequest request, CancellationToken cancellationToken)
		{
			await _repository.MarkSweepAsDrawAttemptedAsync(request.SweepUid, cancellationToken);
		}
	}
}
