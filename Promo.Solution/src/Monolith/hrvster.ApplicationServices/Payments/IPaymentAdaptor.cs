using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Payments
{
	public interface IPaymentAdaptor
	{
		Task<string> GetUserSubscriptionAsync(string emailAddress, CancellationToken cancellationToken);
		Task UnsubscribeUserAsync(long id, CancellationToken cancellationToken);
	}
}
