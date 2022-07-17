using hrvster.Core.Requests.ServiceRequests;
using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class GetDrawProperties : IRequestHandler<GetDrawPropertiesRequest, SweepstakesDrawDetails>
	{
		private readonly ILogger<GetDrawProperties> _logger;

		public GetDrawProperties(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<GetDrawProperties>();
		}

		public async Task<SweepstakesDrawDetails> Handle(GetDrawPropertiesRequest request, CancellationToken cancellationToken)
		{
			var orderedEntries = request.Entries.OrderBy(c => c.SweepEntryId).ToList();
			var firstEntryId = orderedEntries.First().SweepEntryId;
			var lastEntryId = orderedEntries.Last().SweepEntryId;
			var missingBetween = GetMissingNumbers(orderedEntries, firstEntryId, lastEntryId);

			return new SweepstakesDrawDetails
			{
				FirstEntryId = firstEntryId,
				LastEntryId = lastEntryId,
				MissingBetween = missingBetween,
				SweepDrawPropertiesAndRule = request.SweepDrawPropertiesAndRule,
				SweepEntries = request.Entries.ToList()
			};
		}

		private List<int> GetMissingNumbers(List<SweepEntry> orderedEntries, int firstEntryId, int lastEntryId)
		{
			var missing = new List<int>();

			for (var i = firstEntryId; i < lastEntryId + 1; i++)
			{
				if (!orderedEntries.Any(c => c.SweepEntryId.Equals(i)))
					missing.Add(i);

			}

			return missing;
		}
	}
}
