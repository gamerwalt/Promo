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

namespace hrvster.Core.Handlers.Files
{
	public class UploadFileToFileServer : IRequestHandler<UploadFileToFileServerRequest, IEnumerable<string>>
	{
		private readonly ILogger<UploadFileToFileServer> _logger;
		private readonly IFileService _fileService;

		public UploadFileToFileServer(ILoggerFactory loggerFactory, IFileService fileService)
		{
			_logger = loggerFactory.CreateLogger<UploadFileToFileServer>();
			_fileService = fileService;
		}

		public async Task<IEnumerable<string>> Handle(UploadFileToFileServerRequest request, CancellationToken cancellationToken)
		{
			var listOfPaths = new List<string>();

			foreach(var media in request.Media)
			{
				listOfPaths.Add(
					await _fileService.UploadToFileServer(media, request.ProfileType.ToString(), request.UserUid)
					);
			}

			return listOfPaths;
		}
	}
}
