using hrvster.Core.Clients.Common;
using System.Threading.Tasks;

namespace hrvster.Core.Clients.Publishers
{
	public class Client : IExternalClient
	{
		public Task DeleteAsync(string externalId, string accessToken, string accessTokenSecret)
		{
			return Task.CompletedTask;
		}

		public Task<string> PublishAsync(Message message)
		{
			return Task.FromResult(string.Empty);
		}

		public Task<string> PublishResponse(Message message, string responseToId)
		{
			return Task.FromResult(string.Empty);
		}
	}
}
