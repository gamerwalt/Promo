using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MediatR;
using System.Collections.Generic;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class GetDrawPropertiesRequest : IRequest<SweepstakesDrawDetails>
	{
		private SweepDrawPropertiesAndRule _sweepDrawPropertiesAndRule;
		private IEnumerable<SweepEntry> _entries;

		public GetDrawPropertiesRequest(SweepDrawPropertiesAndRule rule, IEnumerable<SweepEntry> entries)
		{
			_sweepDrawPropertiesAndRule = rule;
			_entries = entries;
		}

		public SweepDrawPropertiesAndRule SweepDrawPropertiesAndRule
		{
			get { return _sweepDrawPropertiesAndRule; }
		}

		public IEnumerable<SweepEntry> Entries { get { return _entries; } }
	}
}
