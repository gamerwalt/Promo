using System;

namespace hrvster.Models
{
	public class PaymentServiceModel
	{
		public int PaymentServiceId { get; set; }
		public Guid PaymentServiceUid { get; set; }
		public string PaymentServiceName { get; set; }
		public string PaymentServiceNamespace { get; set; }
		public bool EnabledIndicator { get; set; }
	}
}
