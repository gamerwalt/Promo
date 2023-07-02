using hrvster.Infrastructure.Accounts;
using hrvster.Models.Company;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Accounts
{
	public interface IAccountCompanyService
	{
		Task<IEnumerable<AccountCompany>> GetAccountCompaniesAsync(Guid accountUid);
		Task<AccountCompany> GetAccountCompanyAsync(string subdomain);
	}

	public class AccountCompanyService : IAccountCompanyService
	{
		private readonly ILogger<AccountCompanyService> _logger;
		private readonly IAccountCompanyRepository _repository;

		public AccountCompanyService(ILoggerFactory loggerFactory, IAccountCompanyRepository repository)
		{
			_logger = loggerFactory.CreateLogger<AccountCompanyService>();
			_repository = repository;
		}

		public async Task<IEnumerable<AccountCompany>> GetAccountCompaniesAsync(Guid accountUid)
			=> await _repository.GetAccountCompaniesAsync(accountUid);

		public async Task<AccountCompany> GetAccountCompanyAsync(string subdomain)
			=> await _repository.GetAccountCompanyAsync(subdomain);
	}
}
