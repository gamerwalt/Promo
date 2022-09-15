using hrvster.ApplicationServices.Accounts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class AccountVerificationService : IService
	{
		private readonly ILogger<AccountVerificationService> _logger;
		private readonly IAccountVerificationService _service;

		public AccountVerificationService(
			ILoggerFactory loggerFactory,
			IAccountVerificationService service)
		{
			_logger = loggerFactory.CreateLogger<AccountVerificationService>();
			_service = service;
		}

		public async Task ProcessService(CancellationToken token)
		{
			await _service.VerifyUnverifiedAccountsAsync(token);
		}
	}
}
