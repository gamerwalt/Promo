using hrvster.Models;
using System;

namespace hrvster.Extensions.ModelExtensions
{
	public static class SweepExtensions
	{
		public static bool IsActive(this Sweep sweep)
		{
			return DateTime.UtcNow >= sweep.StartDate.ToUniversalTime() &&
				DateTime.UtcNow <= sweep.EndDate.ToUniversalTime();
		}

		public static bool WillBeActiveInTheFuture(this Sweep sweep)
		{
			return DateTime.UtcNow <= sweep.StartDate.ToUniversalTime();
		}
	}
}
