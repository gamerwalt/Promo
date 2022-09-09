using hrvster.Core.Requests.Notifications.Publishing;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Harvester;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers
{
	public class BlacklistTwitterUserFromSweeps : INotificationHandler<SweepRuleTweetPublished>
	{
		private readonly ILogger<BlacklistTwitterUserFromSweeps> _logger;
		private readonly IBlacklistedSweepsUsersRepository _repository;
		private readonly IProfilesRepository _profileRepository;

		public BlacklistTwitterUserFromSweeps(
			ILoggerFactory loggerFactory,
			IBlacklistedSweepsUsersRepository repository,
			IProfilesRepository profilesRepository)
		{
			_logger = loggerFactory.CreateLogger<BlacklistTwitterUserFromSweeps>();
			_repository = repository;
			_profileRepository = profilesRepository;
		}

		public async Task Handle(SweepRuleTweetPublished notification, CancellationToken cancellationToken)
		{
			var profileUid = notification.Message.Profile.ProfileUid;
			var sweepUid = notification.Message.SweepUid;

			//get the externalUserId of the profile and add the sweeps_uid and tweet_user_id to the blacklisted table
			var profile = await _profileRepository.GetProfileAsync(profileUid, Models.ProfileType.Twitter);
			if (profile != null)
			{
				_logger.LogInformation("Updating the blacklisted list for {sweepUid} with {externalId} for the following notification {notification}",
					sweepUid,
					profile.ExternalProfileId,
					notification.ToJsonString());

				await _repository.CreateAsync(profile.ExternalProfileId, sweepUid);

				return;
			}

			_logger.LogWarning("Unable to locate the profile for {profileUid}. The process {className} cannot be handled. Notification: {notification}",
				profileUid,
				nameof(BlacklistTwitterUserFromSweeps),
				notification.ToJsonString());
		}
	}
}
