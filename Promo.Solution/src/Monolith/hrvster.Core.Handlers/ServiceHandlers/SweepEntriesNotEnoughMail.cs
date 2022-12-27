using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class SweepEntriesNotEnoughMail : AsyncRequestHandler<SweepEntriesNotEnoughMailRequest>
	{
		private readonly ILogger<SweepEntriesNotEnoughMail> _logger;
		private readonly IMailRepository _repository;

		public SweepEntriesNotEnoughMail(ILoggerFactory loggerFactory, IMailRepository repository)
		{
			_logger = loggerFactory.CreateLogger<SweepEntriesNotEnoughMail>();
			_repository = repository;
		}

		protected override async Task Handle(SweepEntriesNotEnoughMailRequest request, CancellationToken cancellationToken)
		{
			await _repository.CreateAsync(new Models.Email
			{
				EmailUid = Guid.NewGuid(),
				EmailAddress = request.EmailAddress,
				Status = Models.EmailStatus.New,
				Template = Models.EmailTemplate.SweepEntriesNotEnoughEmail,
				Parameters = new Dictionary<string, string>
				{
					{ "EmailAddress", request.EmailAddress },
					{ "SweepstakeName", request.SweepName },
				}
			});
		}
	}
}
