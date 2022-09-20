using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Infrastructure.Accounts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Account
{
	public class UnlinkProfile : AsyncRequestHandler<UnlinkProfileRequest>
	{
		private readonly ILogger<UnlinkProfile> _logger;
		private readonly IProfilesRepository _repository;

		public UnlinkProfile(ILoggerFactory loggerFactory, IProfilesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UnlinkProfile>();
			_repository = repository;
		}

		protected override async Task Handle(UnlinkProfileRequest request, CancellationToken cancellationToken)
		{
			await _repository.RemoveProfileAsync(request.AccountUid, request.ProfileType);
		}
	}
}
