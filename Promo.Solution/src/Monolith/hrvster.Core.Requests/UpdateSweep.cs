using hrvster.Core.Requests.ApiRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
    public class UpdateSweep : IRequest<Sweep>
    {
        private Guid sweepUid;
        private Sweep sweep;
        private UpdateSweepRequest request;
		private string _timeZoneId;


		public UpdateSweep(Guid sweepUid, Sweep sweep, UpdateSweepRequest request, string timeZoneId)
        {
            this.sweepUid = sweepUid;
            this.sweep = sweep;
            this.request = request;
			_timeZoneId = timeZoneId;
        }

		public string TimeZoneId { get { return _timeZoneId; } }

        public Guid SweepUid
        {
            get { return sweepUid; }
        }

        public Sweep Sweep
        {
            get { return sweep; }
        }

        public UpdateSweepRequest Request
        {
            get { return request; }
        }
    }

    public class UpdateSweepRequestHandler : IRequestHandler<UpdateSweep, Sweep>
    {
        private readonly ISweepsRepository _repository;

        public UpdateSweepRequestHandler(ISweepsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Sweep> Handle(UpdateSweep request, CancellationToken cancellationToken)
        {
            request.Sweep.Name = request.Request.Name;
            request.Sweep.StartDate = request.Request.StartDate;
            request.Sweep.EndDate = request.Request.EndDate;
            request.Sweep.DrawDate = request.Request.DrawDate;

            await _repository.UpdateAsync(request.SweepUid, request.Sweep, request.TimeZoneId);

            return await _repository.GetAsync(request.SweepUid);
        }
    }
}
