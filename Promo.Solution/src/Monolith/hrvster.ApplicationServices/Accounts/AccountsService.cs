using hrvster.Infrastructure;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Accounts
{
	public interface IAccountsService
	{
		Task<Account> GetAccountByUidAsync(Guid accountUid);
	}

	public class AccountsService : IAccountsService
	{
		private readonly ILogger<AccountsService> _logger;
		private readonly IAccountsRepository _repository;

		public AccountsService(ILoggerFactory loggerFactory, IAccountsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<AccountsService>();
			_repository = repository;
		}

		public async Task<Account> GetAccountByUidAsync(Guid accountUid)
			=> await _repository.GetAsync(accountUid);
	}
}
