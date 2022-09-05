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
		private string _signInToken;

		public UploadFileToFileServerRequest(
			List<IFormFile> media, 
			Guid userUid, 
			ProfileType profileType,
			string signInToken)
		{
			_media = media;
			_userUid = userUid;
			_profileType = profileType;
			_signInToken = signInToken;
		}

		public string SignInToken { get { return _signInToken; } }

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
