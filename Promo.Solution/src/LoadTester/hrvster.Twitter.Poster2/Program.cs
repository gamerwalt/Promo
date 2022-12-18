using hrvster.Extensions.Configuration;
using hrvster.Models.Options;
using MediatR;
using Microsoft.Extensions.Options;
using Serilog;

namespace hrvster.Twitter.Poster2
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
						.WriteTo.Console()
						.WriteTo.Loggly()
						.MinimumLevel.Debug(), preserveStaticLogger: true)
				.ConfigureServices((hostContext, services) =>
				{
					Log.Information("Starting Host for hrvster.Poster2");

					services.AddHostedService<Poster2>()
						.AddTwitterClient(Configuration)
						.AddPublishers()
						.AddDatabaseConfiguration(Configuration)
						.AddAccountRepositories()
						.AddRandomizer()
						.Configure<PosterOptions>(Configuration)
						.Configure<PosterOptions>(Configuration.GetSection(PosterOptions.SectionName))
						.AddTransient(s => s.GetService<IOptions<PosterOptions>>().Value);

					AddMediator(services);
				});

		private static void AddMediator(IServiceCollection services)
		{
			services.AddMediatR(typeof(Program));
		}
	}
}