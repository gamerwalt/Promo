using hrvster.Extensions.Configuration;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Scheduling;
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
			var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

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
						.WriteTo.Console()
						.Enrich.FromLogContext()
						.MinimumLevel.Debug(), preserveStaticLogger: true)
				.ConfigureServices((hostContext, services) =>
				{
					Log.Information("Starting Host for hrvster.Services");

					services.AddHostedService<HrvsterHostedServices>();
					services.AddSingleton<IService, ScheduledTweetService>();
					services.AddSingleton<IService, TestService>();

					AddDatabaseAndRepositories(services);
				});

		private static void AddDatabaseAndRepositories(IServiceCollection services)
		{
			services.AddSingleton<IDatabase>(c =>
			{
				return new MySqlDatabase("hrvster", Configuration);
			});
			services.AddTransient<IScheduledSweepRuleTweetsRepository, ScheduledSweepRuleTweetsRepository>();
			AddServiceRepositoryAndServices(services);
		}

		private static void AddServiceRepositoryAndServices(IServiceCollection services)
		{
			services.AddTransient<IServicesRepository, ServicesRepository>();

			var sp = services.BuildServiceProvider();
			var repository = sp.GetService<IServicesRepository>();
			var serviceContainer = repository.GetServicesEnabledAsync();
			services.AddServiceContainer(serviceContainer);
		}
	}
}
