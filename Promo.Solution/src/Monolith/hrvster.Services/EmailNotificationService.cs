using hrvster.ApplicationServices.Mails;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services.Mailer
{
	public class EmailNotificationService : IService
	{
		private readonly ILogger<EmailNotificationService> _logger;
		private readonly IMailService _mailService;

		public EmailNotificationService(ILoggerFactory loggerFactory, IMailService mailService)
		{
			_logger = loggerFactory.CreateLogger<EmailNotificationService>();
			_mailService = mailService;
		}

		public async Task ProcessService(CancellationToken token)
		{
			await _mailService.ProcessAnyUnsentEmails(token);
		}
	}
}
