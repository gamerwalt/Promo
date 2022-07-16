using System.Threading.Tasks;

namespace hrvster.Twitter.Harvester
{
	public interface IService
	{
		Task ProcessService(System.Threading.CancellationToken token);
	}
}
