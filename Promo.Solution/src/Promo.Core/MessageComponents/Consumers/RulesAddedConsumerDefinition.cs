using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace Promo.Core.MessageComponents.Consumers
{
	public class RulesAddedConsumerDefinition : ConsumerDefinition<RulesAddedConsumer>
	{
		public RulesAddedConsumerDefinition()
		{
			ConcurrentMessageLimit = 8;
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<RulesAddedConsumer> consumerConfigurator)
		{
			endpointConfigurator.UseMessageRetry(r => r.Interval(3, 1000));
		}
	}
}
