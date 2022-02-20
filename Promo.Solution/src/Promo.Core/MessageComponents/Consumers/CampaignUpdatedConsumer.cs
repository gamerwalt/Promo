using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.MessageContracts;
using Promo.Core.Notifications;
using System.Threading.Tasks;

namespace Promo.Core.MessageComponents.Consumers
{
	public class CampaignUpdatedConsumer : IConsumer<ICampaignUpdated>
	{
		private readonly ILogger<CampaignUpdatedConsumer> _logger;
		private readonly IMediator _mediator;

		public CampaignUpdatedConsumer(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<CampaignUpdatedConsumer>();
			_mediator = mediator;
		}

		public async Task Consume(ConsumeContext<ICampaignUpdated> context)
		{
			await context.RespondAsync<ICampaignUpdatedAccepted>(new
			{
				CampaignUid = context.Message.CampaignUid
			});

			await _mediator.Publish(new CampaignUpdated(context.Message));
		}
	}

	public class CampaignUpdatedConsumerDefinition : ConsumerDefinition<CampaignUpdatedConsumer>
	{
		public CampaignUpdatedConsumerDefinition()
		{
			ConcurrentMessageLimit = 8;
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CampaignUpdatedConsumer> consumerConfigurator)
		{
			endpointConfigurator.UseMessageRetry(r => r.Interval(3, 1000));
		}
	}
}
