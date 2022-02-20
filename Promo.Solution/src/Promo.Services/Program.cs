using MassTransit;
using MassTransit.Courier.Contracts;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Promo.Core;
using Promo.Core.Handlers;
using Promo.Core.MessageComponents.BatchConsumers;
using Promo.Core.MessageComponents.Consumers;
using Promo.Core.Notifications;
using Promo.Infrastructure.Games;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Promo.RabbitMq.Consumer.Services
{
    public class Program
    {
		private static IConfigurationRoot Configuration;
        public static void Main(string[] args)
        {
			var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			Configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env}.json")
				.Build();
			
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(Configuration)
				.CreateLogger();

			CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
					Log.Information("Starting Host for Promo.Services");

					services.AddMediatR(typeof(AccountCreated), typeof(AddAccountToGamesDatabase));

					services.AddMassTransit(cfg =>
					{
						cfg.AddConsumersFromNamespaceContaining<RulesAddedConsumer>();
						//cfg.AddActivitiesFromNamespaceContaining<>
						cfg.UsingRabbitMq(ConigureBus);
					});

					services.AddHostedService<PromoHostedServices>();

					AddDatabaseAndRepositories(services);
                });

		private static void AddDatabaseAndRepositories(IServiceCollection services)
		{
			services.AddSingleton<IDatabase>(c =>
			{
				return new MySqlDatabase(ConnectionStrings.Games, Configuration);
			});

			services.AddTransient<IGamesAccountsRepository, GamesAccountsRepository>();
			services.AddTransient<IGamesCampaignRepository, GamesCampaignRepository>();
		}

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
