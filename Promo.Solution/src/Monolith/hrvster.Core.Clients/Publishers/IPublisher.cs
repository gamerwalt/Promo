using hrvster.Core.Clients.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Clients.Publishers
{
	public interface IPublisher
	{
		Task PublishAsync(Message message);
	}
}
