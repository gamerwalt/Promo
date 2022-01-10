using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promo.Winners.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
				.Enrich.FromLogContext()
				.MinimumLevel.Debug()
				.WriteTo.Console()
				.WriteTo.Loggly()
				.CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
                {
					Log.Information("Starting Web Host for Promo.Winners.API");
                    webBuilder
						.UseStartup<Startup>()
						.UseSerilog(ConfigureSerilog, preserveStaticLogger: true, true);
                });

		private static void ConfigureSerilog(WebHostBuilderContext hostingContext, LoggerConfiguration loggerConfiguration) 
			=> loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
	}
}
