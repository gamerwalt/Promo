using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.WebModels.Sweeps;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Validators.Sweepstakes
{
	public interface ISweepRuleUpdateValidator
	{
		Task<ValidatorStatus> CanUpdateAsync(Sweep sweep, SweepEntryRule sweepEntryRule);
	}

	public class SweepRuleUpdateValidator : ISweepRuleUpdateValidator
	{
		private readonly ILogger<SweepRuleUpdateValidator> _logger;
		private readonly ISweepsEntriesRepository _sweepEntriesRepository;

		public SweepRuleUpdateValidator(
			ILoggerFactory loggerFactory,
			ISweepsEntriesRepository sweepsEntriesRepository)
		{
			_logger = loggerFactory.CreateLogger<SweepRuleUpdateValidator>();
			_sweepEntriesRepository = sweepsEntriesRepository;
		}

		public async Task<ValidatorStatus> CanUpdateAsync(Sweep sweep, SweepEntryRule sweepEntryRule)
		{
			if (IsLive(sweep))
			{
				return ValidatorStatus.CreateFailureStatus("Sweepstake is currently live.");
			}

			if(await HasEntries(sweep))
			{
				return ValidatorStatus.CreateFailureStatus("Sweepstake already has entries.");
			}

			return ValidatorStatus.CreateSuccessStatus();
		}

		private async Task<bool> HasEntries(Sweep sweep)
		{
			var entries = await _sweepEntriesRepository.GetSweepEntriesAsync(sweep.SweepUid);

			return entries.Any();
		}

		private static bool IsLive(Sweep sweep)
		{
			return DateTime.UtcNow <= sweep.StartDateUtc &&
							DateTime.UtcNow >= sweep.EndDateUtc;
		}
	}
}
