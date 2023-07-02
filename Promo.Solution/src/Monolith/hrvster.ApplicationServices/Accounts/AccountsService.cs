using hrvster.Core.Clients;
using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using hrvster.Models.Profiles;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Accounts
{
	public interface IAccountsService
	{
		Task<Account> GetAccountByUidAsync(Guid accountUid);
		Task<IEnumerable<Rule>> GetAllRulesAsync(Guid accountUid);
		Task<Profile> GetProfileAsync(Guid userUid, ProfileType profileType);
		Task<Profile> GetProfileBySweepUidAsync(Guid campaignUid, ProfileType profileType);
		Task RaiseProfileValidatedEventAsync(UserProviderDetails userDetails, Guid userUid, ProfileType profileType);
	}

	public class AccountsService : IAccountsService
	{
		private readonly ILogger<AccountsService> _logger;
		private readonly IAccountsRepository _repository;
		private readonly IProfilesRepository _profilesRepository;
		private readonly IMediator _mediator;

		public AccountsService(
			ILoggerFactory loggerFactory, 
			IAccountsRepository repository,
			IProfilesRepository profilesRepository,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<AccountsService>();
			_repository = repository;
			_profilesRepository = profilesRepository;
			_mediator = mediator;
		}

		public async Task<Account> GetAccountByUidAsync(Guid accountUid)
			=> await _repository.GetAsync(accountUid);

		public async Task<IEnumerable<Rule>> GetAllRulesAsync(Guid accountUid)
			=> await _repository.GetRulesAsync(accountUid);

		public async Task<Profile> GetProfileAsync(Guid userUid, ProfileType profileType)
			=> await _profilesRepository.GetProfileAsync(userUid, profileType);

		public async Task<Profile> GetProfileBySweepUidAsync(Guid campaignUid, ProfileType profileType)
		{
			return await _profilesRepository.GetProfileByCampaignUid(campaignUid, profileType);
		}

		public async Task RaiseProfileValidatedEventAsync(UserProviderDetails userDetails, Guid userUid, ProfileType profileType)
			=> await _mediator.Publish(new ProfileValidated(userDetails, userUid, profileType));
	}
}
