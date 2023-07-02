using hrvster.Infrastructure.Accounts;
using hrvster.Templates.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace hrvster.Middleware
{
	public class SubdomainMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<SubdomainMiddleware> _logger;
		private readonly IAccountCompanyRepository _accountCompanyRepository;

		public SubdomainMiddleware(
			RequestDelegate next, 
			ILoggerFactory loggerFactory, 
			IAccountCompanyRepository accountCompanyRepository)
		{
			_next = next;
			_logger = loggerFactory.CreateLogger<SubdomainMiddleware>();
			_accountCompanyRepository = accountCompanyRepository;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			string? subdomain;

			if (context.Request.Host.Host.Equals("localhost"))
			{
				subdomain = "mtn";
			}
			else
			{
				var host = context.Request.Host.Host;
				var index = host.IndexOf(".");
				subdomain = (index < 0) ? null : host.Substring(0, index);
			}

			//is the subdomain valid?
			var companyInformation = await _accountCompanyRepository.GetAccountCompanyAsync(subdomain);

			if (companyInformation == null)
				throw new Exception("Company does not exist on our systems.");

			context.Items[Constants.ContextItemsSubdomain] = subdomain;

			await _next(context);
		}
	}
}