using Microsoft.Extensions.Configuration;
using System.IO;

namespace Promo.TestUtilities.Common
{
    public static class TestConfig
    {
        public static IConfiguration GetConfiguration()
            => new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json", optional: false)
                .Build();
    }
}