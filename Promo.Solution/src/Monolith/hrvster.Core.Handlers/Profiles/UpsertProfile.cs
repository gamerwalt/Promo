using hrvster.Core.Requests.WebRequests.Profiles;
using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using hrvster.Models.Profiles;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Profiles
{
	public class UpsertProfile : AsyncRequestHandler<UpsertProfileRequest>
	{
		private readonly ILogger<UpsertProfile> _logger;
		private readonly IProfilesRepository _repository;

		public UpsertProfile(ILoggerFactory loggerFactory, IProfilesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpsertProfile>();
			_repository = repository;
		}

		protected override async Task Handle(UpsertProfileRequest request, CancellationToken cancellationToken)
		{
			var profile = new Profile
			{
				ProfileUid = Guid.NewGuid(),
				Account = new Account
				{
					AccountUid = request.UserUid
				},
				ProfileType = request.ProfileType,
				DisplayName = request.Name,
				ScreenName = request.ScreenName,
				Description = request.Description,
				AccessToken = request.AccessToken,
				AccessTokenSecret = request.AccessTokenSecret,
				Location = request.Location,
				ProfileImageUrl = request.ProfileImageUrl
			};

			await _repository.UpsertProfileAsync(profile);
		}
	}
}
