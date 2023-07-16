using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Extensions.LogExtensions;
using hrvster.ApplicationServices.MatchGames;
using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Clients;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Scheduling;
using hrvster.Services.Requests;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services.Handlers
{
	public class PublishScheduledPosts : AsyncRequestHandler<PublishScheduledPostsRequest>
	{
		private readonly ILogger<PublishScheduledPosts> _logger;
		private readonly IScheduledPostRepository _scheduledPostRepository;
		private readonly ISweepRulesRepository _sweepRulesRepository;
		private readonly IMatchRulesService _matchRulesService;
		private readonly ITwitterHttpService _twitterHttpService;
		private readonly IHarvesterServiceCommandRepository _harvesterServiceCommandRepository;
		private readonly IProfilesRepository _profileRepository;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly IAccountsService _accountsService;
		private readonly ITwitterHttpImageUploaderService _twitterUploaderService;

		public PublishScheduledPosts(
			ILoggerFactory loggerFactory,
			IScheduledPostRepository scheduledSweepRuleTweetsRepository,
			ISweepRulesRepository sweepRulesRepository,
			IMatchRulesService matchRulesService,
			IHarvesterServiceCommandRepository harvesterServiceCommandRepository,
			ITwitterHttpService twitterHttpService,
			IProfilesRepository profilesRepository,
			IAccountSubscriptionService accountSubscriptionService,
			IAccountsService accountsService,
			ITwitterHttpImageUploaderService twitterUploaderService)
		{
			_logger = loggerFactory.CreateLogger<PublishScheduledPosts>();
			_scheduledPostRepository = scheduledSweepRuleTweetsRepository;
			_sweepRulesRepository = sweepRulesRepository;
			_matchRulesService = matchRulesService;
			_harvesterServiceCommandRepository = harvesterServiceCommandRepository;
			_twitterHttpService = twitterHttpService;
			_profileRepository = profilesRepository;
			_accountSubscriptionService = accountSubscriptionService;
			_accountsService = accountsService;
			_twitterUploaderService = twitterUploaderService;
		}

		protected override async Task Handle(PublishScheduledPostsRequest request, CancellationToken cancellationToken)
		{
			foreach (var scheduledPost in request.ScheduledSweepRuleTweets)
			{
				using (_logger.StartScheduledPostPublishing(
					scheduledPost.Uid, 
					scheduledPost.Profile.ProfileUid, 
					scheduledPost.UrlLink, 
					scheduledPost.GameType.ToString(), 
					scheduledPost.ScheduledTime))
				{
					var profile = await _profileRepository.GetProfileByProfileUidAsync(scheduledPost.Profile.ProfileUid);

					if (profile == null)
					{
						_logger.LogWarning("Unable to locate the profile for this scheduled post.");
						continue;
					}

					var accountSubscription = await _accountSubscriptionService.GetAccountSubscriptionAsync(profile.Account.AccountUid);

					if (accountSubscription == null)
					{
						_logger.LogWarning("Unable to locate the account subscription for the account {accountUid}",
							profile.Account.AccountUid);
						continue;
					}

					var ruleCount = await GetGameRuleCountsForAsync(profile.Account.AccountUid);
					if (ruleCount >= accountSubscription.Plan.GetHashtagCount())
					{
						_logger.LogWarning("Account with uid {accountUid} has a total of {activeRulesCount} active hashtags and their current max is {maxHashtags}",
							profile.Account.AccountUid,
							ruleCount,
							accountSubscription.Plan.HashtagsPerMonth);
						return;
					}

					//if we got here, we can now post a tweet.
					var accessToken = profile.AccessToken;

					if (profile.UpdatedDate.AddHours(2) > DateTime.UtcNow)
					{
						_logger.LogInformation("The profile needs a new refresh token.");

						if (string.IsNullOrWhiteSpace(profile.RefreshToken))
						{
							_logger.LogError("Unable to get a refresh token for the account with uid {accountUid}", profile.Account.AccountUid);
							continue;
						}

						var refreshTokenObject = await _twitterHttpService.GetAccessTokenFromRefreshTokenAsync(profile.RefreshToken);

						accessToken = refreshTokenObject.AccessToken;

						_logger.LogInformation("Updating the profile with uid with new tokens.");

						await _profileRepository.UpdateTokensAsync(
							profile.ProfileUid, refreshTokenObject.AccessToken, refreshTokenObject.RefreshToken);
					}

					_logger.LogInformation("Publishing the scheduled post.");

					var images = await _scheduledPostRepository.GetAllImagesAsync(scheduledPost.Id);
					var postId = string.Empty;

					if(images.Any())
					{
						_logger.LogInformation("Publishing with images.");

						var imageIds = new List<long>();

						foreach(var image in images)
						{
							using var client = new WebClient();
							var byteArray = await client.DownloadDataTaskAsync(new Uri(image.ImageLocation));

							var imageResponse = await _twitterUploaderService.UploadImageAsync(
								byteArray, profile.ExternalProfileId);

							if (string.IsNullOrWhiteSpace(imageResponse.MediaIdString))
							{
								_logger.LogWarning("Unable to upload the image.");
								continue;
							}

							imageIds.Add(long.Parse(imageResponse.MediaIdString));
						}

						postId = await _twitterHttpService.PublishTweetAsync(
							scheduledPost.Post, 
							accessToken, 
							profile.RefreshToken, 
							imageIds);
					}
					else
					{
						_logger.LogInformation("No images to publish.");

						postId = await _twitterHttpService.PublishTweetAsync(
							scheduledPost.Post, 
							accessToken, 
							profile.RefreshToken);
					}

					if (string.IsNullOrWhiteSpace(postId))
					{
						_logger.LogWarning("Unable to continue since we did not get a post id back.");
						continue;
					}

					_logger.LogInformation("Scheduled post published with tweet id {tweetId}", postId);

					if(await UpdateRuleWithPostId(scheduledPost.Rule, postId, scheduledPost.GameType))
					{
						await UpdateScheduledPostStatusAs(scheduledPost, ScheduledStatus.Sent);
					}
					else
					{
						await UpdateScheduledPostStatusAs(scheduledPost, ScheduledStatus.Failed);
					}

					_logger.LogInformation("Finished publishing the scheduled post with Uid {uid}", scheduledPost.Uid);
				}
			}
		}

		private async Task UpdateScheduledPostStatusAs(ScheduledPost scheduledPost, ScheduledStatus status)
		{
			_logger.LogInformation("Updating the scheduled post status as {status}", status);

			await _scheduledPostRepository.UpdateStatusAsync(scheduledPost, status);
		}

		private async Task<bool> UpdateRuleWithPostId(Rule rule, string postId, GameTypesEnum gameType)
		{
			_logger.LogInformation("Updating the rule {ruleId} with the post {postId}",
				rule.RuleId,
				postId);

			switch(gameType)
			{
				case GameTypesEnum.Sweeps:
					return await UpdateSweepRuleWithPostId(rule, postId);
				case GameTypesEnum.MatchGames:
					return await UpdateMatchRuleWithPostId(rule, postId);
				default:
					return await UpdateSweepRuleWithPostId(rule, postId);
			}
		}

		private async Task<bool> UpdateMatchRuleWithPostId(Rule rule, string postId)
		{
			var actualRule = await _matchRulesService.GetRuleByIdAsync(rule.RuleId);

			if(actualRule == null)
			{
				_logger.LogWarning("Unable to update the rule {ruleId} with the post id {postId} since we could not find the rule.",
					rule.RuleId,
					postId);
				return false;
			}

			await _matchRulesService.UpdateRuleExternalIdAsync(actualRule.RuleUid, postId);

			_logger.LogInformation("Finished updating the rule {ruleId} with the post {postId}",
				rule.RuleId,
				postId);

			return true;
		}

		private async Task<bool> UpdateSweepRuleWithPostId(Rule rule, string postId)
		{
			var actualRule = await _sweepRulesRepository.GetRuleByIdAsync(rule.RuleId);

			if(actualRule == null)
			{
				_logger.LogWarning("Unable to update the rule {ruleId} with the post id {postId} since we could not find the rule.",
					rule.RuleId,
					postId);
				return false;
			}

			await _sweepRulesRepository.UpdateRuleExternalId(actualRule.RuleUid, postId);

			_logger.LogInformation("Finished updating the rule {ruleId} with the post {postId}",
				rule.RuleId,
				postId);

			return true;
		}

		private async Task<int> GetGameRuleCountsForAsync(Guid userUid)
		{
			var rules = await _accountsService.GetAllRulesAsync(userUid);

			if (rules == null) return 0;

			return rules.GetActiveRules().Count();
		}
	}
}
