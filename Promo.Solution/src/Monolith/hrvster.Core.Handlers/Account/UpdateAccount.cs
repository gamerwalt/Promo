using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Account
{
	public class UpdateAccount : AsyncRequestHandler<UpdateAccountRequest>
	{
		private readonly ILogger<UpdateAccount> _logger;
		private readonly IAccountsRepository _repository;

		public UpdateAccount(ILoggerFactory loggerFactory, IAccountsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpdateAccount>();
			_repository = repository;
		}


		protected override async Task Handle(UpdateAccountRequest request, CancellationToken cancellationToken)
		{
			await _repository.UpdateAsync(request.AccountUid, request.FirstName, request.LastName, request.DisplayName);
		}
	}
}
