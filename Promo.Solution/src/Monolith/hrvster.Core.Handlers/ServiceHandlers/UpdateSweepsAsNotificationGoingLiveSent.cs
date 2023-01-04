using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class UpdateSweepsAsNotificationGoingLiveSent : AsyncRequestHandler<UpdateSweepsAsNotificationGoingLiveSentRequest>
	{
		private readonly ISweepsRepository _repository;

		public UpdateSweepsAsNotificationGoingLiveSent(ISweepsRepository repository)
		{
			_repository = repository;
		}

		protected override async Task Handle(UpdateSweepsAsNotificationGoingLiveSentRequest request, CancellationToken cancellationToken)
			=> await _repository.UpdateSweepsAsNotificationGoingLiveAsync(request.SweepUid, true, cancellationToken);

	}
}
