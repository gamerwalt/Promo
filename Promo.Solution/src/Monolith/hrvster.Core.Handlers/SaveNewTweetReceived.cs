using hrvster.Core.Requests.Harvester;
using hrvster.Infrastructure.Harvester;
using hrvster.Models.Harvester;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers
{
	public class SaveNewTweetReceived : IRequestHandler<SaveNewTweetReceivedRequest, IncomingTweet>
	{
		private readonly ILogger<SaveNewTweetReceived> _logger;
		private readonly IIncomingTweetsRepository _repository;

		public SaveNewTweetReceived(ILoggerFactory loggerFactory, IIncomingTweetsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<SaveNewTweetReceived>();
			_repository = repository;
		}

		public async Task<IncomingTweet> Handle(SaveNewTweetReceivedRequest request, CancellationToken cancellationToken)
		{
			var incomingTweetUid = Guid.NewGuid();
			await _repository.CreateAsync(incomingTweetUid, request.JsonTweet, IncomingTweetStatus.New);
			
			return await _repository.GetByUidAsync(incomingTweetUid);
		}
	}
}
