using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Profiles
{
	public class GetAccountAndProfile : IRequestHandler<GetAccountAndProfileRequest, Models.Account>
	{
		private readonly ILogger<GetAccountAndProfile> _logger;
		private readonly IAccountsRepository _repository;
		private readonly IProfilesRepository _profilesRepository;

		public GetAccountAndProfile(
			ILoggerFactory loggerFactory,
			IAccountsRepository repository,
			IProfilesRepository profilesRepository)
		{
			_logger = loggerFactory.CreateLogger<GetAccountAndProfile>();
			_repository = repository;
			_profilesRepository = profilesRepository;
		}

		public async Task<Models.Account> Handle(GetAccountAndProfileRequest request, CancellationToken cancellationToken)
		{
			var account = await _repository.GetAsync(request.AccountUid);
			var profiles = await _profilesRepository.GetProfilesAsync(request.AccountUid);

			if (account != null)
				account.Profiles = profiles;

			return account;
		}
	}
}
