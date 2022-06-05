using hrvster.Models;
using hrvster.Models.Profiles;
using MediatR;
using System;

namespace hrvster.Core.Requests
{
	public class GetUserProfile : IRequest<Profile>
	{
		private Guid userUid;
		private ProfileType profileType;

		public GetUserProfile(Guid userUid, Models.ProfileType profileType)
		{
			this.userUid = userUid;
			this.profileType = profileType;
		}

		public Guid UserUid { get { return userUid; } }
		public ProfileType ProfileType { get { return profileType; } }
	}
}
