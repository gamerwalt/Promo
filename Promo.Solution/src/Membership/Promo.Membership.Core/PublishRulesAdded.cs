using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.MessageContracts;
using Promo.Membership.Core.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core
{
	public class PublishRulesAdded : INotificationHandler<RulesAdded>
	{
		private readonly ILogger<PublishRulesAdded> _logger;
		private readonly IRequestClient<IRulesAdded> _submitRulesAddedClient;

		public PublishRulesAdded(ILoggerFactory loggerFactory, IRequestClient<IRulesAdded> submitRulesAddedClient)
		{
			_logger = loggerFactory.CreateLogger<PublishRulesAdded>();
			_submitRulesAddedClient = submitRulesAddedClient;
		}

		public async Task Handle(RulesAdded notification, CancellationToken cancellationToken)
		{
			var (accepted, rejected) = await _submitRulesAddedClient.GetResponse<IRulesAddedAccepted, IRulesAddedRejected>(new
			{
				Rules = notification.GetRules()
			});

			if (accepted.IsCompletedSuccessfully)
				_logger.LogInformation("Rules added sent successfully.");
			else
				_logger.LogError($"Rules were not properly sent to the queue. Reason: {rejected.Exception.Message}");
		}
	}
}
