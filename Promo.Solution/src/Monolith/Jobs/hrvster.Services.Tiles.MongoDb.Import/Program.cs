using hrvster.ApplicationServices.Caching;
using hrvster.ApplicationServices.JobContracts;
using hrvster.Core.Clients.Publishers;
using hrvster.Extensions.Configuration;
using hrvster.Extensions.OptionsConfiguration;
using hrvster.Infrastructure.Facebook;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.MongoRepositories;
using hrvster.Models;
using MediatR;
using Serilog;

namespace hrvster.Services.Tiles.MongoDb.Import
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

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog((context, configuration) =>
					configuration.ReadFrom.Configuration(context.Configuration)
						.Enrich.FromLogContext()
						.WriteTo.Loggly()
						.WriteTo.Console()
						.WriteTo.File("C:/hrvster/logs/hrvsterMongoDBImporter/logs.txt", Serilog.Events.LogEventLevel.Verbose)
						.MinimumLevel.Verbose(), preserveStaticLogger: true)
				.ConfigureServices((hostContext, services) =>
				{
					Log.Information("Starting Host for hrvster.Services.BoardTilesSeeder");

					services.AddHostedService<TileImporter>();

					services.AddSingleton<IService>(sp => CreateMongoDbTileImporter(Configuration, sp));

					services
						.AddDatabaseConfiguration(Configuration)
						.AddAccountRepositories()
						.AddSweepstakesRepositories()
						.AddHarvesterAndReportingRepositories()
						.AddEmailRepository()
						.AddSingletons()
						.AddServicesOptions(Configuration)
						.AddSessionManager()
						.AddHttpContextAccessor()
						.AddMatchGameService()
						.AddTransient<IMatchGamesRepository, MatchGamesRepository>()
						.AddTransient<IMatchPrizesRepository, MatchPrizesRepository>()
						.AddTransient<IMatchImagesRepository, MatchImagesRepository>()
						.AddTransient<IMatchBoardsRepository, MatchBoardsRepository>()
						.AddTransient<IMatchGameRulesRepository, MatchGameRulesRepository>()
						.AddTransient<IMatchRulePageRepository, MatchRulePageRepository>()
						.AddTransient<IFacebookPageRepository, FacebookPageRepository>()
						.AddTransient<IMatchGamePlayRepository, MatchGamePlayRepository>()
						.AddTransient<IMatchTilesRepository, MatchTilesRepository>()
						.AddTransient<IMatchPlayRulesRepository, MatchPlayRulesRepository>()
						.AddTransient<IMongoMatchTileRepository>(sp => GenerateMongoMatchTileRepository(Configuration))
						.AddTransient<ICacheDataService, RedisCacheService>()
						.AddHarvsterConfigurationOption(Configuration)
						.AddTransient<
							ApplicationServices.MatchGames.V2.IMatchGameBoardService, ApplicationServices.MatchGames.V2.MatchGameBoardService>()
						.AddTransient<Func<ExternalClientType, IExternalClient>>(sp => serviceTypeName =>
						{
							switch (serviceTypeName)
							{
								case ExternalClientType.Twitter:
									return sp.GetService<TwitterClient>();
								case ExternalClientType.Facebook:
									return sp.GetService<FacebookClient>();
								case ExternalClientType.Instagram:
									return sp.GetService<InstagramClient>();
								default:
									return sp.GetService<TwitterClient>();
							}
						})
						.AddSendGrid(Configuration);


					AddMediator(services);
				});

		private static IService CreateMongoDbTileImporter(IConfiguration configuration, IServiceProvider sp)
		{
			var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
			var connectionString = configuration["MongoDbSettings:ConnectionString"];
			var databaseName = configuration["MongoDbSettings:DatabaseName"];
			var matchTileCollectionName = configuration["MongoDbSettings:TileCollection"];
			var directory = configuration["TileFilesLocation"];

			return new MongoDBTileImporter(loggerFactory, connectionString, databaseName, matchTileCollectionName, directory);
		}

		private static IMongoMatchTileRepository GenerateMongoMatchTileRepository(IConfiguration configuration)
		{
			var connectionString = configuration["MongoDbSettings:ConnectionString"];
			var databaseName = configuration["MongoDbSettings:DatabaseName"];
			var matchTileCollectionName = configuration["MongoDbSettings:TileCollection"];

			return new MongoMatchTileRepository(connectionString, databaseName, matchTileCollectionName);
		}

		private static void AddMediator(IServiceCollection services)
			=> services.AddMediatR(typeof(Program));
	}
}