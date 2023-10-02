namespace hrvster.Models.Options
{
	public class PaymentPendingOptions
	{
		public static string SectionName = "PaymentPendingSettings";

		public int NumberOfHoursBeforeRollback { get; set; } = 2;
	}
}
