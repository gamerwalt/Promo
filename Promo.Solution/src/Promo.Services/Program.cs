using MassTransit;
using MassTransit.Courier.Contracts;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Promo.Core.MessageComponents.BatchConsumers;
using Promo.Core.MessageComponents.Consumers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promo.RabbitMq.Consumer.Services
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env}.json")
				.Build();
			
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();

			CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
					Log.Information("Starting Host for Promo.Services");

					services.AddMassTransit(cfg =>
					{
						cfg.AddConsumersFromNamespaceContaining<RulesAddedConsumer>();
						//cfg.AddActivitiesFromNamespaceContaining<>
						cfg.UsingRabbitMq(ConigureBus);
					});

					services.AddHostedService<PromoHostedServices>();
                });

		private static void ConigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator)
		{
			configurator.ReceiveEndpoint(KebabCaseEndpointNameFormatter.Instance.Consumer<RoutingSlipBatchEventConsumer>(), e =>
			{
				e.PrefetchCount = 16;
				e.Batch<RoutingSlipCompleted>(b =>
				{
					b.TimeLimit = TimeSpan.FromSeconds(5);
					b.MessageLimit = 10;
					b.Consumer<RoutingSlipBatchEventConsumer, RoutingSlipCompleted>(context);
				});
			});

			configurator.ConfigureEndpoints(context);
		}
	}
}
