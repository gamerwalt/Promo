using System;

namespace hrvster.Models.Subscriptions.Flutterwave
{
	public class FlutterwavePlan
	{
		public int FlutterwavePlanId { get; set; }
		public Guid FlutterwavePlanUid { get; set; }
		public Uri ExternalMonthlyPaymentLink { get; set; }
		public Uri ExternalYearlyPaymentLink { get; set; }
	}
}
