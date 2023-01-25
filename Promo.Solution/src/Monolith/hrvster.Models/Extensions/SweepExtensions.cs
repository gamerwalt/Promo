using System;

namespace hrvster.Models.Extensions
{
	public static class SweepExtensions
	{
		public static bool IsActive(this Sweep sweep)
		{
			return DateTime.UtcNow >= sweep.StartDateUtc && DateTime.UtcNow <= sweep.EndDateUtc;
		}

		public static bool WillBeInProgress(this Sweep sweep)
		{
			return DateTime.UtcNow <= sweep.StartDateUtc;
		}
	}
}
