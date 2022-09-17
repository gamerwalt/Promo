using hrvster.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace TestUtilities.Framework
{
    public static class TestWebHostBuilderExtension
    {
        public static IWebHostBuilder ConfigureForTest(
            this IWebHostBuilder builder,
            Action<IConfiguration> additionalConfiguration = null,
            Action<IServiceCollection> additionalServiceConfiguration = null)
        {
            var configuration = TestConfig.GetConfiguration();
            additionalConfiguration?.Invoke(configuration);

            builder
                .UseTestServer()
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .UseSerilog(ConfigureSerilog, preserveStaticLogger: true, true);

            builder.ConfigureTestServices(services => AddTestServices(services, configuration));

            if (additionalServiceConfiguration != null)
                builder.ConfigureTestServices(additionalServiceConfiguration);

            return builder;
        }

        private static void ConfigureSerilog(WebHostBuilderContext hostingContext, LoggerConfiguration loggerConfiguration)
            => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);

        public static void AddTestServices(IServiceCollection services, IConfiguration configuration)
        {
            //Mock Authentication here
            //Mock Logging here
            //Mock Test Database connection 
        }
    }
}
