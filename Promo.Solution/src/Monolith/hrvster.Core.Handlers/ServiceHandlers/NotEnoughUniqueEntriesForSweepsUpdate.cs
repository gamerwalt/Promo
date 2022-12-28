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
	public class NotEnoughUniqueEntriesForSweepsUpdate : AsyncRequestHandler<NotEnoughUniqueEntriesForSweepsUpdateRequest>
	{
		private readonly ILogger<NotEnoughUniqueEntriesForSweepsUpdate> _logger;
		private readonly ISweepsRepository _repository;

		public NotEnoughUniqueEntriesForSweepsUpdate(ILoggerFactory loggerFactory, ISweepsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<NotEnoughUniqueEntriesForSweepsUpdate>();
			_repository = repository;
		}

		protected async override Task Handle(NotEnoughUniqueEntriesForSweepsUpdateRequest request, CancellationToken cancellationToken)
		{
			await _repository.UpdateSweepAsNotEnoughUniqueEntrantsAsync(request.SweepUid, cancellationToken);
		}
	}
}
