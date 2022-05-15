using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class CreateSweepDrawRuleRequest : IRequest
	{
		public Guid SweepUid { get; set; }
		public int SweepId { get; set; }
		public int WinnerCount { get; set; }
		public int WinnerBackupCount { get; set; }
	}
}
