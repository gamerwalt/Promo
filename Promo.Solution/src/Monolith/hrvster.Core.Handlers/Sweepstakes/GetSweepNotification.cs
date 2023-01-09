using hrvster.Core.Requests.Sweepstakes;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetSweepNotification : IRequestHandler<GetSweepNotificationRequest, SweepNotification>
	{
		private readonly ISweepNotificationRepository _repository;

		public GetSweepNotification(ISweepNotificationRepository repository)
		{
			_repository = repository;
		}

		public async Task<SweepNotification> Handle(GetSweepNotificationRequest request, CancellationToken cancellationToken)
			=> await _repository.GetSweepNotificationAsync(request.Sweepstake.SweepId, cancellationToken);
	}
}
