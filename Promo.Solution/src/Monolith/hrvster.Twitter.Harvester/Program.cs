using hrvster.Core.Handlers.ServiceHandlers;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Extensions.Configuration;
using hrvster.Extensions.OptionsConfiguration;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace hrvster.Twitter.Harvester
{
	public class Program
	{
		private static IConfiguration Configuration;

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
				.UseSerilog((context, configuration) =>
					configuration.ReadFrom.Configuration(context.Configuration)
						.Enrich.FromLogContext()
						.MinimumLevel.Debug(), preserveStaticLogger: true)
				.ConfigureServices((hostContext, services) =>
				{
					Log.Information("Starting Host for hrvster");

					services.AddHostedService<HostedService>();
					services.AddSingleton<IService, TwitterHarvestingService>();

					services
						.AddAccountRepositories()
						.AddSweepstakesRepositories()
						.AddHarvesterAndReportingRepositories()
						.AddTwitterClient(Configuration)
						.AddApplicationCronServices()
						.AddPublishers()
						.AddGameService()
						.AddMatchEngineServices()
						.AddMatchGameRepositories()
						.AddSweepstakesServices()
						.AddSubscriptionRepositories()
						.AddSubscriptionService()
						.AddAccountsService()
						.AddHarvsterConfigurationOption(Configuration);

					AddDatabases(services);
					AddMediator(services);
				});

		private static void AddMediator(IServiceCollection services) 
			=> services.AddMediatR(typeof(Program))
				.AddMediatR(typeof(GetActiveHashtagRulesRequest), typeof(GetActiveHashtagRules));

		private static void AddDatabases(IServiceCollection services)
		{
			services.AddSingleton<IDatabase>(c =>
			{
				return new MySqlDatabase(DatabaseNames.Main, Configuration, DatabaseNames.Harvester, DatabaseNames.Reporting);
			});

			var sp = services.BuildServiceProvider();
			var database = sp.GetService<IDatabase>();
		}
	}
}
