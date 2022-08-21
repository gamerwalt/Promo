using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class CreateSweep : AsyncRequestHandler<CreateSweepRequest>
	{
		private readonly ILogger<CreateSweep> _logger;
		private readonly ISweepsRepository _repository;

		public CreateSweep(ILoggerFactory loggerFactory, ISweepsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateSweep>();
			_repository = repository;
		}

		protected override async Task Handle(CreateSweepRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling the action {className} for the request {request}.",
				nameof(CreateSweep),
				request.ToJsonString());

			await _repository.CreateAsync(new Models.Sweep
			{
				Account = new Models.Account
				{
					AccountUid = request.AccountUid
				},
				Name = request.Name,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				DrawDate = request.DrawDate,
				Campaign = new Models.Campaign
				{
					CampaignUid = request.CampaignUid
				}
			});
		}
	}
}
