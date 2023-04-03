using hrvster.ApplicationServices.Common;
using hrvster.Core.Requests.MatchGames.Harvester;
using hrvster.Models;
using hrvster.Models.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Models;
using hrvster.Models.Extensions;
using hrvster.Infrastructure.Core;
using hrvster.Infrastructure.Reporting;
using hrvster.Infrastructure.MatchGames;
using System.Linq;
using hrvster.Infrastructure.Accounts;
using hrvster.Models.Profiles;
using Tweetinvi;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure.Harvester;
using hrvster.Games.Match.Models;

namespace hrvster.Core.Handlers.MatchGame
{
	public class ProcessPlayMatchGameHashtagReply : AsyncRequestHandler<ProcessPlayMatchGameHashtagReplyRequest>
	{
		private readonly ILogger<ProcessPlayMatchGameHashtagReply> _logger;
		private readonly HarvesterSettingsOptions _harvesterSettings;
		private readonly SignatureUtility _signatureUtility;
		private readonly IIncomingTweetsRepository _incomingTweetsRepository;
		private readonly ISignatureRepository _signatureRepository;
		private readonly IMatchGamePlayReportingRepository _matchGamePlayReportingRepository;
		private readonly IMatchPlayRulesRepository _matchPlayRulesRepository;
		private readonly IProfilesRepository _profilesRepository;
		private readonly TwitterAccount _twitterAccount;
		private readonly IMediator _mediator;

		private TwitterClient _twitterClient;

		public ProcessPlayMatchGameHashtagReply(
			ILoggerFactory loggerFactory,
			HarvesterSettingsOptions harvsterSettings,
			SignatureUtility signatureUtility,
			IIncomingTweetsRepository incomingTweetsRepository,
			ISignatureRepository signatureRepository,
			IMatchGamePlayReportingRepository matchGamePlayReportingRepository,
			IMatchPlayRulesRepository matchPlayRulesRepository,
			IProfilesRepository profilesRepository,
			TwitterAccount twitterAccount,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<ProcessPlayMatchGameHashtagReply>();
			_harvesterSettings = harvsterSettings;
			_signatureUtility = signatureUtility;
			_incomingTweetsRepository = incomingTweetsRepository;
			_signatureRepository = signatureRepository;
			_matchGamePlayReportingRepository = matchGamePlayReportingRepository;
			_matchPlayRulesRepository = matchPlayRulesRepository;
			_profilesRepository = profilesRepository;
			_twitterAccount = twitterAccount;
			_mediator = mediator;
		}

		protected async override Task Handle(ProcessPlayMatchGameHashtagReplyRequest request, CancellationToken cancellationToken)
		{
			using (
				_logger.BeginScope(
					new Dictionary<string, object> {
					{"IncomingTweetUid", request.IncomingTweet.IncomingTweetUid },
					{"TweetId", request.Tweet.Id },
					{ "HarvestedUid", request.HarvestedUid },
					{ "UserUid", request.UserUid },
					{ "GameUid", request.Rule.Game.Uid },
					{ "GameName", request.Rule.Game.Name}})
				)
			{
				_logger.LogInformation("Checking if user can play the game.");

				if (await CanPlayMatchGame(request.Rule, request.Tweet, cancellationToken))
				{
					_logger.LogInformation("User can play the game.");

					_logger.LogInformation("Checking if the tweet is in reply to the rule/tweet of the hashtag setup.");
					if (request.Tweet.InReplyToStatusId.Value.ToString() == request.Rule.ExternalId)
					{
						_logger.LogInformation("Tweet is in reply to the rule/tweet of the hashtag setup");

						if (request.BlacklistedUsers.Any(c => c.UserId.Equals(request.Tweet.CreatedBy.IdStr)))
						{
							_logger.LogWarning("User with screen name {screenName} is within the blacklisted users.",
								request.Tweet.CreatedBy.ScreenName);

							return;
						}
						else
						{
							_logger.LogInformation("User with screen name {screenName} is not within the blacklisted list. User can play the game.",
								request.Tweet.CreatedBy.ScreenName);
						}
					}
					else
					{
						_logger.LogWarning("Tweet is not in reply to the rule/tweet of the hashtag setup.");

						return;
					}
				}
				else
				{
					_logger.LogWarning("User with screen name {screenName} cannot play the game anymore.", request.Tweet.CreatedBy.ScreenName);

					return;
				}

				ContinueToProcessAsync(request, cancellationToken);
			}
		}

