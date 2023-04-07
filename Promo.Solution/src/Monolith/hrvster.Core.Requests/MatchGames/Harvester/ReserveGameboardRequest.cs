using hrvster.Models;
using MediatR;
using System;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.MatchGames.Harvester
{
	public class ReserveGameboardRequest : IRequest<Guid>
	{
		private Guid _gameUid;
		private ITweet _tweet;
		private UserIdentifierType _handle;

		public ReserveGameboardRequest(Guid uid, ITweet tweet, UserIdentifierType handle)
		{
			_gameUid = uid;
			_tweet = tweet;
			_handle = handle;
		}

		public Guid GameUid { get { return _gameUid; } }
		public ITweet Tweet { get { return _tweet; } }
		public UserIdentifierType Handle { get { return _handle; } }
	}
}
