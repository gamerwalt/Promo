using hrvster.Extensions.Configuration;
using MediatR;
using Microsoft.Extensions.Options;
using Serilog;

namespace SweepEntrySeeder
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
						.Enrich.FromLogContext()
						.MinimumLevel.Debug(), preserveStaticLogger: true)
				.ConfigureServices((hostContext, services) =>
				{
					Log.Information("Starting Host for hrvster.Services");

					services.AddHostedService<HostedService>()
						.AddDatabaseConfiguration(Configuration)
						.AddSweepstakesRepositories()
						.AddRandomizer()
						.Configure<SweepstakeSeederOptions>(Configuration)
						.Configure<SweepstakeSeederOptions>(Configuration.GetSection(SweepstakeSeederOptions.SectionName))
						.AddTransient(s => s.GetService<IOptions<SweepstakeSeederOptions>>().Value);

					AddMediator(services);
				});

		private static void AddMediator(IServiceCollection services)
		{
			services.AddMediatR(typeof(Program));
		}
	}
}