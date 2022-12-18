using hrvster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace hrvster.Core
{
	public interface IRandomizer
	{
		int GetRandomNumberFromSweepEntries(int firstEntryId, int lastEntryId, IEnumerable<int> missingBetween, IEnumerable<SweepEntry> sweepEntries);
		int GetRandomNumber(int min, int maxInclusive);
	}

	public class Randomizer : IRandomizer
	{
		public int GetRandomNumber(int min, int maxInclusive)
		{
			if (min >= maxInclusive) throw new ArgumentOutOfRangeException();

			return RandomNumberGenerator.GetInt32(min, maxInclusive);
		}

		public int GetRandomNumberFromSweepEntries(int min, int max, IEnumerable<int> missingBetween, IEnumerable<SweepEntry> sweepEntries)
		{
			if (min >= max) throw new ArgumentOutOfRangeException();

			var result = 0;

			if(missingBetween.Count() > 0)
			{
				result = missingBetween.First();

				while (missingBetween.Contains(result) && !sweepEntries.Any(s => s.SweepEntryId.Equals(result)))
				{
					result = RandomNumberGenerator.GetInt32(min, max + 1);
				}

				return result;
			}

			return RandomNumberGenerator.GetInt32(min, max + 1);
		}
	}
}
