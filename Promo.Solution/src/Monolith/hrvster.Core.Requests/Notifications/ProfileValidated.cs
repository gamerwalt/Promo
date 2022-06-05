using hrvster.Core.Clients;
using hrvster.Models;
using MediatR;
using System;

namespace hrvster.Core.Requests.Notifications
{
	public class ProfileValidated : INotification
	{
		private UserProviderDetails userDetails;
		private Guid userUid;
		private ProfileType profileType;

		public ProfileValidated(UserProviderDetails userDetails, Guid userUid, ProfileType profileType)
		{
			this.userDetails = userDetails;
			this.userUid = userUid;
			this.profileType = profileType;
		}

		public UserProviderDetails UserDetails
		{
			get { return userDetails; }
		}

		public Guid UserUid
		{
			get { return userUid; }
		}

		public ProfileType ProfileType
		{
			get { return profileType; }
		}
	}
}
