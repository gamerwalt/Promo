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

namespace hrvster.Services
{
	public class Program
	{
		private static IConfiguration Configuration;

		public static void Main(string[] args)
		{
			//ASPNETCORE_ENVIRONMENT=Development
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
						//.WriteTo.Loggly()
						//.WriteTo.Console()
						.Enrich.FromLogContext()
						.MinimumLevel.Debug(), preserveStaticLogger: true)
				.ConfigureServices((hostContext, services) =>
				{
					Log.Information("Starting Host for hrvster.Services");

					services.AddHostedService<HostedServices>();
					services.AddSingleton<IService, ScheduledTweetService>();
					services.AddSingleton<IService, SweepstakesDrawService>();

					services
						.AddAccountRepositories()
						.AddScheduledRepositories()
						.AddServicesRepositories()
						.AddSweepsRepositories()
						.AddHarvesterAndReportingRepositories()
						.AddBlacklistedRepository()
						.AddTwitterClient(Configuration)
						.AddDrawSettingConfiguration(Configuration)
						.AddApplicationCronServices()
						.AddPublishers();

					AddDatabases(services);
					LoadServices(services);
					AddMediator(services);
				});

		private static void AddMediator(IServiceCollection services)
			=> services.AddMediatR(typeof(Program));
		

		private static void AddDatabases(IServiceCollection services)
		{
			services.AddSingleton<IDatabase>(c =>
			{
				return new MySqlDatabase(DatabaseNames.Main, Configuration, DatabaseNames.Harvester, DatabaseNames.Reporting);
			});
		}

		private static void LoadServices(IServiceCollection services)
		{
			var sp = services.BuildServiceProvider();
			var repository = sp.GetService<IServicesRepository>();

			var serviceContainer = repository.GetServicesEnabledAsync();

			services.AddServiceContainer(serviceContainer);
		}
	}
}
