using hrvster.Games.Match.Models.Rules;
using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests
{
	public class GetMatchGameAndPrizesRequest : IRequest<MatchRulesAndPrizes>
	{
		private readonly Guid _gameUid;

		public GetMatchGameAndPrizesRequest(Guid gameUid)
		{
			_gameUid = gameUid;
		}

		public Guid GameUid
		{
			get { return _gameUid; }
		}
	}
}
