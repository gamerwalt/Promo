using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class CreateSweepEntryRuleRequest : IRequest
	{
		public Guid SweepUid { get; set; }
		public int SweepId { get; set; }
		public int EntryPerUser { get; set; }
	}
}
