using hrvster.Models;
using System.Collections.Generic;
using System.Linq;

namespace hrvster.Core
{
	public class PrizePositionFinder
	{
		public int GetNextPrizePosition(int winnerCount, IEnumerable<SweepPrize> prizes)
		{
			var prizePositions = Enumerable.Range(1, winnerCount).ToArray();
			foreach (var prize in prizes)
			{
				prizePositions[prize.PrizePosition - 1] = 0;
			}

			return prizePositions.FirstOrDefault(c => c != 0);
		}
	}
}
