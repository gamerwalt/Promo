using Firebase.Storage;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests.Files;
using hrvster.Core.Storage;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Files
{
	public class UploadMatchImages : IRequestHandler<UploadMatchImagesRequest, IEnumerable<UploadedImage>>
	{
		private readonly ILogger<UploadMatchImages> _logger;
		private readonly ISessionManager _sessionManager;

		private HrvsterUser _hrvsterUser;

		public UploadMatchImages(
			ILoggerFactory loggerFactory,
			ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<UploadMatchImages>();
			_sessionManager = sessionManager;
		}

		public async Task<IEnumerable<UploadedImage>> Handle(UploadMatchImagesRequest request, CancellationToken cancellationToken)
		{
			var uploadedImages = new List<UploadedImage>();

			_logger.LogInformation("Uploading match image for the match game {gameName}", request.MatchGame.Name);

			_hrvsterUser = _sessionManager.GetUserInformation();

			var googleStorageFileName = string.Empty;

			foreach (var image in request.Images)
			{
				_logger.LogInformation("Saving the image {imageName} for game {gameName}",
					image.Name,
					request.MatchGame.Name);

				var filePath = GetFilePath(request.MatchGame.Uid);

				Directory.CreateDirectory(filePath);
				filePath = Path.Combine(filePath, image.FileName);

				using (var stream = File.Create(filePath))
				{
					await image.CopyToAsync(stream);
				}

				var firebaseStorage = new FirebaseStorage(StorageNames.MatchGameUploads, new FirebaseStorageOptions
				{
					AuthTokenAsyncFactory = () => Task.FromResult(_hrvsterUser.Token),
					ThrowOnCancel = true
				}).Child(request.MatchGame.Uid.ToString());

				using (var ms = new FileStream(filePath, FileMode.Open))
				{
					googleStorageFileName = await firebaseStorage.Child(image.FileName).PutAsync(ms);
				}

				uploadedImages.Add(new UploadedImage {  LocationUrl = googleStorageFileName, Name = image.FileName });

				_logger.LogInformation("Finished saving the image {imageName} for game {gameName} at {storageFileName}",
					image.Name,
					request.MatchGame.Name,
					googleStorageFileName);
			}

			return uploadedImages;
		}

		private string GetFilePath(Guid matchGameUid)
			=> Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "hrvster", matchGameUid.ToString());
	}
}
