using hrvster.Core.Clients.Common;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models;
using hrvster.Models.Scheduling;
using hrvster.Services.Requests;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services.Handlers
{
	public class PublishScheduledTweets : AsyncRequestHandler<PublishScheduledTweetsRequest>
	{
		private readonly ILogger<PublishScheduledTweets> _logger;
		private readonly Func<ExternalClientType, Core.Clients.Publishers.IExternalClient> _publisherResolver;
		private readonly IScheduledSweepRuleTweetsRepository _scheduledSweepRuleTweetsRepository;
		private readonly ISweepRulesRepository _sweepRulesRepository;
		private readonly IHarvesterServiceCommandRepository _harvesterServiceCommandRepository;

		public PublishScheduledTweets(
			ILoggerFactory loggerFactory,
			Func<ExternalClientType, Core.Clients.Publishers.IExternalClient> publisherResolver,
			IScheduledSweepRuleTweetsRepository scheduledSweepRuleTweetsRepository,
			ISweepRulesRepository sweepRulesRepository,
			IHarvesterServiceCommandRepository harvesterServiceCommandRepository)
		{
			_logger = loggerFactory.CreateLogger<PublishScheduledTweets>();
			_publisherResolver = publisherResolver;
			_scheduledSweepRuleTweetsRepository = scheduledSweepRuleTweetsRepository;
			_sweepRulesRepository = sweepRulesRepository;
			_harvesterServiceCommandRepository = harvesterServiceCommandRepository;
		}

		protected override async Task Handle(PublishScheduledTweetsRequest request, CancellationToken cancellationToken)
		{
			var twitterPublisher = _publisherResolver(ExternalClientType.Twitter);

			foreach (var scheduledTweet in request.ScheduledSweepRuleTweets)
			{
				_logger.LogInformation("Publishing the scheduled tweet for the profile with uid {profileUid} and Rule uid {ruleUid}",
					scheduledTweet.Profile.ProfileUid,
					scheduledTweet.Rule.RuleUid);

				var tweetId = await twitterPublisher.PublishAsync(CreateMessageFrom(scheduledTweet));

				if (string.IsNullOrWhiteSpace(tweetId))
				{
					_logger.LogError("An issue occurred trying to publish the tweet for scheduled sweep rule tweet with uid {uid}",
						scheduledTweet.ScheduledUid);

					await _scheduledSweepRuleTweetsRepository.UpdateStatus(scheduledTweet, ScheduledStatus.Failed);

					continue;
				}

				_logger.LogInformation("Published the scheduled tweet with id {tweetId} for the profile with uid {profileUid} and Rule uid {ruleUid}",
					tweetId,
					scheduledTweet.Profile.ProfileUid,
					scheduledTweet.Rule.RuleUid);

				await UpdateScheduledTweetAndRuleAsync(scheduledTweet, tweetId);

				_logger.LogInformation("Writing a command to restart the Harvester Service to pick up the new hashtag");
				await _harvesterServiceCommandRepository.CreateAsync(new HarvesterServiceCommandModel
				{
					Uid = Guid.NewGuid(),
					Command = HarvesterServiceCommand.start.ToString()
				}, cancellationToken);
			}
		}

		private async Task UpdateScheduledTweetAndRuleAsync(ScheduledSweepRuleTweet scheduledTweet, string tweetId)
		{
			await _scheduledSweepRuleTweetsRepository.UpdateStatus(scheduledTweet, ScheduledStatus.Sent);
			await _sweepRulesRepository.UpdateRuleExternalId(scheduledTweet.Rule.RuleUid, tweetId);
		}

		private Message CreateMessageFrom(ScheduledSweepRuleTweet scheduledTweet)
		{
			return new Message
			{
				Text = scheduledTweet.Text,
				Media = scheduledTweet.Images.Select(item => item.ImageLocation).ToList(),
				Profile = scheduledTweet.Profile
			};
		}
	}
}
