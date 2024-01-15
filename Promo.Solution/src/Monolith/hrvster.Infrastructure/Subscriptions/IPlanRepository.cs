using hrvster.Models.Subscriptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Subscriptions
{
	public interface IPlanRepository
	{
		Task<IEnumerable<Plan>> GetPlansAsync();
	}
}
