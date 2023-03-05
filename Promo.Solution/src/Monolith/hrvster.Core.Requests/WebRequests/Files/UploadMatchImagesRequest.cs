using hrvster.Games.Match.Models;
using hrvster.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace hrvster.Core.Requests.WebRequests.Files
{
	public class UploadMatchImagesRequest : IRequest<IEnumerable<UploadedImage>>
	{
		private MatchGame matchGame;
		private ICollection<IFormFile> images;

		public UploadMatchImagesRequest(MatchGame matchGame, ICollection<IFormFile> images)
		{
			this.matchGame = matchGame;
			this.images = images;
		}

		public MatchGame MatchGame { get { return matchGame; } }
		public ICollection<IFormFile> Images { get { return images; } }
	}
}
