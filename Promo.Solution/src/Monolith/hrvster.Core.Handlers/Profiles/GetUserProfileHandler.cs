using hrvster.Core.Requests;
using hrvster.Infrastructure.Accounts;
using hrvster.Models.Profiles;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Profiles
{
	public class GetUserProfileHandler : IRequestHandler<GetUserProfile, Profile>
	{
		private readonly ILogger<GetUserProfileHandler> _logger;
		private readonly IProfilesRepository _repository;

		public GetUserProfileHandler(
			ILoggerFactory loggerFactory, 
			IProfilesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetUserProfileHandler>();
			_repository = repository;
		}

		public async Task<Profile> Handle(GetUserProfile request, CancellationToken cancellationToken)
		{
			return await _repository.GetProfileAsync(request.UserUid, request.ProfileType);
		}
	}
}
