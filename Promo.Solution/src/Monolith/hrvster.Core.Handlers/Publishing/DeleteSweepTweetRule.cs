using hrvster.Core.Clients;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Facebook;
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
		private readonly IFacebookPageRepository _facebookPageRepository;
		private readonly Func<ExternalClientType, Clients.Publishers.IExternalClient> _publisherResolver;
		private readonly ISweepRulesPageRepository _sweepRulesPageRepository;
		private readonly IRebrandlyService _rebrandlyService;

		public DeleteSweepTweetRule(
			ILoggerFactory loggerFactory,
			ISweepRulesRepository sweepRulesRepository,
			IProfilesRepository profilesRepository,
			IFacebookPageRepository facebookPageRepository,
			Func<ExternalClientType, Clients.Publishers.IExternalClient> publisherResolver,
			ISweepRulesPageRepository sweepRulesPageRepository,
			IRebrandlyService rebrandlyService)
		{
			_logger = loggerFactory.CreateLogger<DeleteSweepTweetRule>();
			_sweepRulesRepository = sweepRulesRepository;
			_profilesRepository = profilesRepository;
			_facebookPageRepository = facebookPageRepository;
			_publisherResolver = publisherResolver;
			_sweepRulesPageRepository = sweepRulesPageRepository;
			_rebrandlyService = rebrandlyService;
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
			var profile = await _profilesRepository.GetProfileAsync(request.UserUid, GetProfileType(rule));
			if (profile == null)
			{
				_logger.LogError("Could not load the profile with AccountUid: {userUid}", request.UserUid);
				return;
			}

			var accessToken = profile.AccessToken;

			if (rule.ExternalClientType == ExternalClientType.Facebook)
			{
				var sweepRulesPage = await _sweepRulesPageRepository.GetByRuleIdAsync(rule.RuleId);

				if(sweepRulesPage == null)
				{
					_logger.LogError("Could not find the sweep rules page.");

					return;
				}

				var page = await _facebookPageRepository.GetPageAsync(sweepRulesPage.PageId);
				if (page == null)
				{
					_logger.LogError("Unable to load the page to delete properly.");
				}
				else
				{
					accessToken = page.PageAccessToken;
				}
			}

			var externalId = rule.ExternalId;
			var publisher = _publisherResolver(rule.ExternalClientType);

			//Delete Post/Tweet/Instagram/Tiktok
			await publisher.DeleteAsync(externalId.ToString(), accessToken, profile.AccessTokenSecret);

			//Delete the ShortUrl if any
			await _rebrandlyService.DeleteLinkAsync(rule.UrlLink);

			await _sweepRulesRepository.DeleteAsync(request.RuleUid);
		}

		private ProfileType GetProfileType(Rule rule)
		{
			if (rule.ExternalClientType == ExternalClientType.Twitter)
				return ProfileType.Twitter;
			else if (rule.ExternalClientType == ExternalClientType.Facebook)
				return ProfileType.Facebook;

			return ProfileType.Facebook;
		}
	}
}
