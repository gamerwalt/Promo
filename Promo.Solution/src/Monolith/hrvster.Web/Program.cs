using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace hrvster.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
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
				.ConfigureWebHostDefaults(webBuilder =>
				{
					Log.Information("Starting Web Host for hrvster.WEB");

					webBuilder
						.UseStartup<Startup>()
						.UseContentRoot(Directory.GetCurrentDirectory())
						.UseSerilog(ConfigureSerilog, preserveStaticLogger: true, true);
				});

		private static void ConfigureSerilog(WebHostBuilderContext hostingContext, LoggerConfiguration loggerConfiguration)
			=> loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
	}
}
