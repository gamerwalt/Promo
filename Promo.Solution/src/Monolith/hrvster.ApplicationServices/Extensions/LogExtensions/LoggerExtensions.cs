using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace hrvster.ApplicationServices.Extensions.LogExtensions
{
	public static class LoggerExtensions
	{
		public static IDisposable StartSaveScheduledPostToPublishAsyncScope(
			this ILogger logger, 
			Guid ruleUid, 
			Guid gameUid, 
			Models.Profiles.Profile profile, 
			string watchValue, 
			Models.GameTypesEnum gameType)
		{
			return logger.BeginScope(new Dictionary<string, object>()
			{
				{"RuleUid", ruleUid },
				{"GameUid", gameUid },
				{"ProfileUid", profile.ProfileUid },
				{"AccountUid", profile.Account.AccountUid },
				{"Hashtag", watchValue },
				{"GameType", gameType.ToString()}
			});
		}

		public static IDisposable StartScheduledPostPublishing(
			this ILogger logger,
			Guid scheduledPostUid,
			Guid profileUid,
			string urlLink,
			string gameType,
			DateTime scheduledTime)
		{
			return logger.BeginScope(new Dictionary<string, object>()
			{
				{"ScheduledPostUid", scheduledPostUid},
				{"ProfileUid", profileUid },
				{"UrlLink", urlLink},
				{"GameType", gameType},
				{"ScheduledTime", scheduledTime}
			});
		}

		public static IDisposable BeginGenerateBoard(
			this ILogger logger,
			string gameName,
			Guid gameGuid)
		{
			return logger.BeginScope(new Dictionary<string, object>()
			{
				{"Game Name", gameName},
				{"Game Guid", gameGuid}
			});
		}
	}
}
