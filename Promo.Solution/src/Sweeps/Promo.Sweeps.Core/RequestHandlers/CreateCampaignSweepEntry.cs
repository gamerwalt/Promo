using MediatR;
using Promo.Sweeps.Core.Requests;
using Promo.Sweeps.Infrastructure;
using Promo.Sweeps.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Core.RequestHandlers
{
    public class CreateCampaignSweepEntry : IRequest<IEnumerable<SweepEntry>>
    {
        private NewCampaignSweepEntry request;
        private Guid campaignUid;

        public CreateCampaignSweepEntry(NewCampaignSweepEntry request, Guid campaignUid)
        {
            this.request = request;
            this.campaignUid = campaignUid;
        }

        public NewCampaignSweepEntry Request
        {
            get { return request; }
        }

        public Guid CampaignUid
        {
            get { return campaignUid; }
        }
    }

    public class CreateCampaignSweepEntryHandler : IRequestHandler<CreateCampaignSweepEntry, IEnumerable<SweepEntry>>
    {
        private readonly ISweepsEntryRepository _repository;

        public CreateCampaignSweepEntryHandler(ISweepsEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SweepEntry>> Handle(CreateCampaignSweepEntry request, CancellationToken cancellationToken)
        {
            var listOfSweepEntry = new List<SweepEntry>();

            for (var i = 0; i < request.Request.EntryCount; i++)
            {
                var sweepEntryUid = Guid.NewGuid();

                await _repository.CreateCampaignSweepsEntryAsync(new CampaignSweepsEntry
                {
                    SweepEntryUid = sweepEntryUid,
					SweepUid = request.Request.SweepUid,
                    CampaignUid = request.CampaignUid,
                    UserIdentifier = request.Request.UserIdentifier,
                    UserIdentifierType = request.Request.UserIdentifierType,
                    AcceptedTerms = false
                });

                listOfSweepEntry.Add(await _repository.GetAsync(sweepEntryUid));
            }

            return listOfSweepEntry;
        }
    }
}
