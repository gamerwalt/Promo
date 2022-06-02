using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Profiles
{
	public class UpsertProfileRequest : IRequest
	{
		private Guid userUid;
		private string profileType;
		private string name;
		private string screenName;
		private string description;
		private string accessToken;
		private string accessTokenSecret;
		private string location;
		private string profileImageUrl;

		public UpsertProfileRequest(Guid userUid, string profileType, string name, string screenName, string description, string accessToken, string accessTokenSecret, string location, string profileImageUrl)
		{
			this.userUid = userUid;
			this.profileType = profileType;
			this.name = name;
			this.screenName = screenName;
			this.description = description;
			this.accessToken = accessToken;
			this.accessTokenSecret = accessTokenSecret;
			this.location = location;
			this.profileImageUrl = profileImageUrl;
		}

		public Guid UserUid
		{
			get { return userUid; }
		}

		public string ProfileType
		{
			get { return profileType; }
		}

		public string Name
		{
			get { return name; }
		}

		public string ScreenName
		{
			get { return screenName; }
		}

		public string Description
		{
			get { return description; }
		}

		public string AccessToken
		{
			get { return accessToken; }
		}

		public string AccessTokenSecret
		{
			get { return accessTokenSecret; }
		}

		public string Location
		{
			get { return location; }
		}

		public string ProfileImageUrl
		{
			get { return profileImageUrl; }
		}
	}
}
