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

		public UpdateSweepstake(ILoggerFactory loggerFactory, ISweepsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpdateSweepstake>();
			_repository = repository;
		}

		protected override async Task Handle(UpdateSweepstakeRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Updating sweep with uid: {sweepUid} with the request {request}",
				request.SweepUid,
				request.ToJsonString());

			await _repository.UpdateAsync(request.SweepUid, new Models.Sweep
			{
				Name = request.Name,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				DrawDate = request.DrawDate
			});
		}
	}
}
