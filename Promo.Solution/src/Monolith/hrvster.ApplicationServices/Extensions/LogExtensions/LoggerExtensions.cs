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

		public static IDisposable SweepsGameLoggingEntry(
			this ILogger logger,
			Guid ruleUid,
			Guid gameUid,
			Guid campaignUid,
			Guid accountUid)
		{
			return logger.BeginScope(new Dictionary<string, object>()
			{
				{"RuleUid", ruleUid },
				{"GameUid", gameUid },
				{"CampaignUid", campaignUid },
				{"AccountUid", accountUid },
			});
		}

		public static IDisposable Pick3MatchGameLoggingEntry(
			this ILogger logger,
			Guid ruleUid,
			Guid gameUid,
			Guid campaignUid,
			Guid accountUid)
		{
			return logger.BeginScope(new Dictionary<string, object>()
			{
				{"RuleUid", ruleUid },
				{"GameUid", gameUid },
				{"CampaignUid", campaignUid },
				{"AccountUid", accountUid },
			});
		}

		public static IDisposable CreateMemberPageLogScope(
			this ILogger logger,
			Guid ruleUid,
			Guid gameUid,
			string externalClient,
			Guid accountUid)
			=> logger.BeginScope(new Dictionary<string, object>()
			{
				{"RuleUid", ruleUid },
				{"GameUid", gameUid },
				{"ExternalClient", externalClient },
				{"AccountUid", accountUid },
			});
		
		public static IDisposable CreateMemberPagePostLogScope(
			this ILogger logger,
			Guid ruleUid,
			Guid gameUid,
			string externalClient,
			Guid accountUid)
			=> logger.CreateMemberPageLogScope(ruleUid, gameUid, externalClient, accountUid);

		public static IDisposable ValidatingTwitterAuthBeginScope(
			this ILogger logger,
			Guid ruleUid,
			Guid gameUid,
			string externalClient,
			string gameType)
		{
			return logger.BeginScope(new Dictionary<string, object>()
			{
				{"RuleUid", ruleUid },
				{"GameUid", gameUid },
				{"ExternalClient", externalClient },
				{"GameType", gameType },
			});
		}

		public static IDisposable GameServiceGameExistsForAccount(
			this ILogger logger,
			Guid gameUid,
			Guid accountUid)
		{
			return logger.BeginScope(new Dictionary<string, object>()
			{
				{"GameUid", gameUid },
				{"AccountUid", accountUid },
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
