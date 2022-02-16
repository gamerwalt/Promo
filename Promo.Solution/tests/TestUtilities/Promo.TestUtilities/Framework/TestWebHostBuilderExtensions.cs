using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promo.Membership.API;
using Promo.TestUtilities.Common;
using Promo.TestUtilities.Extensions;
using Promo.TestUtilities.Logging;
using System;

namespace Promo.TestUtilities.Framework
{
    public static class TestWebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureMembershipApiForTest(this IWebHostBuilder builder,
            TestLogger testLogger,
            Action<IConfiguration> additionalConfiguration = null,
            Action<IServiceCollection> additionalServiceConfiguration = null)
        {
            var configuration = TestConfig.GetConfiguration();

            additionalConfiguration?.Invoke(configuration);

            builder
                .UseTestServer()
                .UseConfiguration(configuration)
                .UseStartup<Startup>();

            builder.ConfigureTestServices(services => AddTestServices(services, configuration, testLogger));

            return builder;
        }

        private static void AddTestServices(IServiceCollection services, IConfiguration configuration, TestLogger testLogger)
        {
            services.AddTestLogging(configuration, testLogger);
            services.AddDatabaseForTesting(configuration);
        }
    }
}
