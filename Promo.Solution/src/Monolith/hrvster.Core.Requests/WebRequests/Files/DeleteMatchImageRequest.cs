using hrvster.Games.Match.Models;
using MediatR;

namespace hrvster.Core.Requests.WebRequests.Files
{
	public class DeleteMatchImageRequest : IRequest
	{
		private MatchImage _matchImage;
		private MatchGame _matchGame;

		public DeleteMatchImageRequest(MatchImage matchImage, MatchGame matchGame)
		{
			_matchImage = matchImage;
			_matchGame = matchGame;
		}

		public MatchImage MatchImage { get { return _matchImage; } }
		public MatchGame MatchGame { get { return _matchGame; } }
	}
}
