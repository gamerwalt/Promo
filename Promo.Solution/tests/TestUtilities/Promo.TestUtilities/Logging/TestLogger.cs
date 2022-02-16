using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Promo.TestUtilities.Logging
{
    public class TestLogger : ILogger
    {
        private List<TestLogMessage> _logs = new List<TestLogMessage>();
        public IEnumerable<TestLogMessage> Logs => _logs;

        public IEnumerable<string> Messages => _logs.Select(l => l.FormattedMessage);
        public IEnumerable<TestLogScope> Scopes => _scopes;
        private List<TestLogScope> _scopes = new List<TestLogScope>();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _logs.Add(new TestLogMessage
            {
                LogLevel = logLevel,
                EventId = eventId,
                State = state,
                StateType = typeof(TState),
                Exception = exception,
                FormattedMessage = formatter(state, exception)
            });
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state)
        {
            if (state is Dictionary<string, object> properties)
            {
                _scopes.Add(new TestLogScope
                {
                    Properties = properties
                });
            }

            return Mock.Of<IDisposable>();
        }

        public void Clear()
        {
            _logs = new List<TestLogMessage>();
            _scopes = new List<TestLogScope>();
        }
    }
}
