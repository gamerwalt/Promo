using hrvster.Core.Clients.Common;
using System.Threading.Tasks;

namespace hrvster.Core.Clients.Publishers
{
	public interface IExternalClient
	{
		Task<string> PublishAsync(Message message);
		Task DeleteAsync(string externalId, string accessToken, string accessTokenSecret);
	}
}
