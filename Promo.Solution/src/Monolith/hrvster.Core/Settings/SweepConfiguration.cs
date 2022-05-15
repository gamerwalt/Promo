using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Settings
{
	public class SweepConfiguration
	{
		public const string SectionName = "SweepstakeConfigurationOptions";

		public int DefaultRuleEntryPerUser { get; set; } = 0;
	}
}
