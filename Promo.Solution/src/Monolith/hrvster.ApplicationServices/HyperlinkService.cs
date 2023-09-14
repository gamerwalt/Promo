using hrvster.ApplicationServices.Accounts;
using hrvster.Models;
using hrvster.Models.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices
{
	public interface IHyperlinkService
	{
		Task<string> GetCompanyHyperlinkAsync(Guid accountUid);
	}

	public class HyperlinkService : IHyperlinkService
	{
		private readonly ILogger<HyperlinkService> _logger;
		private readonly HarvesterSettingsOptions _harvesterSettingsOptions;
		private readonly IAccountCompanyService _accountCompanyService;

		public HyperlinkService(
			ILoggerFactory loggerFactory, 
			HarvesterSettingsOptions harvesterSettingsOptions,
			IAccountCompanyService accountCompanyService)
		{
			_logger = loggerFactory.CreateLogger<HyperlinkService>();
			_harvesterSettingsOptions = harvesterSettingsOptions;
			_accountCompanyService = accountCompanyService;
		}

		public async Task<string> GetCompanyHyperlinkAsync(Guid accountUid)
		{
			var companyInformation = await _accountCompanyService.GetAccountCompaniesAsync(accountUid);

			if (companyInformation == null || companyInformation.Count() == 0)
			{
				_logger.LogError("Unable to load company information for the account {accountUid}", accountUid);

				throw new Exception($"Unable to load company information for the account {accountUid}");
			}

			var companyList = companyInformation.ToList();

			return _harvesterSettingsOptions.HrvsterUrlForSubdomains;
		}
	}
}
