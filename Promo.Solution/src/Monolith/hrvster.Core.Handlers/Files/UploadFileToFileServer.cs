using hrvster.Core.Requests.WebRequests.Files;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using hrvster.Core.Storage;
using Firebase.Storage;
using System.IO;

namespace hrvster.Core.Handlers.Files
{
	public class UploadFileToFileServer : IRequestHandler<UploadFileToFileServerRequest, IEnumerable<string>>
	{
		private readonly ILogger<UploadFileToFileServer> _logger;
		private readonly IFileService _fileService;
		private readonly FirebaseAuthProvider _firebaseAuthProvider;

		public UploadFileToFileServer(
			ILoggerFactory loggerFactory, 
			IFileService fileService,
			FirebaseAuthProvider firebaseAuthProvider)
		{
			_logger = loggerFactory.CreateLogger<UploadFileToFileServer>();
			_fileService = fileService;
			_firebaseAuthProvider = firebaseAuthProvider;
		}

		public async Task<IEnumerable<string>> Handle(UploadFileToFileServerRequest request, CancellationToken cancellationToken)
		{
			var firebaseStorage = new FirebaseStorage(StorageNames.TweetUploads, new FirebaseStorageOptions
			{
				AuthTokenAsyncFactory = () => Task.FromResult(request.SignInToken),
				ThrowOnCancel = true
			})
			.Child(request.UserUid.ToString());

			var listOfPaths = new List<string>();
			var googleStorageFileName = string.Empty;

			foreach (var media in request.Media)
			{
				var filePath = GetFilePath(request.UserUid, request.ProfileType.ToString());
				
				Directory.CreateDirectory(filePath);
				filePath = Path.Combine(filePath, media.FileName);

				using (var stream = File.Create(filePath))
				{
					await media.CopyToAsync(stream);
				}

				using (var ms = new FileStream(filePath, FileMode.Open))
				{
					googleStorageFileName = await firebaseStorage.Child(media.FileName).PutAsync(ms);
				}

				listOfPaths.Add(googleStorageFileName);
			}

			_logger.LogInformation("Media saved succecssfully.");

			return listOfPaths;
		}

		private string GetFilePath(Guid userUid, string addToPath)
		{
			//return Path.Combine(_fileServerOptions.Path, userUid.ToString(), addToPath);
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "hrvster", userUid.ToString(), addToPath);
		}
	}
}
