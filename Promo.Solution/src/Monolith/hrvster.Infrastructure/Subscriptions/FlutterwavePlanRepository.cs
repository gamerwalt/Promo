using hrvster.Models.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Subscriptions
{
	public interface IFlutterwavePlanRepository
	{

	}

	public class FlutterwavePlanRepository : IFlutterwavePlanRepository
	{
		private readonly IDatabase _database;
		public FlutterwavePlanRepository(IDatabase database)
		{
			_database = database;
		}
	}
}
