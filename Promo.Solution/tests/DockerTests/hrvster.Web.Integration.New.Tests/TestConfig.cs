using Microsoft.Extensions.Configuration;

namespace hrvster.Web.TestUtilities
{
    public static class TestConfig
    {
        public static IConfiguration GetConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json", optional: false)
                .Build();
    }
}
