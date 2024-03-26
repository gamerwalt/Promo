using Azure.Storage.Blobs;
using hrvster.ApplicationServices.Storage;
using hrvster.Models.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Files
{
	public interface IFileUploaderService
	{
		Task<string> UploadToServiceAsync(IFormFile file, string token, string companyName);
	}

	public class FileUploaderService : IFileUploaderService
	{
		private readonly ILogger<FileUploaderService> _logger;
		private readonly HarvesterSettingsOptions _harvesterSettings;

		public FileUploaderService(ILoggerFactory loggerFactory, HarvesterSettingsOptions harvesterSettings)
		{
			_logger = loggerFactory.CreateLogger<FileUploaderService>();
			_harvesterSettings = harvesterSettings;
		}

		public async Task<string> UploadToServiceAsync(IFormFile file, string token, string companyName)
		{
			var blobServiceClient = new BlobServiceClient(_harvesterSettings.HrvsterAzureStorageConnectionString);
			var containerClient = blobServiceClient.GetBlobContainerClient(StorageNames.ContainerName);

			try
			{
				var filePath = GetFileName(file, Guid.NewGuid());

				var folderPath = $"{companyName}/";
				var blobName = $"{folderPath}{Guid.NewGuid()}_{ file.FileName}";

				using (var stream = File.Create(filePath))
				{
					await file.CopyToAsync(stream);
				}

				var blobClient = containerClient.GetBlobClient(blobName);
				await using (var uploadStream = File.OpenRead(filePath))
				{
					await blobClient.UploadAsync(uploadStream, true);
					uploadStream.Close();
				}

				return blobClient.Uri.AbsoluteUri;
			}
			catch (Exception ex)
			{
				_logger.LogWarning("An error occurred while trying to upload an image to Google.");
				_logger.LogError("Error Received: {errorMessage}", ex.Message);
				throw;
			}
		}

		private string GetFileName(IFormFile file, Guid newUid)
		{
			var filePath = GetFilePath(newUid, string.Empty);

			Directory.CreateDirectory(filePath);

			return Path.Combine(filePath, file.FileName);
		}

		private string GetFilePath(Guid userUid, string addToPath)
			=> Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "hrvster", userUid.ToString(), addToPath);
	}
}
