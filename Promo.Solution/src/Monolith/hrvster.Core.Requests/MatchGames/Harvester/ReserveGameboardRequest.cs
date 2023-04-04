using MediatR;
using System;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.MatchGames.Harvester
{
	public class ReserveGameboardRequest : IRequest<Guid>
	{
		private Guid _gameUid;
		private ITweet _tweet;

		public ReserveGameboardRequest(Guid uid, ITweet tweet)
		{
			_gameUid = uid;
			_tweet = tweet;
		}

		public Guid GameUid { get { return _gameUid; } }
		public ITweet Tweet { get { return _tweet; } }
	}
}
