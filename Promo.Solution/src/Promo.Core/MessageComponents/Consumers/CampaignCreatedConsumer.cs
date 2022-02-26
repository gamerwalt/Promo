using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Promo.Core.MessageContracts;
using Promo.Core.Notifications;
using System.Threading.Tasks;

namespace Promo.Core.MessageComponents.Consumers
{
	public class CampaignCreatedConsumer : IConsumer<ICampaignCreated>
	{
		private readonly ILogger<CampaignCreatedConsumer> _logger;
		private readonly IMediator _mediator;

		public CampaignCreatedConsumer() { }

		public CampaignCreatedConsumer(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<CampaignCreatedConsumer>();
			_mediator = mediator;
		}

		public async Task Consume(ConsumeContext<ICampaignCreated> context)
		{
			var campaignRecieved = JsonConvert.SerializeObject(context.Message);
			_logger.LogInformation("Campaign Created Received: {campaignReceived}", campaignRecieved);

			await context.RespondAsync<ICampaignCreatedAccepted>(new
			{
				AccountUid = context.Message.AccountUid,
				CampaignUid = context.Message.CampaignUid
			});

			await _mediator.Publish(new CampaignCreated(context.Message));
		}
	}

	public class CampaignCreatedConsumerDefinition : ConsumerDefinition<CampaignCreatedConsumer>
	{
		public CampaignCreatedConsumerDefinition()
		{
			ConcurrentMessageLimit = 8;
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CampaignCreatedConsumer> consumerConfigurator)
		{
			endpointConfigurator.UseMessageRetry(r => r.Interval(3, 1000));
		}
	}
}
