using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Account
{
	public class UpdateLastLogin : AsyncRequestHandler<UpdateLastLoginRequest>
	{
		private readonly ILogger<UpdateLastLogin> _logger;
		private readonly IAccountsRepository _repository;

		public UpdateLastLogin(ILoggerFactory loggerFactory, IAccountsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpdateLastLogin>();
			_repository = repository;
		}

		protected override async Task Handle(UpdateLastLoginRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Update last login for account with uid: {accountUid}", request.AccountUid);

			await _repository.UpdateLastLoginAsync(request.AccountUid);
		}
	}
}
