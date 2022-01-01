using MediatR;
using Promo.Reporting.Core.Requests;
using Promo.Reporting.Infrastructure;
using Promo.Reporting.Infrastructure.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Reporting.Core.RequestHandlers
{
	public class AddSweepEntryReport : IRequest<SweepEntryReport>
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

	public class AddSweepEntryReportHandler : IRequestHandler<AddSweepEntryReport, SweepEntryReport>
	{
		private readonly IReportingRepository _repository;
		private readonly IMediator _mediator;

		public AddSweepEntryReportHandler(IReportingRepository repository, IMediator mediator)
		{
			_repository = repository;
			_mediator = mediator;
		}

		public async Task<SweepEntryReport> Handle(AddSweepEntryReport request, CancellationToken cancellationToken)
		{
			await _repository.DoSomething();

			return new SweepEntryReport();
		}
	}
}
