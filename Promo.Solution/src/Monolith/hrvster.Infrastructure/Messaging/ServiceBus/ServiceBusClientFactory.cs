using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;

namespace hrvster.Infrastructure.Messaging.ServiceBus
{
	public class ServiceBusClientFactory
	{
		private readonly string _connectionString;
		private ServiceBusClient _client;

		public ServiceBusClientFactory(IConfiguration configuration)
		{
			_connectionString = configuration["ServiceBus:ConnectionString"];
			_client = new ServiceBusClient(_connectionString);
		}

		public ServiceBusClient GetClient()
		{
			return _client;
		}

		public void Dispose()
		{
			if (_client != null)
			{
				_client.DisposeAsync().AsTask().Wait();
				_client = null;
			}
		}
	}

}
