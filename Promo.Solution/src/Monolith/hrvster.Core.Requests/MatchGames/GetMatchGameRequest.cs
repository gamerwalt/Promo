using hrvster.Games.Match.Models;
using MediatR;
using System;
using System.Threading;

namespace hrvster.Core.Requests.MatchGames
{
	public class GetMatchGameRequest : IRequest<MatchGame>
	{
		private Guid _matchGameUid;

		public GetMatchGameRequest(Guid matchGameUid)
		{
			_matchGameUid = matchGameUid;
		}

		public Guid MatchGameUid { get { return _matchGameUid; } }
	}
}
