using hrvster.Core.Clients.Common;
using hrvster.Models;
using hrvster.Models.Profiles;
using MediatR;
using System;

namespace hrvster.Core.Requests.Rules.Notifications
{
	public class RulePublished : INotification
	{
		private Profile _profile;
		private string _externalId;
		private Message _message;
		private bool _following;
		private int _gameId;
		private Guid _gameUid;
		private GameTypesEnum _gameType;

		public RulePublished(Profile profile, string externalId, Message message, bool following, int gameId, Guid gameUid, GameTypesEnum gameType)
		{
			_profile = profile;
			_externalId = externalId;
			_message = message;
			_following = following;
			_gameId = gameId;
			_gameUid = gameUid;
			_gameType = gameType;
		}

		public Profile Profile { get { return _profile; } }
		public string ExternalId { get { return _externalId; } }
		public Message Message { get { return _message; } }
		public bool MustBeFollowing { get { return _following; } }
		public int GameId { get { return _gameId; } }
		public Guid GameUid { get { return _gameUid; } }
		public GameTypesEnum GameType { get { return _gameType; } }
	}
}
