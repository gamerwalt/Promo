using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Games.Core.Notifications;
using Promo.Games.Core.Requests;
using Promo.Games.Infrastructure;
using Promo.Games.Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Games.Core.RequestHandlers
{
    public class CreateSweep : IRequest<Sweep>
    {
        private NewSweepRequest request;

        public CreateSweep(NewSweepRequest request)
        {
            this.request = request;
        }

        public NewSweepRequest Sweep
        {
            get { return request; }
        }
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
