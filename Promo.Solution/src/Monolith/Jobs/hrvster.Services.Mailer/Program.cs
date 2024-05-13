using hrvster.ApplicationServices.JobContracts;
using hrvster.ApplicationServices.Mails;
using hrvster.Extensions.Configuration;
using hrvster.Extensions.OptionsConfiguration;
using MediatR;
using Serilog;

namespace hrvster.Services.Mailer
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
				.WriteTo.Console()
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
						.WriteTo.Console()
						.MinimumLevel.Debug(), preserveStaticLogger: true)
				.ConfigureServices((hostContext, services) =>
				{
					Log.Information("Starting Host for hrvster.Services.Mailer");

					services.AddHostedService<MailerService>();

					services.AddSingleton<IService, EmailNotificationService>();

					services
						.AddDatabaseConfiguration(Configuration)
						.AddAccountRepositories()
						.AddSweepstakesRepositories()
						.AddHarvesterAndReportingRepositories()
						.AddEmailRepository()
						.AddSingletons()
						.AddTransient<IMailService, MailService>()
						.AddServicesOptions(Configuration)
						.AddSessionManager()
						.AddHttpContextAccessor()
						//.AddAutoMapper(typeof(EntityMap))
						.AddSendGrid(Configuration);


					AddMediator(services);
				});

		private static void AddMediator(IServiceCollection services)
			=> services.AddMediatR(typeof(Program));
	}
}