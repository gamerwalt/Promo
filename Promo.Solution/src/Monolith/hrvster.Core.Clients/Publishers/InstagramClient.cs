using hrvster.Core.Clients.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Clients.Publishers
{
	public class InstagramClient : IExternalClient
	{
		public Task DeleteAsync(string externalId, string accessToken, string accessTokenSecret)
		{
			throw new NotImplementedException();
		}

		public Task<string> PublishAsync(Message message)
		{
			throw new NotImplementedException();
		}
	}
}
