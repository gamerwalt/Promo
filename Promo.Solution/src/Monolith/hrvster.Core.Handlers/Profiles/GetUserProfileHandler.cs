using hrvster.Core.Requests;
using hrvster.Models.Profiles;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Profiles
{
	public class GetUserProfileHandler : IRequestHandler<GetUserProfile, Profile>
	{
		public Task<Profile> Handle(GetUserProfile request, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}
	}
}
