using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.Notifications.Handlers
{
	public class StorePasswordResetLink : INotificationHandler<PasswordResetLinkReceived>
	{
		private readonly ILogger<StorePasswordResetLink> _logger;
		private readonly IMailRepository _mailRepository;

		public StorePasswordResetLink(ILoggerFactory loggerFactory, IMailRepository mailRepository)
		{
			_logger = loggerFactory.CreateLogger<StorePasswordResetLink>();
			_mailRepository = mailRepository;
		}

		public async Task Handle(PasswordResetLinkReceived notification, CancellationToken cancellationToken)
		{
			await _mailRepository.CreateAsync(new Models.Email
			{
				EmailAddress = notification.EmailAddress,
				Parameters = new Dictionary<string, string>()
				{
					{ "PasswordResetLink", notification.ResetPasswordLink }
				},
				Template = EmailTemplate.PasswordResetEmail,
				Status = EmailStatus.New
			});
		}
	}
}
