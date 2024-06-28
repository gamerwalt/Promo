using hrvster.ApplicationServices.JobContracts;
using log4net;
using log4net.Config;
using log4net.Util;
using MediatR;

namespace hrvster.Services.RedisGame.Purge
{
	public class Program
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));
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

			var log4NetConfigFile = Configuration["Logging:Log4NetConfigFile"];
			var log4NetConfigFileInfo = new FileInfo(log4NetConfigFile);
			XmlConfigurator.ConfigureAndWatch(log4NetConfigFileInfo);

			LogicalThreadContext.Properties["tags"] = $"hrvster.Services.RedisGame.Purge-{env}, RedisGame.Purge-{env}";
			LogicalThreadContext.Properties["environment"] = env;
			LogicalThreadContext.Properties["Service"] = "hrvster.Services.RedisGame.Purge";
			LogicalThreadContext.Properties["Job"] = "hrvster.Services.RedisGame.Purge";
			GlobalContext.Properties["tags"] = $"hrvster.Services.RedisGame.Purge-{env}, RedisGame.Purge-{env}";

			_logger.Info("Application hrvster.Services.RedisGame.Purge is starting.");

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<PurgeRedisHostedService>();
					services.AddSingleton<IService, PurgeRedisService>();

					services.AddSingleton<ILog>(LogManager.GetLogger(typeof(Program)));


					AddMediator(services);
				});

		private static void AddMediator(IServiceCollection services)
			=> services.AddMediatR(typeof(Program));
	}
}