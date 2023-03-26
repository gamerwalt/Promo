using hrvster.Core.Requests.Rules.Notifications;
using hrvster.Infrastructure.Harvester;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Rules.NotificationHandlers
{
	public class BlacklistUserFromGame : INotificationHandler<RulePublished>
	{
		private readonly ILogger<BlacklistTwitterUserFromSweeps> _logger;
		private readonly IBlacklistedSweepsUsersRepository _repository;
		private readonly IBlacklistedUserRepository _blacklistedUserRepository;

		public BlacklistUserFromGame(
			ILoggerFactory loggerFactory,
			IBlacklistedSweepsUsersRepository repository,
			IBlacklistedUserRepository blacklistedUserRepository)
		{
			_logger = loggerFactory.CreateLogger<BlacklistTwitterUserFromSweeps>();
			_repository = repository;
			_blacklistedUserRepository = blacklistedUserRepository;
		}

		public async Task Handle(RulePublished notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Rule published. Started handling Blacklist user from game with payload {payLoad}", notification.ToJsonString());

			//Todo: Once we switch to this method of blacklisting user when we redo the sweeps
			//		We won't need this checking of the game type
			switch (notification.GameType)
			{
				case Models.GameTypesEnum.Sweeps:
					await _repository.CreateAsync(notification.Profile.ExternalProfileId, notification.GameUid);
					break;

				case Models.GameTypesEnum.MatchGames:
					await CreateBlacklistedUserAsync(notification);
					break;
			}
		}

		private async Task CreateBlacklistedUserAsync(RulePublished notification)
		{
			var blacklistedUsers = await _blacklistedUserRepository.GetBlacklistedUsersAsync(notification.GameUid);

			if(blacklistedUsers != null && 
				blacklistedUsers.Any(b => b.UserId == notification.Profile.ExternalProfileId))
			{
				_logger.LogInformation("Will not save blacklisted user for the game with uid {gameUid} and with hashtag {hashTag} since the user already exists for this game in the blacklisted user list.",
					notification.GameUid,
					notification.Message.Hashtag);

				return;
			}

			await _blacklistedUserRepository.CreateAsync(notification.Profile.ExternalProfileId, notification.GameUid);
		}
	}
}
