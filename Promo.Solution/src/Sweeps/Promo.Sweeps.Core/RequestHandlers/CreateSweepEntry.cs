using MediatR;
using Promo.Core;
using Promo.Core.Exceptions;
using Promo.Sweeps.Core.Requests;
using Promo.Sweeps.Infrastructure;
using Promo.Sweeps.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Core.RequestHandlers
{
    public class CreateSweepEntry : IRequest<IEnumerable<SweepEntry>>
    {
        private NewSweepEntryRequest request;
        private Guid sweepUid;

        public CreateSweepEntry(NewSweepEntryRequest request, Guid sweepUid)
        {
            this.request = request;
            this.sweepUid = sweepUid;
        }

        public NewSweepEntryRequest Entry
        {
            get { return request; }
        }

        public Guid SweepUid
        {
            get { return sweepUid; }
        }
    }

    public class CreateSweepEntryHandler : IRequestHandler<CreateSweepEntry, IEnumerable<SweepEntry>>
    {
        private readonly ISweepsEntriesRepository _repository;
        private readonly ISweepsRepository _sweepsRepository;

        private List<Guid> EntryGuids;

        public CreateSweepEntryHandler(ISweepsEntriesRepository repository, ISweepsRepository sweepsRepository)
        {
            _repository = repository;
            _sweepsRepository = sweepsRepository;
        }

        public async Task<IEnumerable<SweepEntry>> Handle(CreateSweepEntry request, CancellationToken cancellationToken)
        {
            var sweep = await _sweepsRepository.GetAsync(request.SweepUid);
            if (sweep == null)
                throw new Exception(ExceptionMessages.InvalidSweepsMessage);

            EntryGuids = new List<Guid>();

            if (request.Entry.EntryCount == 1)
            {
                await AddSweepEntry(sweep, request);
            }
            else if(request.Entry.EntryCount > 1)
            {
                for(var i = 0; i < request.Entry.EntryCount; i++)
                {
                    await AddSweepEntry(sweep, request);
                }
            }

            var sweepEntries = new List<SweepEntry>();
            foreach(var guid in EntryGuids)
            {
                sweepEntries.Add(await _repository.GetAsync(guid));
            }

            return sweepEntries;
        }

        private async Task AddSweepEntry(Sweep sweep, CreateSweepEntry request)
        {
            var sweepEntryGuid = Guid.NewGuid();

            var sweepEntry = new SweepEntry
            {
                SweepEntryUid = sweepEntryGuid,
                Sweep = sweep,
                AcceptedTerms = request.Entry.AcceptedTerms,
                UserIdentifier = request.Entry.UserIdentifier,
                UserIdentifierType = (UserIdentifierType)Enum.Parse(typeof(UserIdentifierType), request.Entry.UserIdentifierType)
            };

            await _repository.CreateAsync(sweepEntry);

            EntryGuids.Add(sweepEntryGuid);
        }
    }
}
