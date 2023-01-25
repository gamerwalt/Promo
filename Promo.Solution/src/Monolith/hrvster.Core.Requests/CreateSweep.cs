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
		private bool _isDaylight;

        public CreateSweep(NewSweepRequest request, string timezoneId, bool isDaylight)
        {
            this.request = request;
			_timezoneId= timezoneId;
			_isDaylight = isDaylight;
        }

        public NewSweepRequest Sweep
        {
            get { return request; }
        }
		public string TimeZoneId { get { return _timezoneId; } }
		public bool IsDaylight { get { return _isDaylight; } }

	}
}
