using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Publishing
{
	public class DeleteSweepTweetRule : AsyncRequestHandler<DeleteSweepTweetRuleRequest>
	{
		private readonly ILogger<DeleteSweepTweetRule> _logger;
		private readonly ISweepRulesRepository _sweepRulesRepository;
		private readonly IProfilesRepository _profilesRepository;
		private readonly Func<ExternalClientType, Clients.Publishers.IExternalClient> _publisherResolver;

		public DeleteSweepTweetRule(
			ILoggerFactory loggerFactory,
			ISweepRulesRepository sweepRulesRepository,
			IProfilesRepository profilesRepository,
			Func<ExternalClientType, Clients.Publishers.IExternalClient> publisherResolver)
		{
			_logger = loggerFactory.CreateLogger<DeleteSweepTweetRule>();
			_sweepRulesRepository = sweepRulesRepository;
			_profilesRepository = profilesRepository;
			_publisherResolver = publisherResolver;
		}

		protected override async Task Handle(DeleteSweepTweetRuleRequest request, CancellationToken cancellationToken)
		{
			var rule = await _sweepRulesRepository.GetAsync(request.RuleUid);
			if (rule == null)
			{
				_logger.LogError("Could not load the rule with uid: {ruleUid}", request.RuleUid);
				return;
			}

			//we get the profile
			var profile = await _profilesRepository.GetProfileAsync(request.UserUid, Models.ProfileType.Twitter);
			if (profile == null)
			{
				_logger.LogError("Could not load the profile with AccountUid: {userUid}", request.UserUid);
				return;
			}

			var externalId = rule.ExternalId;
			var twitterPublisher = _publisherResolver(ExternalClientType.Twitter);
			await twitterPublisher.DeleteAsync(externalId.ToString(), profile.AccessToken, profile.AccessTokenSecret);

			await _sweepRulesRepository.DeleteAsync(request.RuleUid);
		}
	}
}
