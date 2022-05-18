using hrvster.Models.WebModels.Sweeps;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class GetSweepDrawRuleRequest : IRequest<DrawRule>
	{
		private Guid drawRuleUid;

		public GetSweepDrawRuleRequest(Guid drawRuleUid)
		{
			this.drawRuleUid = drawRuleUid;
		}

		public Guid DrawRuleUid
		{
			get { return drawRuleUid; }
		}
	}
}