		private async void ContinueToProcessAsync(ProcessPlayMatchGameHashtagReplyRequest request, CancellationToken cancellationToken)
		{
			var profile = await _profilesRepository.GetProfileByCampaignUid(request.Rule.Campaign.CampaignUid, ProfileType.Twitter);

			_logger.LogInformation("Creating a signature to use for the game play.");

			var signatureUid = Guid.NewGuid();

			var signature = CreateSignature(signatureUid, request.Rule.Game.Uid, request.Tweet);
			var url = new Uri(_harvesterSettings.HrvsterUrl)
				.GenerateUrl(UrlTypes.MatchGamePlayLink)
				.ToString()
				.Replace("{GameUid}", $"{request.Rule.Game.Uid}");
			var link = $"{url}?signature={signature}";

			_logger.LogInformation("Geneated signature and link required to play the game.");
			_logger.LogInformation("Saving the signature for verification purposes.");

			if(await IsFollowing(profile, request))
			{
				if(CampaignAndMatchGameIsStillRunning(request.Rule))
				{
					var playRules = await _matchPlayRulesRepository.GetAsync(request.Rule.Game.Uid, cancellationToken);

					if (playRules == null)
					{
						_logger.LogError("Could not load play rules. There seems to be an issue.");

						return;
					}

					await SendLinkToPlayGameAsync(link, request.Rule, request.Tweet, profile);

					await SaveSignatureAsync(signatureUid, link, signature, request, playRules, cancellationToken);
				}
				else
				{
					await _incomingTweetsRepository.UpdateStatusAsync(
						request.IncomingTweet.IncomingTweetUid,
						Models.Harvester.IncomingTweetStatus.CompletedCampaignOrGameNotRunning);
				}
			}
			else
			{
				_logger.LogInformation("The user {screenName} is not following Profile {accountScreenName}",
							request.Tweet.CreatedBy.ScreenName,
							profile.ScreenName);

				//await _mediator.Send(new NotifyUserOfNotFollowingAccountToPlayMatchGameRequest(request.Tweet,profile, request.IncomingTweet));
			}
		}

		private async Task SaveSignatureAsync(
			Guid signatureUid,
			string link, 
			string signature, 
			ProcessPlayMatchGameHashtagReplyRequest request,
			MatchPlayRule playRules,
			CancellationToken cancellationToken)
		{
			
			await _signatureRepository.CreateAsync(
				signatureUid, 
				signature, 
				link, 
				DateTime.UtcNow.AddMinutes(_harvesterSettings.SignatureExpiryInMinutes), 
				request.Rule.Game.Uid, 
				request.Tweet.CreatedBy.IdStr, 
				request.Tweet.CreatedBy.ScreenName, 
				playRules.PlayPerTweet, 
				cancellationToken);
		}

		private async Task SendLinkToPlayGameAsync(string link, Rule rule, ITweet tweet, Profile profile)
		{
			await _mediator.Send(new SendTwitterLinkToPlayGameRequest(link, rule, tweet, profile));
		}

		private bool CampaignAndMatchGameIsStillRunning(Rule rule)
		{
			if(CampaignIsNotRunning(rule.Campaign))
			{
				_logger.LogWarning("Campaign {campaignName} is not running. Campaign StartDate: {startDate}. Campaign EndDate: {endDate}",
					rule.Campaign.CampaignName,
					rule.Campaign.StartDateUtc,
					rule.Campaign.EndDateUtc);

				return false;
			}

			if(MatchGameIsNotActive(rule.Game))
			{
				_logger.LogWarning("Match game {gameName} is not running or active. Game Start Date: {startDate}. Game End Date {endDate}",
					rule.Game.Name,
					rule.Game.StartDateUtc,
					rule.Game.EndDateUtc);

				return false;
			}

			return true;
		}

		private bool MatchGameIsNotActive(Game game)
			=> DateTime.UtcNow > game.EndDateUtc ||
				DateTime.UtcNow < game.StartDateUtc;

		private static bool CampaignIsNotRunning(Campaign campaign)
			=> DateTime.UtcNow > campaign.EndDateUtc ||
				DateTime.UtcNow < campaign.StartDateUtc;

		private async Task<bool> IsFollowing(Profile profile, ProcessPlayMatchGameHashtagReplyRequest request)
		{
			if (profile == null)
			{
				_logger.LogError("Unable to retrieve the profile belonging to the campaign uid {campaignUid} and campaign name {campaignName}",
					request.Rule.Campaign.CampaignUid,
					request.Rule.Campaign.CampaignName);

				return false;
			}

			BuildTwitterClient(profile.AccessToken, profile.AccessTokenSecret);

			var followers = await _twitterClient.Users.GetFollowerIdsAsync(profile.ScreenName);

			if (request.Rule.MustBeFollowing)
				return followers.Any(c => c.Equals(request.Tweet.CreatedBy.Id));

			return true;
		}

		private void BuildTwitterClient(string accessToken, string accessTokenSecret)
		{
			_twitterClient = new TwitterClient(
				_twitterAccount.ConsumerKey,
				_twitterAccount.ConsumerSecret,
				accessToken,
				accessTokenSecret);
		}

		private async Task<bool> CanPlayMatchGame(Rule rule, ITweet tweet, CancellationToken cancellationToken)
		{
			var report = await _matchGamePlayReportingRepository.GetGamePlayReportAsync(
				rule.Game.Uid,
				rule.Campaign.CampaignUid,
				UserIdentifierType.TwitterHandle.ToString(),
				tweet.CreatedBy.IdStr,
				tweet.CreatedBy.ScreenName,
				cancellationToken);

			if (report == null) return true;

			var playRules = await _matchPlayRulesRepository.GetAsync(rule.Game.Uid, cancellationToken);

			if (playRules == null) return false;

			return report.Plays < playRules.PlayPerGame;
		}

		private string CreateSignature(Guid signatureUid, Guid gameUid, ITweet tweet)
			=> _signatureUtility.CreateSignature(signatureUid, tweet.CreatedBy.Id, tweet.CreatedBy.ScreenName, gameUid);
		
	}
}
