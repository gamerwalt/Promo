using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class UpdateSweepDrawRuleRequest : IRequest
	{
		private int sweepId;
		private Guid sweepUid;
		private Guid drawRuleUid;
		private int winnerCount;
		private int winnerBackupCount;

		public UpdateSweepDrawRuleRequest(
			int sweepId, 
			Guid sweepUid, 
			Guid drawRuleUid, 
			int winnerCount, 
			int winnerBackupCount)
		{
			this.sweepId = sweepId;
			this.sweepUid = sweepUid;
			this.drawRuleUid = drawRuleUid;
			this.winnerCount = winnerCount;
			this.winnerBackupCount = winnerBackupCount;
		}

		public int SweepId
		{
			get { return sweepId; }
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}

		public Guid DrawRuleUid
		{
			get { return drawRuleUid; }
		}

		public int WinnerCount
		{
			get { return winnerCount; }
		}

		public int WinnerBackupCount
		{
			get { return winnerBackupCount; }
		}
	}
}
