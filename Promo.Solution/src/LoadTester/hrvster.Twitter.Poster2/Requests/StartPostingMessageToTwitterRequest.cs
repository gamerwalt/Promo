using hrvster.Core;
using hrvster.Core.Clients.Publishers;
using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using hrvster.Models.Options;
using MediatR;
using System.Diagnostics;

namespace hrvster.Twitter.Poster2.Requests
{
	public class StartPostingMessageToTwitterRequest : IRequest
	{
	}

	public class StartPostingMessageToTwitter : AsyncRequestHandler<StartPostingMessageToTwitterRequest>
	{
		private readonly ILogger<StartPostingMessageToTwitter> _logger;
		private readonly PosterOptions _posterOptions;
		private readonly IProfilesRepository _profilesRepository;
		private readonly Func<ExternalClientType, IExternalClient> _publisherResolver;
		private readonly IExternalClient _twitterPublisher;
		private readonly IRandomizer _randomizer;

		public StartPostingMessageToTwitter(
			ILoggerFactory loggerFactory,
			PosterOptions posterOptions,
			IProfilesRepository profilesRepository,
			Func<ExternalClientType, IExternalClient> publisherResolver,
			IRandomizer randomizer)
		{
			_logger = loggerFactory.CreateLogger<StartPostingMessageToTwitter>();
			_posterOptions = posterOptions;
			_profilesRepository = profilesRepository;
			_publisherResolver = publisherResolver;
			_randomizer = randomizer;

			_twitterPublisher = _publisherResolver(ExternalClientType.Twitter);
		}

		protected override async Task Handle(StartPostingMessageToTwitterRequest request, CancellationToken cancellationToken)
		{
			var profile = await _profilesRepository.GetProfileAsync(_posterOptions.AccountUidToLogonWith, ProfileType.Twitter);

			var messages = _posterOptions.Messages.Split(',').ToList();

			var previousTime = DateTime.Now;
			var elapsed = 0;
			var timer = new Stopwatch();
			timer.Start();

			while (elapsed < _posterOptions.MaxMinutesToContinuePosting)
			{
				var randomNumber = _randomizer.GetRandomNumber(1, messages.Count);

				var message = messages[randomNumber];

				var text = $"{message.Replace("[hashtag]", _posterOptions.Hashtag).Replace("[screenName]", _posterOptions.ProfileNameToReplyTo).Replace("[ticks]", DateTime.Now.Ticks.ToString())}";

				if (profile != null)
				{
					await _twitterPublisher.PublishResponse(new Core.Clients.Common.Message
					{
						Profile = profile,
						Text = text
					},
					_posterOptions.TweetIdToReplyTo);
				}

				var totalMinutesToWait = _randomizer.GetRandomNumber(
						_posterOptions.MinutesApart,
						_posterOptions.MinutesApart + _posterOptions.MinutesApart);

				_logger.LogInformation("Will wait now for {minutes} minutes.", totalMinutesToWait);

				await Task.Delay((int)TimeSpan.FromMinutes(totalMinutesToWait).TotalMilliseconds);

				elapsed = (int)timer.Elapsed.TotalMinutes;

				_logger.LogInformation("Elapsed time is {elapsedTime}", elapsed);
			}

			timer.Stop();
		}
	}
}
