using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promo.Campaign.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
						.UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "local")
						.ConfigureAppConfiguration(BuildConfiguration)
						.UseStartup<Startup>();
                });

		private static void BuildConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
		{
			builder.SetBasePath(context.HostingEnvironment.ContentRootPath)
				.AddKeyPerFile("/var/run/secrets/aspnet", true)
				.AddEnvironmentVariables();
		}
	}
}
