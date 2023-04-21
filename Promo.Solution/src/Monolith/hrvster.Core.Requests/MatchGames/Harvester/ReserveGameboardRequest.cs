using hrvster.Models;
using hrvster.Models.Twitter;
using MediatR;
using System;

namespace hrvster.Core.Requests.MatchGames.Harvester
{
	public class ReserveGameboardRequest : IRequest<Guid>
	{
		private Guid _gameUid;
		private Tweet _tweet;
		private UserIdentifierType _handle;

		public ReserveGameboardRequest(Guid uid, Tweet tweet, UserIdentifierType handle)
		{
			_gameUid = uid;
			_tweet = tweet;
			_handle = handle;
		}

		public Guid GameUid { get { return _gameUid; } }
		public Tweet Tweet { get { return _tweet; } }
		public UserIdentifierType Handle { get { return _handle; } }
	}
}
