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
	public class SweepUpdatedConsumer : IConsumer<ISweepUpdated>
	{
		private readonly ILogger<SweepUpdatedConsumer> _logger;
		private readonly IMediator _mediator;

		public SweepUpdatedConsumer() { }

		public SweepUpdatedConsumer(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SweepUpdatedConsumer>();
			_mediator = mediator;
		}

		public async Task Consume(ConsumeContext<ISweepUpdated> context)
		{
			_logger.LogInformation("Received message for SweepUpdated: {sweepName}", context.Message.SweepName);

			await context.RespondAsync<ISweepUpdatedAccepted>(new
			{
				SweepUid = context.Message.SweepUid
			});

			await _mediator.Publish(new SweepUpdated(context.Message));
		}
	}

	public class SweepUpdatedConsumerDefinition : ConsumerDefinition<SweepUpdatedConsumer>
	{
		public SweepUpdatedConsumerDefinition()
		{
			ConcurrentMessageLimit = 8;
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SweepUpdatedConsumer> consumerConfigurator)
		{
			endpointConfigurator.UseMessageRetry(r => r.Interval(3, 1000));
		}
	}
}
