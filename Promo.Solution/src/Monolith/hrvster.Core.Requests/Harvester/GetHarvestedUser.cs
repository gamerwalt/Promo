using hrvster.Infrastructure.Harvester;
using hrvster.Models.Harvester;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.Harvester
{
	public class GetHarvestedUser : IRequest<HarvestedUser>
	{
		private string userIdentifier;

		public GetHarvestedUser(string userIdentifier)
		{
			this.userIdentifier = userIdentifier;
		}

		public string UserIdentifier
		{
			get { return userIdentifier; }
		}
	}

	public class GetHarvestedUserRequestHandler : IRequestHandler<GetHarvestedUser, HarvestedUser>
	{
		private readonly IHarvesterRepository _repository;

		public GetHarvestedUserRequestHandler(IHarvesterRepository repository)
		{
			_repository = repository;
		}

		public async Task<HarvestedUser> Handle(GetHarvestedUser request, CancellationToken cancellationToken)
		{
			return await _repository.GetHarvestedUserByUserIdentifierAsync(request.UserIdentifier);
		}
	}
}
