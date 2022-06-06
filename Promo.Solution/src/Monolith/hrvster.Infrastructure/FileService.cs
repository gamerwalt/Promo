using hrvster.Models.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface IFileService
	{
		Task<string> UploadToFileServer(IFormFile media, string addToPath, Guid userUid);
	}

	public class FileService : IFileService
	{
		private readonly ILogger<FileService> _logger;
		private readonly FileServerOptions _fileServerOptions;

		public FileService(ILoggerFactory loggerFactory, FileServerOptions fileServerOptions)
		{
			_logger = loggerFactory.CreateLogger<FileService>();
			_fileServerOptions = fileServerOptions;
		}

		public async Task<string> UploadToFileServer(IFormFile media, string addToPath, Guid userUid)
		{
			var filePath = GetFilePath(userUid, addToPath);

			Directory.CreateDirectory(filePath);
			filePath = Path.Combine(filePath, media.FileName);

			using var stream = File.Create(filePath);
			await media.CopyToAsync(stream);

			return filePath;
		}

		private string GetFilePath(Guid userUid, string addToPath)
		{
			//return Path.Combine(_fileServerOptions.Path, userUid.ToString(), addToPath);
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "hrvster", userUid.ToString(), addToPath);
		}
	}
}
