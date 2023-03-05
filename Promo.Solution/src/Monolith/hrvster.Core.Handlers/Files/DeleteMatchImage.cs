using Firebase.Storage;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests.Files;
using hrvster.Core.Storage;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Files
{
	public class DeleteMatchImage : AsyncRequestHandler<DeleteMatchImageRequest>
	{
		private readonly ILogger<DeleteMatchImage> _logger;
		private readonly ISessionManager _sessionManager;

		private HrvsterUser _hrvsterUser;

		public DeleteMatchImage(ILoggerFactory loggerFactory, ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<DeleteMatchImage>();
			_sessionManager = sessionManager;
		}

		protected override async Task Handle(DeleteMatchImageRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Deleting match image with Uid {matchImageUid} with a url {imageUrl} for the match game {gameName}",
				request.MatchImage.Uid,
				request.MatchImage.LocationUrl,
				request.MatchGame.Name);

			_hrvsterUser = _sessionManager.GetUserInformation();

			var firebaseStorage = new FirebaseStorage(StorageNames.MatchGameUploads, new FirebaseStorageOptions
			{
				AuthTokenAsyncFactory = () => Task.FromResult(_hrvsterUser.Token),
				ThrowOnCancel = true
			})
			.Child(request.MatchGame.Uid.ToString());

			await firebaseStorage.Child(request.MatchImage.Name).DeleteAsync();

			_logger.LogInformation("Finished deleting the match image with uid {matchImageUid}", request.MatchImage.Uid);
		}
	}
}
