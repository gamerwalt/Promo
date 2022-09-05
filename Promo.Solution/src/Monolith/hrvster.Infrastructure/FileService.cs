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
		Task UploadToFileServer(IFormFile media, string addToPath);
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

		public Task UploadToFileServer(IFormFile media, string filePath)
		{
			Directory.CreateDirectory(filePath);
			filePath = Path.Combine(filePath, media.FileName);

			using var stream = File.Create(filePath);
			return media.CopyToAsync(stream);
		}
	}
}
