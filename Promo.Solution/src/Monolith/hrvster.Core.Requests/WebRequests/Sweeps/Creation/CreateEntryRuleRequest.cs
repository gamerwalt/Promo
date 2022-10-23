using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.WebRequests.Sweeps.Creation
{
	public class CreateEntryRuleRequest : IRequest
	{
		private Sweep _sweep;
		private int _entryPerUser;

		public CreateEntryRuleRequest(Sweep sweep, int entryPerUser)
		{
			_sweep = sweep;
			_entryPerUser = entryPerUser;
		}

		public Sweep Sweep { get { return _sweep; } }
		public int EntryPerUser { get { return _entryPerUser; } }
	}
}
