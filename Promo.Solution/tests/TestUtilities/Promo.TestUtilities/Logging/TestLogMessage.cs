using Microsoft.Extensions.Logging;
using System;

namespace Promo.TestUtilities.Logging
{
    public class TestLogMessage
    {
        public LogLevel LogLevel { get; set; }
        public EventId EventId { get; set; }
        public object State { get; set; }
        public Type StateType { get; set; }
        public Exception Exception { get; set; }
        public string FormattedMessage { get; set; }
    }
}
