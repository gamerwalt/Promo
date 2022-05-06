using hrvster.Core.Requests.ApiRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetSweepEntryReport : IRequest<SweepEntryReport>
	{
		private GetEntryRequest request;
		private Guid campaignUid;
		private Guid sweepUid;

		public GetSweepEntryReport(GetEntryRequest request, Guid campaignUid, Guid sweepUid)
		{
			this.request = request;
			this.campaignUid = campaignUid;
			this.sweepUid = sweepUid;
		}

		public Guid CampaignUid
		{
			get { return campaignUid; }
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}

		public string ScreenName
		{
			get { return request.ScreenName; }
		}

		public string UserIdentifierType
		{
			get { return request.UserIdentifierType; }
		}

		public string ExternalUserId
		{
			get { return request.ExternalUserId; }
		}
	}

	public class GetSweepEntryReportHandler : IRequestHandler<GetSweepEntryReport, SweepEntryReport>
	{
		private readonly IReportingRepository _repository;

		public GetSweepEntryReportHandler(IReportingRepository repository)
		{
			_repository = repository;
		}

		public async Task<SweepEntryReport> Handle(GetSweepEntryReport request, CancellationToken cancellationToken)
		{
			return await _repository.GetCampaignSweepUserReportAsync(
				request.ScreenName,
				request.UserIdentifierType,
				request.ExternalUserId,
				request.CampaignUid,
				request.SweepUid);
		}
	}
}
