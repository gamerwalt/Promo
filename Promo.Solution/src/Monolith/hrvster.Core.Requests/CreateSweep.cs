using hrvster.Core.Requests.ApiRequests;
using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
    public class CreateSweep : IRequest<Sweep>
    {
        private NewSweepRequest request;
		private string _timezoneId;

        public CreateSweep(NewSweepRequest request, string timezoneId)
        {
            this.request = request;
			_timezoneId= timezoneId;
        }

        public NewSweepRequest Sweep
        {
            get { return request; }
        }
		public string TimeZoneId { get { return _timezoneId; } }

	}

    public class CreateSweepRequestHandler : IRequestHandler<CreateSweep, Sweep>
    {
		private readonly ILogger<CreateSweepRequestHandler> _logger;
		private readonly IMediator _mediator;
        private readonly ISweepsRepository _repository;

        public CreateSweepRequestHandler(ILoggerFactory loggerFactory, IMediator mediator, ISweepsRepository repository)
        {
			_logger = loggerFactory.CreateLogger<CreateSweepRequestHandler>();
			_mediator = mediator;
            _repository = repository;
        }

        public async Task<Sweep> Handle(CreateSweep request, CancellationToken cancellationToken)
        {
			_logger.LogInformation("{action}... Creating sweep {sweepName} for AccountUid: {accountUid} under campaign {campaignUid}",
				nameof(CreateSweepRequestHandler),
				request.Sweep.Name,
				request.Sweep.AccountUid,
				request.Sweep.CampaignUid);

            var sweepUid = Guid.NewGuid();
            await _repository.CreateAsync(new Sweep
            {
                SweepUid = sweepUid,
                Campaign = new Campaign
				{
					CampaignUid = request.Sweep.CampaignUid
				},
				Account = new Account
				{
					AccountUid = request.Sweep.AccountUid
				},
                Name = request.Sweep.Name,
                StartDate = request.Sweep.StartDate,
                EndDate = request.Sweep.EndDate,
				DrawDate = request.Sweep.DrawDate
            });

			var sweep = await _repository.GetAsync(sweepUid);

			await _mediator.Publish(new SweepCreated(sweep));

			return sweep;
        }
    }
}
