using hrvster.Core.Clients.Publishers;
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

		public static IDisposable BeginPrePlayForPick3GameScope(
			this ILogger logger,
			Guid gameUid,
			Guid ruleUid,
			string externalClient)
		{
			return logger.BeginScope(new Dictionary<string, object>()
			{
				{"GameUid", gameUid },
				{"RuleUid", ruleUid },
				{"ExternalClient", externalClient}
			});
		}

		public static IDisposable BeginGetPlayForPick3GameScope(
			this ILogger logger,
			Guid gameUid,
			Guid ruleUid,
			string externalClient,
			Guid gameplayUid)
		{
			return logger.BeginScope(new Dictionary<string, object>()
			{
				{"GameUid", gameUid },
				{"RuleUid", ruleUid },
				{"ExternalClient", externalClient},
				{"GameplayUid", gameplayUid}
			});
		}

		public static IDisposable BeginPostPlayForPick3GameScope(
			this ILogger logger,
			Guid gameUid,
			Guid ruleUid,
			string externalClient,
			Guid gameplayUid)
		{
			return logger.BeginScope(new Dictionary<string, object>()
			{
				{"GameUid", gameUid },
				{"RuleUid", ruleUid },
				{"ExternalClient", externalClient},
				{"GameplayUid", gameplayUid}
			});
		}
	}
}
