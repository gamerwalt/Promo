using hrvster.Models.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
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

			Log.Information("Creating Web Host for hrvster.WEB for environment {env}", env);
			Log.Information("Configuration loaded... {config}", configuration.ToJsonString());
			Log.Information("Databases : {databasesConfiguration}", configuration.GetConnectionString("hrvster"));

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
						.ConfigureAppConfiguration((context, config) =>
						{
							if(!context.HostingEnvironment.IsDevelopment())
							{
								StaticWebAssetsLoader.UseStaticWebAssets(context.HostingEnvironment, context.Configuration);
							}
						})
						.UseSerilog(ConfigureSerilog, preserveStaticLogger: true, true);
				});

		private static void ConfigureSerilog(WebHostBuilderContext hostingContext, LoggerConfiguration loggerConfiguration)
			=> loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
	}
}
