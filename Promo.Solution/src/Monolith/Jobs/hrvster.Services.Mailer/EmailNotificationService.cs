using hrvster.ApplicationServices.JobContracts;
using hrvster.ApplicationServices.Mails;

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
			_logger.LogInformation($"Processing the service {nameof(EmailNotificationService)}.");

			await _mailService.ProcessAnyUnsentEmails(token);

			_logger.LogInformation($"Finished processing the {nameof(EmailNotificationService)}.");
		}
	}
}
