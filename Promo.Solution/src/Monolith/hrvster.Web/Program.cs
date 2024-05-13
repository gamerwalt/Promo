using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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

			Log.Information($"Main - hrvster.WEB - Environment : {env}");

			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env}.json")
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.Enrich.FromLogContext()
				//.WriteTo.Loggly()
				.MinimumLevel.Debug()
				.CreateLogger();

			Log.Information("Main - Starting Web Host for hrvster.WEB");

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
							if (!context.HostingEnvironment.IsDevelopment())
							{
								StaticWebAssetsLoader.UseStaticWebAssets(context.HostingEnvironment, context.Configuration);
							}
						})
						.UseSerilog((context, configuration)
							=> configuration.ReadFrom.Configuration(context.Configuration)
								.Enrich.FromLogContext()
								.WriteTo.Loggly()
								.MinimumLevel.Debug(), preserveStaticLogger: true);
				});
	}
}
