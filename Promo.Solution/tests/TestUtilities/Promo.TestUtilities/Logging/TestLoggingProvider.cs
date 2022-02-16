using Microsoft.Extensions.Logging;

namespace Promo.TestUtilities.Logging
{
    public class TestLoggingProvider : ILoggerProvider
    {
        private readonly ILogger _logger;

        public TestLoggingProvider(TestLogger logger)
        {
            _logger = logger;
        }

        public void Dispose() { }

        public ILogger CreateLogger(string categoryName) => _logger;
    }
}
