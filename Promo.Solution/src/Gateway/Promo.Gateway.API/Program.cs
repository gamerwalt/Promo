using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promo.Gateway.API
{
    public class Program
    {
		private static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public static void Main(string[] args)
        {
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env}.json")
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();

			CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((host, config) =>
				{
					config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
				})
                .ConfigureWebHostDefaults(webBuilder =>
                {
					Log.Information("Starting Web Host for Promo.Gateway.API");
					
					webBuilder
						.UseStartup<Startup>()
						.UseSerilog(ConfigureSerilog, preserveStaticLogger: true, true);
				});

		private static void ConfigureSerilog(WebHostBuilderContext hostingContext, LoggerConfiguration loggerConfiguration)
			=> loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
	}
}
