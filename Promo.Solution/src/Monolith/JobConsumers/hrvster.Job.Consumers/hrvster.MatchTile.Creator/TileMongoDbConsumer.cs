using Azure.Messaging.ServiceBus;

namespace hrvster.MatchTile.Creator
{
	public class TileMongoDbConsumer
	{
		private readonly ILogger<TileMongoDbConsumer> _logger;
		private readonly IConfiguration _config;
		private readonly ServiceBusClient _client;
		private readonly ServiceBusProcessor _processor;
		private readonly Guid _instanceId;

		public TileMongoDbConsumer(ILoggerFactory loggerFactory, IConfiguration config)
		{
			_instanceId = Guid.NewGuid();
			_config = config;
			_logger = loggerFactory.CreateLogger<TileMongoDbConsumer>();
			_client = new ServiceBusClient(_config["ServiceBus:ConnectionString"]);
			_processor = _client.CreateProcessor(
				_config["ServiceBus:CreatedTileQueueName"],
				new ServiceBusProcessorOptions
				{
					MaxConcurrentCalls = 2,  // Adjust according to your scenario
					PrefetchCount = 20       // Adjust according to your scenario
				});
		}

		public async Task RunAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Tile MongbDb Consumer with instance id {InstanceId} is starting...", _instanceId);

			_processor.ProcessMessageAsync += TileCreatedReceivedHandler;
			_processor.ProcessErrorAsync += TileCreatedReceivedErrorHandler;

			await _processor.StartProcessingAsync(cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Tile MongDb Consumer with instance id {InstanceId} is stopping...", _instanceId);

			await _processor.StopProcessingAsync();
			await _processor.DisposeAsync();
			await _client.DisposeAsync();
		}

		private Task TileCreatedReceivedErrorHandler(ProcessErrorEventArgs args)
		{
			//Save error somewhere

			_logger.LogError(args.Exception.Message);

			return Task.CompletedTask;
		}

		private async Task TileCreatedReceivedHandler(ProcessMessageEventArgs args)
		{
			var message = args.Message.Body.ToString();

			_logger.LogInformation("Received the message {Message}", message);

			await args.CompleteMessageAsync(args.Message);
		}
	}
}
