using MediatR;
using Stripe;
using System.Threading;

namespace hrvster.Core.Requests.WebRequests.Stripe
{
	public class ProcessStripeEventRequest : IRequest
	{
		private Event _stripeEvent;
		private CancellationToken _cancellationToken;

		public ProcessStripeEventRequest(Event stripeEvent, CancellationToken cancellationToken)
		{
			_stripeEvent = stripeEvent;
			_cancellationToken = cancellationToken;
		}

		public Event StripeEvent { get { return _stripeEvent; } }
		public CancellationToken CancellationToken { get { return _cancellationToken; } }
	}
}
