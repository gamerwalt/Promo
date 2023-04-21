using hrvster.Core.Clients.Common;
using hrvster.Core.Clients.Publishers;
using hrvster.Core.Requests.MatchGames.Harvester;
using hrvster.Models;
using hrvster.Models.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.MatchGame
{
	public class SendTwitterLinkToPlayGame : AsyncRequestHandler<SendTwitterLinkToPlayGameRequest>
	{
		private readonly ILogger<SendTwitterLinkToPlayGame> _logger;
		private readonly Func<ExternalClientType, IExternalClient> _publisherResolver;
		private readonly HarvesterSettingsOptions _hrvsterSettings;

		private readonly IExternalClient _twitterPublisher;

		public SendTwitterLinkToPlayGame(
			ILoggerFactory loggerFactory,
			Func<ExternalClientType, IExternalClient> publisherResolver,
			HarvesterSettingsOptions hrvsterSettings)
		{
			_logger = loggerFactory.CreateLogger<SendTwitterLinkToPlayGame>();
			_publisherResolver = publisherResolver;
			_hrvsterSettings = hrvsterSettings;

			_twitterPublisher = _publisherResolver(ExternalClientType.Twitter);
		}


		protected override async Task Handle(SendTwitterLinkToPlayGameRequest request, CancellationToken cancellationToken)
		{
			var screenName = request.Tweet.ScreenName;
			var tweetId = request.Tweet.TweetId;
			var rulesLink = GenerateRuleLink(request.Rule);

			var tweetText = $"@{screenName} {DateTime.UtcNow:MMM dd, yyyy hh:mm:fffff} - You have been selected to play a Match Game. See next tweet for rules and play link for you alone.";

			var rulesText = $"Visit {rulesLink} for Rules";

			var playText = $"A gameboard has been reserved for you alone. Visit {request.Url} to play";

			await PublishTweetAsync(request, tweetText, playText, rulesText);
		}

		private async Task PublishTweetAsync(
			SendTwitterLinkToPlayGameRequest request,
			string tweetText,
			string playText,
			string rulesText)
		{
			var mainTweetId = await _twitterPublisher.PublishResponse(new Message
			{
				Text = tweetText,
				Profile = request.Profile
			}, request.Tweet.TweetId);

			var playTweetId = await _twitterPublisher.PublishResponse(new Message
			{
				Text = playText,
				Profile = request.Profile
			}, mainTweetId);

			var ruleTweetId = await _twitterPublisher.PublishResponse(new Message
			{
				Text = rulesText,
				Profile = request.Profile
			}, playTweetId);
		}

		private Uri GenerateRuleLink(Rule rule)
		{
			var url = _hrvsterSettings.HrvsterUrl;
			var uriString = $"{url}/{Constants.GameRulesPath}/{GameTypes.MatchGame}/NG/en/{rule.Game.Uid}";

			return new Uri(uriString);
		}
	}
}
