using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace hrvster.ApplicationServices.Extensions.LogExtensions
{
	public static class Pick3GameLoggerExtensions
	{
		public static IDisposable Pick3MatchPlayLogging(
			this ILogger logger,
			Guid gameUid,
			Guid ruleUid,
			string externalClient)
		{
			return logger.BeginScope(new Dictionary<string, object>()
			{
				{"GameUid", gameUid},
				{"RuleUid", ruleUid },
				{"ExternalClient", externalClient}
			});
		}
	}
}
