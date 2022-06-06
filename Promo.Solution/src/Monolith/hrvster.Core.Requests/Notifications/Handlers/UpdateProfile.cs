using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using hrvster.Models.Profiles;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.Notifications.Handlers
{
	public class UpdateProfile : INotificationHandler<ProfileValidated>
	{
		private readonly ILogger<UpdateProfile> _logger;
		private readonly IProfilesRepository _repository;

		public UpdateProfile(ILoggerFactory loggerFactory, IProfilesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpdateProfile>();
			_repository = repository;
		}

		public async Task Handle(ProfileValidated notification, CancellationToken cancellationToken)
		{
			var profile = await _repository.GetProfileAsync(notification.UserUid, notification.ProfileType);

			if (profile == null)
			{
				_logger.LogError("Unable to find a profile for user with Uid : {userUid}", notification.UserUid);
				_logger.LogInformation("Will attempt to create a new profile for the user with uid : {userUid}", notification.UserUid);
			}

			await _repository.UpsertProfileAsync(BuildProfile(notification));
		}

		private Profile BuildProfile(ProfileValidated model)
		{
			return new Profile
			{
				Account = new Account
				{
					AccountUid = model.UserUid
				},
				ExternalProfileId = model.UserDetails.ExternalProfileId,
				ProfileUid = model.UserUid,
				ProfileType = model.ProfileType,
				DisplayName = model.UserDetails.Name,
				ScreenName = model.UserDetails.ScreenName,
				Description = model.UserDetails.Description,
				AccessToken = model.UserDetails.AccessToken,
				AccessTokenSecret = model.UserDetails.AccessTokenSecret,
				Location = model.UserDetails.Location,
				ProfileImageUrl = model.UserDetails.ProfileImage
			};
		}
	}
}
