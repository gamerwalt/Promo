using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Promo.Core.MessageComponents.BatchConsumers
{
	public class RoutingSlipBatchEventConsumer : IConsumer<Batch<RoutingSlipCompleted>>
	{
		public ILogger<RoutingSlipBatchEventConsumer> _logger { get; }

		public RoutingSlipBatchEventConsumer(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<RoutingSlipBatchEventConsumer>();
		}

		public Task Consume(ConsumeContext<Batch<RoutingSlipCompleted>> context)
		{
			var trackingNumbers = context.Message.Select(x => x.Message.TrackingNumber);

			_logger.LogInformation("Routing slip completed: TrackingNumber : {trackingNumber}", string.Join(", ", trackingNumbers));

			return Task.CompletedTask;
		}
	}
}
