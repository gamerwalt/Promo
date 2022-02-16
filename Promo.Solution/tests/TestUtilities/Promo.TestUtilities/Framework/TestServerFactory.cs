using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Promo.TestUtilities.Logging;
using System;

namespace Promo.TestUtilities.Framework
{
    public static class TestServerFactory
    {
        public static TestServer CreateMembershipTestServer(
            TestLogger logger,
            Action<IConfiguration> additionalConfiguration,
            Action<IServiceCollection> additionalServiceConfiguration = null)
        {
            return new HostBuilder()
                .ConfigureWebHost(builder => builder.ConfigureMembershipApiForTest(logger, additionalConfiguration, additionalServiceConfiguration))
                .Start()
                .GetTestServer();
        }
    }
}
