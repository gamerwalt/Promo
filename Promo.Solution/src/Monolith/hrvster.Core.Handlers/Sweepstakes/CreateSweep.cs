using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class CreateSweep : AsyncRequestHandler<CreateSweepRequest>
	{
		private readonly ILogger<CreateSweep> _logger;
		private readonly ISweepsRepository _repository;
		private readonly IMediator _mediator;

		public CreateSweep(
			ILoggerFactory loggerFactory, 
			ISweepsRepository repository,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<CreateSweep>();
			_repository = repository;
			_mediator = mediator;
		}

		protected override async Task Handle(CreateSweepRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling the action {className} for the request {request}.",
				nameof(CreateSweep),
				request.ToJsonString());

			var sweepUid = Guid.NewGuid();

			var sweep = new Models.Sweep
			{
				SweepUid = sweepUid,
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
			};

			if(request.SweepUid.Equals(Guid.Empty))
			{
				await _repository.CreateAsync(sweep, request.TimeZoneId);
			}
			else
			{
				sweep.SweepUid = request.SweepUid;

				await _repository.CreateAsync(sweep, request.TimeZoneId);
			}

			await _mediator.Publish(new SweepCreated(sweep));
		}
	}
}
