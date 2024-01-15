using hrvster.ApplicationServices.Files;
using hrvster.ApplicationServices.Harvesters.Facebook;
using hrvster.ApplicationServices.Subscription;
using hrvster.Extensions.Configuration;
using hrvster.Extensions.OptionsConfiguration;
using hrvster.Infrastructure.Facebook;
using hrvster.Infrastructure.Subscriptions;
using MediatR;
using Serilog;

namespace hrvster.PaymentPendingChecker.Service
{
	public class Program
	{
		private static IConfiguration Configuration;

		public static void Main(string[] args)
		{
			var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			if (string.IsNullOrEmpty(env))
			{
				env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
			}

			Configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(Configuration)
				.Enrich.FromLogContext()
				.WriteTo.Console()
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
						.WriteTo.Console()
						.WriteTo.Loggly()
						.MinimumLevel.Debug(), preserveStaticLogger: true)
				.ConfigureServices((hostContext, services) =>
				{
					Log.Information("Starting Host for hrvster.Services");

					services.AddHostedService<PaymentPendingCheckerService>();

					services.AddTransient<IAccountSubscriptionCheckerService, AccountSubscriptionCheckerService>();
					services.AddTransient<IAccountSubscriptionService, AccountSubscriptionService>();
					services.AddTransient<IAccountSubscriptionsRepository, AccountSubscriptionsRepository>();

					services.AddPaymentPendingConfigurationOption(hostContext.Configuration);
					services.AddHarvsterConfigurationOption(hostContext.Configuration);

					services
						.AddDatabaseConfiguration(Configuration)
						.AddCampaignRepositories()
						.AddWatchTypeRepositories()
						.AddAccountRepositories()
						.AddLinkBuilder()
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
						.AddMatchGameRepositories()
						.AddCacheService()
						.AddMatchGameService()
						.AddPostSchedulers()
						.AddTwitterHttpService(Configuration)
						.AddServicesOptions(Configuration)
						.AddSessionManager()
						.AddLinkBuilder()
						.AddHttpContextAccessor()
						.AddAutoMapper(typeof(EntityMap))
						.AddGameService()
						.AddStripeConfiguration(Configuration)
						.AddPaymentPlanProvider(Configuration);

					AddMediator(services);

					services.AddTransient<IFacebookPageSerivce, FacebookPageSerivce>();
					services.AddTransient<IFacebookPageRepository, FacebookPageRepository>();

					services.AddTransient<IFileUploaderService, FileUploaderService>();
				});

		private static void AddMediator(IServiceCollection services)
			=> services.AddMediatR(typeof(Program));
	}
}