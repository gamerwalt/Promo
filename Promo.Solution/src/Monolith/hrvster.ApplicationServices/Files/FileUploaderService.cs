using Firebase.Storage;
using hrvster.ApplicationServices.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Files
{
	public interface IFileUploaderService
	{
		Task<string> UploadToServiceAsync(IFormFile file, string token, string appendToName);
	}

	public class FileUploaderService : IFileUploaderService
	{
		private readonly ILogger<FileUploaderService> _logger;

		public FileUploaderService(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<FileUploaderService>();
		}

		public async Task<string> UploadToServiceAsync(IFormFile file, string token, string appendToName)
		{
			_logger.LogInformation("Started the upload to the file server");

			var firebaseStorage = new FirebaseStorage(GetStoragePath(appendToName), new FirebaseStorageOptions
			{
				AuthTokenAsyncFactory = () => Task.FromResult(token),
				ThrowOnCancel = true
			});

			var googleStorageFileName = string.Empty;

			_logger.LogInformation("Uploading the media {fileName}.", file.FileName);
			
			var filePath = GetFileName(file, Guid.NewGuid());

			using (var stream = File.Create(filePath))
			{
				await file.CopyToAsync(stream);
			}

			using (var ms = new FileStream(filePath, FileMode.Open))
			{
				googleStorageFileName = await firebaseStorage.Child(file.FileName).PutAsync(ms);
			}

			_logger.LogInformation("Finished uploading the media {fileName}.", file.FileName);

			return googleStorageFileName;
		}

		private string GetFileName(IFormFile file, Guid newUid)
		{
			var filePath = GetFilePath(newUid, string.Empty);

			Directory.CreateDirectory(filePath);

			return Path.Combine(filePath, file.FileName);
		}

		private static string GetStoragePath(string appendToName)
		{
			if (string.IsNullOrWhiteSpace(appendToName))
			{
				return $"{StorageNames.CompanyName}";
			}

			return $"{StorageNames.CompanyName}-{appendToName}";
		}

		private string GetFilePath(Guid userUid, string addToPath)
			=> Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "hrvster", userUid.ToString(), addToPath);
	}
}
