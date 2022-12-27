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
	public class SweepsDistinctUsersLessThanWinnerCountMail : AsyncRequestHandler<SweepsDistinctUsersLessThanWinnerCountMailRequest>
	{
		private readonly ILogger<SweepsDistinctUsersLessThanWinnerCountMail> _logger;
		private readonly IMailRepository _repository;

		public SweepsDistinctUsersLessThanWinnerCountMail(ILoggerFactory loggerFactory, IMailRepository repository)
		{
			_logger = loggerFactory.CreateLogger<SweepsDistinctUsersLessThanWinnerCountMail>();
			_repository = repository;
		}

		protected async override Task Handle(SweepsDistinctUsersLessThanWinnerCountMailRequest request, CancellationToken cancellationToken)
		{
			await _repository.CreateAsync(new Models.Email
			{
				EmailUid = Guid.NewGuid(),
				EmailAddress = request.EmailAddress,
				Status = Models.EmailStatus.New,
				Template = Models.EmailTemplate.NotEnoughSweepstakeDistinctUsers,
				Parameters = new Dictionary<string, string>
				{
					{ "EmailAddress", request.EmailAddress },
					{ "SweepstakeName", request.SweepstakeName },
				}
			});
		}
	}
}
