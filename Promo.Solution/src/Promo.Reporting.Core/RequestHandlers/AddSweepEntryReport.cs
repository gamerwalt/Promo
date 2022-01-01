using MediatR;
using Promo.Reporting.Core.Notifications;
using Promo.Reporting.Core.Requests;
using Promo.Reporting.Infrastructure;
using Promo.Reporting.Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Reporting.Core.RequestHandlers
{
	public class AddSweepEntryReport : IRequest
	{
		private CreateSweepEntryReport request;

		public AddSweepEntryReport(CreateSweepEntryReport request)
		{
			this.request = request;
		}

		public CreateSweepEntryReport Request
		{
			get { return request; }
		}
	}

	public class AddSweepEntryReportHandler : AsyncRequestHandler<AddSweepEntryReport>
	{
		private readonly IReportingRepository _repository;
		private readonly IMediator _mediator;

		public AddSweepEntryReportHandler(IReportingRepository repository, IMediator mediator)
		{
			_repository = repository;
			_mediator = mediator;
		}

		protected async override Task Handle(AddSweepEntryReport request, CancellationToken cancellationToken)
		{
			await _repository.UpsertSweepEntryReportAsync(new SweepEntryReport
			{
				SweepEntryReportUid = Guid.NewGuid(),
				CampaignUid = request.Request.CampaignUid,
				SweepUid = request.Request.SweepUid,
				Entries = 1,
				ScreenName = request.Request.ScreenName,
				ExternalId = request.Request.ExternalId,
				ExternalUserId = request.Request.ExternalUserId,
				UserIdentifierType = request.Request.UserIdentifierType
			});

			await _mediator.Publish(new SweepEntryReportUpserted(request));
		}
	}
}
