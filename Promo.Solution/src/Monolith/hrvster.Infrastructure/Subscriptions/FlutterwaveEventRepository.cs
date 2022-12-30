using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Subscriptions
{
	public interface IFlutterwaveEventRepository
	{

	}

	public class FlutterwaveEventRepository : IFlutterwaveEventRepository
	{
		private readonly IDatabase _database;
		public FlutterwaveEventRepository(IDatabase database)
		{
			_database = database;
		}
	}
}
