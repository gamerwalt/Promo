using hrvster.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace hrvster.Core.Requests.WebRequests.Files
{
	public class UploadFileToFileServerRequest : IRequest<IEnumerable<string>>
	{
		private List<IFormFile> _media;
		private Guid _userUid;
		private ProfileType _profileType;

		public UploadFileToFileServerRequest(List<IFormFile> media, Guid userUid, ProfileType profileType)
		{
			_media = media;
			_userUid = userUid;
			_profileType = profileType;
		}

		public List<IFormFile> Media
		{
			get { return _media; }
		}

		public Guid UserUid
		{
			get { return _userUid; }
		}

		public ProfileType ProfileType
		{
			get { return _profileType; }
		}
	}
}
