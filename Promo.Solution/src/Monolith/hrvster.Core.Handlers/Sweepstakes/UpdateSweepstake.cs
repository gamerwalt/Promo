using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class UpdateSweepstake : AsyncRequestHandler<UpdateSweepstakeRequest>
	{
		private readonly ILogger<UpdateSweepstake> _logger;
		private readonly ISweepsRepository _repository;
		private readonly IMediator _mediator;

		public UpdateSweepstake(
			ILoggerFactory loggerFactory,
			ISweepsRepository repository,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<UpdateSweepstake>();
			_repository = repository;
			_mediator = mediator;
		}

		protected override async Task Handle(UpdateSweepstakeRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Updating sweep with uid: {sweepUid} with the request {request}",
				request.SweepUid,
				request.ToJsonString());

			request.Sweepstake.Name = request.Name;
			request.Sweepstake.StartDate = request.StartDate;
			request.Sweepstake.EndDate = request.EndDate;
			request.Sweepstake.DrawDate = request.DrawDate;

			await _repository.UpdateAsync(request.SweepUid, new Models.Sweep
			{
				Name = request.Name,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				DrawDate = request.DrawDate
			});

			await _mediator.Publish(new SweepstakeUpdated(request.Sweepstake));
		}
	}
}
