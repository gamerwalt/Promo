using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Harvesters.Facebook;
using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Clients.Common;
using hrvster.Core.Clients.Publishers;
using hrvster.Core.Requests.Rules.Notifications;
using hrvster.Models;
using hrvster.Models.Options;
using hrvster.Models.Profiles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using hrvster.Models.Extensions;
using System.Linq;
using FirebaseAdmin.Messaging;

namespace hrvster.ApplicationServices.Publishing
{
	public interface IPublishService
	{
		Task<bool> PublishAsync(Guid ruleUid, string urlLink, Profile profile, int gameId, Guid gameUid, GameTypesEnum gameType, IList<IFormFile> media, IList<string> mediaLocation, string token, Guid userUid, string watchValue, int watchTypeId, string tweetText, ExternalClientType externalClientType, bool following, string pageId);
		Task<bool> SaveScheduledPostToPublishAsync(Guid ruleUid, string urlLink, Guid userUid, int gameId, Guid gameUid, Profile profile, int watchTypeId, string watchValue, string tweetText, DateTime scheduledDate, IList<string> mediaLocation, ExternalClientType externalClientType, bool following, string pageId);
	}

	public class PublishService : IPublishService
	{
		private readonly ILogger<PublishService> _logger;
		private readonly Func<ExternalClientType, IExternalClient> _publisherResolver;
		private readonly IMediator _mediator;
		private readonly IFacebookPageSerivce _facebookPageService;
		private readonly HarvesterSettingsOptions _hrvsterSettings;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly IAccountsService _accountService;

		public PublishService(
			ILoggerFactory loggerFactory,
			Func<ExternalClientType, IExternalClient> publisherResolver,
			IMediator mediator,
			IFacebookPageSerivce facebookPageSerivce,
			HarvesterSettingsOptions hrvsterSettings,
			IAccountSubscriptionService accountSubscriptionService,
			IAccountsService accountsService)
		{
			_logger = loggerFactory.CreateLogger<PublishService>();
			_publisherResolver = publisherResolver;
			_mediator = mediator;
			_facebookPageService = facebookPageSerivce;
			_hrvsterSettings = hrvsterSettings;
			_accountSubscriptionService = accountSubscriptionService;
			_accountService = accountsService;
		}

		public async Task<bool> PublishAsync(Guid ruleUid, string urlLink, Profile profile, int gameId, Guid gameUid, GameTypesEnum gameType, IList<IFormFile> media, IList<string> mediaLocation, string token, Guid userUid, string watchValue, int watchTypeId, string tweetText, ExternalClientType externalClientType, bool following, string pageId = "")
		{
			_logger.LogInformation("Publishing the tweet {tweetText} with the hashtag {watchValue} for the user with Uid {userUid} and profile name {screenName} for the External Client {externalClient}",
				tweetText,
				watchValue,
				userUid,
				profile.ScreenName,
				externalClientType.ToString());

			var ruleCount = await GetGameRuleCountsForAsync(profile.Account.AccountUid);
			if (ruleCount >= _hrvsterSettings.MaxRulesPerAccount)
			{
				_logger.LogError("The user with Uid {userUid} has reached the maximum number of rules allowed", userUid);

				return false;
			}

			var publisher = _publisherResolver(externalClientType);

			var message = BuildMessage(gameUid, profile, mediaLocation, watchValue, watchTypeId, tweetText);
			if (!string.IsNullOrWhiteSpace(pageId))
			{
				var page = await _facebookPageService.GetPageAsync(pageId);
				message.PageId = pageId;
				message.PageAccessToken = page.PageAccessToken;
			}

			var externalId = await publisher.PublishAsync(message);

			if (string.IsNullOrWhiteSpace(externalId))
			{
				_logger.LogError("Unable to use client type {externalClientType} publish the text {text} for game with uid {uid} for {profileName} and {profileUid}",
					externalClientType,
					message.Text,
					message.GameUid,
					message.Profile.ScreenName,
					message.Profile.ProfileUid);

				return false;
			}

			await _mediator.Publish(new RulePublished(ruleUid, urlLink, profile, externalId, message, following, gameId, gameUid, gameType, externalClientType, pageId));

			return true;
		}

		private static Core.Clients.Common.Message BuildMessage(
			Guid gameUid, Profile profile, IList<string> mediaLocation, string watchValue, int watchTypeId, string tweetText)
		{
			return new Core.Clients.Common.Message
			{
				GameUid = gameUid,
				Hashtag = watchValue,
				IsScheduled = false,
				Profile = profile,
				Media = mediaLocation,
				Text = tweetText,
				WatchTypeId = watchTypeId
			};
		}

		public async Task<bool> SaveScheduledPostToPublishAsync(
			Guid ruleUid,
			string urlLink,
			Guid userUid,
			int gameId,
			Guid gameUid,
			Profile profile,
			int watchTypeId,
			string watchValue,
			string tweetText,
			DateTime scheduledDate,
			IList<string> mediaLocation,
			ExternalClientType externalClientType,
			bool following, string pageId = "")
		{
			_logger.LogInformation("Scheduling the tweet {tweetText} with the hashtag {watchValue} for the user with Uid {userUid} and profile name {screenName} for the External Client {externalClient}",
				tweetText,
				watchValue,
				userUid,
				profile.ScreenName,
				externalClientType.ToString());

			//Todo: Finish the scheduling of posts
			//Todo: Finish the sending of the scheduled posts

			return true;
		}

		private async Task<int> GetGameRuleCountsForAsync(Guid userUid)
		{
			var rules = await _accountService.GetAllRulesAsync(userUid);

			if (rules == null) return 0;

			return rules.GetActiveRules().Count();
		}
	}
}
