using System.Threading.Tasks;

namespace hrvster.Services
{
	public interface IService
	{
		Task ProcessService(System.Threading.CancellationToken token);
	}
}
