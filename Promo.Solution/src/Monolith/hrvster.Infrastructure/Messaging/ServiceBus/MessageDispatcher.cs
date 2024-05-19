using Azure.Messaging.ServiceBus;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.MongoEntities.MatchGames;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Messaging.ServiceBus
{
	public class MessageDispatcher
	{
		private readonly ServiceBusClient _serviceBusClient;

		public MessageDispatcher(ServiceBusClientFactory clientFactory)
		{
			_serviceBusClient = clientFactory.GetClient();
		}

		public async Task SendTilesAsync(IEnumerable<CachedTile> tiles, string queueName, CancellationToken token)
		{
			token.ThrowIfCancellationRequested();

			var sender = _serviceBusClient.CreateSender(queueName);
			var batch = await sender.CreateMessageBatchAsync();

			foreach (var tile in tiles)
			{
				var message = new ServiceBusMessage(JsonConvert.SerializeObject(tile));
				if (!batch.TryAddMessage(message))
				{
					// If the batch is full, send it and start a new one
					await sender.SendMessagesAsync(batch);
					batch = await sender.CreateMessageBatchAsync();
					if (!batch.TryAddMessage(message))
					{
						throw new Exception("Message is too large to fit in an empty batch.");
					}
				}
			}

			if (batch.Count > 0)
			{
				await sender.SendMessagesAsync(batch);
			}
		}
	}

}
