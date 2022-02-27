using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetSweepsForCampaignUid : IRequest<IEnumerable<Sweep>>
	{
		private Guid campaignUid;
		private Guid accountVerifierUid;

		public GetSweepsForCampaignUid(Guid campaignUid, Guid accountVerfierUid)
		{
			this.campaignUid = campaignUid;
			this.accountVerifierUid = accountVerfierUid;
		}

		public Guid CampaignUid
		{
			get { return campaignUid; }
		}

		public Guid AccountVerifierUid
		{
			get { return accountVerifierUid; }
		}
	}

	public class GetSweepsForCampaignUidHandler : IRequestHandler<GetSweepsForCampaignUid, IEnumerable<Sweep>>
	{
		private readonly ISweepsRepository _repository;

		public GetSweepsForCampaignUidHandler(ISweepsRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<Sweep>> Handle(GetSweepsForCampaignUid request, CancellationToken cancellationToken)
		{
			return await _repository.GetAllForCampaignAsync(request.CampaignUid);
		}
	}
}
