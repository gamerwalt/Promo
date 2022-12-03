using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace TestUtilities.Framework
{
    public static class TestServerFactory
    {
        public static TestServer CreateTestServer(Action<IConfiguration> additionalConfiguration, Action<IServiceCollection> additionalServiceConfiguration = null)
        {
            var testServer = new HostBuilder()
                .ConfigureWebHost(builder => builder.ConfigureForTest(additionalConfiguration, additionalServiceConfiguration))
                .Start()
                .GetTestServer();

            return testServer;
        }
    }
}
