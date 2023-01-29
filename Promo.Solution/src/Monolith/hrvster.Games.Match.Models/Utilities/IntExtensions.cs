namespace hrvster.Games.Match.Models.Utilities
{
	public static class IntExtensions
	{
		public static bool IsPerfectSquare(this int integer)
		{
			if (integer >= 0)
			{
				var sr = Math.Sqrt(integer);

				return (sr * sr == integer);
			}

			return false;
		}
	}
}
