using MassTransit;
using Microsoft.Extensions.Logging;
using Promo.Core.MessageContracts;
using System.Linq;
using System.Threading.Tasks;

namespace Promo.Core.MessageComponents.Consumers
{
	public class RulesAddedConsumer : IConsumer<IRulesAdded>
	{
		private readonly ILogger<RulesAddedConsumer> _logger;

		public RulesAddedConsumer() { }

		public RulesAddedConsumer(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<RulesAddedConsumer>();
		}

		public async Task Consume(ConsumeContext<IRulesAdded> context)
		{
			//save the message sent
			//var rulesAdded = context.Message;
			//context.MessageId
			_logger.LogInformation($"Consuming message. MessageId {context.MessageId} for {context.Message.Rules.Count} and {context.Message.Rules.FirstOrDefault().CampaignUid}");
			_logger.LogInformation($"Consuming message: WatchValue: {context.Message.Rules.FirstOrDefault().WatchValue}");
			//respond
			//if we're good
			await context.RespondAsync<IRulesAddedAccepted>(new
			{
				Rules = context.Message.Rules,
				Timestamp = InVar.Timestamp
			});

			//if rejected or invalid
			//await context.RespondAsync<IRulesAddedRejected>(new
			//{
			//	Rules = context.Message.Rules,
			//	Timestamp = InVar.Timestamp
			//});
		}
	}
}
