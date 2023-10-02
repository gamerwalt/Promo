using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class SendAccountSubscriptionUpdatedEmailNotificationToUserRequest : IRequest
	{
		private readonly Account _account;
		private readonly SubscriptionStatus _status;
		private readonly SubscriptionStatus _newStatus;
		private readonly int _pendingHours;

		public SendAccountSubscriptionUpdatedEmailNotificationToUserRequest(
			Account account,
			SubscriptionStatus status,
			SubscriptionStatus newStatus,
			int pendingHours)
		{
			_account = account;
			_status = status;
			_newStatus = newStatus;
			_pendingHours = pendingHours;
		}

		public Account Account { get { return _account; } }
		public SubscriptionStatus Status { get { return _status; } }
		public SubscriptionStatus NewStatus { get { return _newStatus; } }
		public int PendingHours { get { return _pendingHours; } }
	}
}
