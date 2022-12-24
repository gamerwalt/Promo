using hrvster.Models;
using System;

namespace hrvster.Extensions.ModelExtensions
{
	public static class SweepExtensions
	{
		public static bool IsActive(this Sweep sweep)
		{
			return DateTime.UtcNow >= sweep.StartDateUtc &&
				DateTime.UtcNow <= sweep.EndDateUtc;
		}

		public static bool WillBeActiveInTheFuture(this Sweep sweep)
		{
			return DateTime.UtcNow <= sweep.StartDateUtc;
		}
	}
}
