using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using hrvster.Models.Profiles;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Accounts
{
	public interface IAccountsService
	{
		Task<Account> GetAccountByUidAsync(Guid accountUid);
		Task<Profile> GetProfileAsync(Guid userUid, ProfileType profileType);
	}

	public class AccountsService : IAccountsService
	{
		private readonly ILogger<AccountsService> _logger;
		private readonly IAccountsRepository _repository;
		private readonly IProfilesRepository _profilesRepository;

		public AccountsService(
			ILoggerFactory loggerFactory, 
			IAccountsRepository repository,
			IProfilesRepository profilesRepository)
		{
			_logger = loggerFactory.CreateLogger<AccountsService>();
			_repository = repository;
			_profilesRepository = profilesRepository;
		}

		public async Task<Account> GetAccountByUidAsync(Guid accountUid)
			=> await _repository.GetAsync(accountUid);

		public async Task<Profile> GetProfileAsync(Guid userUid, ProfileType profileType)
			=> await _profilesRepository.GetProfileAsync(userUid, profileType);
	}
}
