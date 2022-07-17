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
	}

	public class Randomizer : IRandomizer
	{
		public int GetRandomNumberFromSweepEntries(int min, int max, IEnumerable<int> missingBetween, IEnumerable<SweepEntry> sweepEntries)
		{
			if (min >= max) throw new ArgumentOutOfRangeException();

			var result = missingBetween.First();

			while (missingBetween.Contains(result) && !sweepEntries.Any(s => s.SweepEntryId.Equals(result)))
			{
				result = RandomNumberGenerator.GetInt32(min, max + 1);
			}

			return result;
		}
	}
}
