using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Promo.Core;
using Promo.TestUtilities.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promo.TestUtilities.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTestLogging(
            this IServiceCollection services,
            IConfiguration configuration,
            TestLogger testLogger)
        {
            var serilogLogger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            if (testLogger != null)
                services.AddLogging(builder => builder
                    .AddProvider(new TestLoggingProvider(testLogger)));

            services.AddLogging(builder => builder
                .AddSerilog(serilogLogger, dispose: true));

            return services;
        }

        public static IServiceCollection AddDatabaseForTesting(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddSingleton<IDatabase>(c => AddTestingDatabase(c, configuration));
        }

        private static IDatabase AddTestingDatabase(IServiceProvider sp, IConfiguration configuration)
        {
            return new MySqlDatabase("TestDatabase", configuration);
        }
    }
}
