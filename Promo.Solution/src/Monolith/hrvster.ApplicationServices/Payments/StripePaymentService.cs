using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Payments
{
	public class StripePaymentService : IPaymentAdaptor
	{
		private readonly ILogger<StripePaymentService> _logger;

		public StripePaymentService(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<StripePaymentService>();
		}

		public Task<string> GetUserSubscriptionAsync(string emailAddress, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task UnsubscribeUserAsync(long id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
