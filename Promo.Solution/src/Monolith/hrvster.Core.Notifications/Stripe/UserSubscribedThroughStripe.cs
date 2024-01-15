using MediatR;

namespace hrvster.Core.Notifications.Stripe
{
	public class UserSubscribedThroughStripe : INotification
	{
		public string ExternalEventId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string CustomerId { get; set; }
		public string PlanId { get; set; }
		public string ProductId { get; set; }
		public string CustomerEmail { get; set; }
	}
}
