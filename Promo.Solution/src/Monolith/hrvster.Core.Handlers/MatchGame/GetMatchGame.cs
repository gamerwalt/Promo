using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Requests.MatchGames;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.MatchGame
{
	public class GetMatchGame : IRequestHandler<GetMatchGameRequest, Games.Match.Models.MatchGame>
	{
		private readonly ILogger<GetMatchGame> _logger;
		private readonly IMatchGamesService _service;

		public GetMatchGame(ILoggerFactory loggerFactory, IMatchGamesService service)
		{
			_logger = loggerFactory.CreateLogger<GetMatchGame>();
			_service = service;
		}

		public Task<Games.Match.Models.MatchGame> Handle(GetMatchGameRequest request, CancellationToken cancellationToken)
			=> _service.GetMatchGameAsync(request.MatchGameUid, cancellationToken);
	}
}
