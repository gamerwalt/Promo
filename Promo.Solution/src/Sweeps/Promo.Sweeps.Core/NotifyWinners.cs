using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Sweeps.Core.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Core
{
	public class NotifyWinners : INotificationHandler<WinnerSaved>
	{
		private readonly ILogger<NotifyWinners> _logger;

		public NotifyWinners(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<NotifyWinners>();
		}

		public async Task Handle(WinnerSaved notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Notifying winners of their win.");
		}
	}
}
