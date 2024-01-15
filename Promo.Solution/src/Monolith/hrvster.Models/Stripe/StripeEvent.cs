using System;

namespace hrvster.Models.Stripe
{
	public class StripeEvent
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public string RawEvent { get; set; }
		public StripeEventStatus Status { get; set; }
		public string SignatureHeader { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}
