using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.MessageContracts;
using Promo.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Core.MessageComponents.Consumers
{
	public class SweepCreatedConsumer : IConsumer<ISweepCreated>
	{
		private readonly ILogger<SweepCreatedConsumer> _logger;
		private readonly IMediator _mediator;

		public SweepCreatedConsumer() { }

		public SweepCreatedConsumer(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SweepCreatedConsumer>();
			_mediator = mediator;
		}

		public async Task Consume(ConsumeContext<ISweepCreated> context)
		{
			_logger.LogInformation("Received message for SweepCreated : {sweepName}.", context.Message.SweepName);

			await context.RespondAsync<ISweepCreatedAccepted>(new
			{
				SweepUid = context.Message.SweepUid
			});

			await _mediator.Publish(new SweepCreated(context.Message));
		}
	}

	public class SweepCreatedConsumerDefinition : ConsumerDefinition<SweepCreatedConsumer>
	{
		public SweepCreatedConsumerDefinition()
		{
			ConcurrentMessageLimit = 8;
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SweepCreatedConsumer> consumerConfigurator)
		{
			endpointConfigurator.UseMessageRetry(r => r.Interval(3, 1000));
		}
	}
}
