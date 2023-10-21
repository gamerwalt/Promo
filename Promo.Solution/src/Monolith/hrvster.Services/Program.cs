using hrvster.ApplicationServices.Harvesters.Facebook;
using hrvster.Core.Handlers.ServiceHandlers;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Extensions.Configuration;
using hrvster.Extensions.OptionsConfiguration;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Facebook;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace hrvster.Services
{
	public class Program
	{
		private static IConfiguration Configuration;

		public static void Main(string[] args)
		{
			//ASPNETCORE_ENVIRONMENT=Development
			var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			if (string.IsNullOrEmpty(env))
			{
				env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
			}

			Configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env ?? "Development"}.json", optional: true, reloadOnChange: true)
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(Configuration)
				.Enrich.FromLogContext()
				.WriteTo.Loggly()
				.MinimumLevel.Debug()
				.CreateLogger();

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog((context, configuration) =>
					configuration.ReadFrom.Configuration(context.Configuration)
						.Enrich.FromLogContext()
						.WriteTo.Loggly()
						.MinimumLevel.Debug(), preserveStaticLogger: true)
				.ConfigureServices((hostContext, services) =>
				{
					Log.Information("Starting Host for hrvster.Services");

					services.AddHostedService<HostedServices>();
					services.AddSingleton<IService, ScheduledPostService>();
					services.AddSingleton<IService, SweepstakesDrawService>();
					services.AddSingleton<IService, EmailNotificationService>();
					services.AddSingleton<IService, AccountVerificationService>();
					services.AddSingleton<IService, FlutterwaveEventService>();
					services.AddSingleton<IService, HarvesterRestarterService>();
					services.AddSingleton<IService, SweepsGoingLiveService>();
					services.AddSingleton<IService, MatchGameBoardService>();

					services
						.AddDatabaseConfiguration(Configuration)
						.AddAccountRepositories()
						.AddServicesRepositories()
						.AddSweepstakesRepositories()
						.AddHarvesterAndReportingRepositories()
						.AddEmailRepository()
						.AddTwitterClient(Configuration)
						.AddDrawSettingConfiguration(Configuration)
						.AddApplicationCronServices()
						.AddPublishers()
						.AddSendGrid(Configuration)
						.AddRandomizer()
						.AddFirebaseServices(Configuration)
						.AddSingletons()
						.AddHarvsterConfigurationOption(Configuration)
						.AddSubscriptionService()
						.AddSubscriptionRepositories()
						.AddFlutterwaveEventService()
						.AddHarvesterServiceCommandRepository()
						.AddHarvesterService()
						.AddSweepstakesServices()
						.AddAccountsService()
						.AddPaymentServices(Configuration)
						.AddAutoMapper(typeof(EntityMap))
						.AddMatchGameRepositories()
						.AddCacheService()
						.AddMatchGameService()
						.AddPostSchedulers()
						.AddTwitterHttpService(Configuration);

					services.AddTransient<IFacebookPageSerivce, FacebookPageSerivce>();
					services.AddTransient<IFacebookPageRepository, FacebookPageRepository>();

					LoadServices(services);
					AddMediator(services);
				});

		private static void AddMediator(IServiceCollection services)
			=> services.AddMediatR(typeof(Program))
				.AddMediatR(typeof(GetSweepstakesRequiringADrawRequest), typeof(GetSweepstakesRequiringADraw));


		private static void LoadServices(IServiceCollection services)
		{
			var sp = services.BuildServiceProvider();
			var repository = sp.GetService<IServicesRepository>();

			var serviceContainer = repository.GetServicesEnabledAsync();

			services.AddServiceContainer(serviceContainer);
		}
	}
}
