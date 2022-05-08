using hrvster.Models.Profiles;
using MediatR;
using System;

namespace hrvster.Core.Requests
{
	public class GetUserProfile : IRequest<Profile>
	{
		private Guid userUid;

		public GetUserProfile(Guid userUid)
		{
			this.userUid = userUid;
		}

		public Guid UserUid { get { return userUid; } }
	}
}
