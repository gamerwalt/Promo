using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace hrvster.ApplicationServices.Extensions.LogExtensions
{
	public static class SweepsGameLoggerExtensions
	{
		public static IDisposable BeginPreplayForSweepsGameScope(
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

		public static IDisposable BeginPlayForSweepsGameScope(
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

		public static IDisposable BeginPostPlayForSweepsGameScope(
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
