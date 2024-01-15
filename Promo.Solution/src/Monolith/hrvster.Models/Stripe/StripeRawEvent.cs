using System;

namespace hrvster.Models.Stripe
{
	public class StripeRawEvent
	{
		public Guid Uid { get; set; }
		public string ExternalId { get; set; }
		public string ObjectType { get; set; }
		public string DataObjectType { get; set; }
		public string Currency { get; set; }
		public DateTime CurrentPeriodStart { get; set; }
		public DateTime CurrentPeriodEnd { get; set; }
		public DateTime StartDate { get; set; }
		public string ExternalCustomerId { get; set; }
		public string ExternalPlanId { get; set; }
		public string ExternalProductId { get; set; }
		public decimal? Amount { get; set; }
		public string ExternalInvoiceId { get; set; }
		public string ExternalStatus { get; set; }
		public string CustomerEmail { get; set; }
	}
}
