using hrvster.Models.Subscriptions;

namespace hrvster.Models.Extensions
{
	public static class PlanExtensions
	{
		public static int GetHashtagCount(this Plan plan)
		{
			return plan.HashtagsPerMonth == 0 ? int.MaxValue : plan.HashtagsPerMonth;
		}
	}
}
