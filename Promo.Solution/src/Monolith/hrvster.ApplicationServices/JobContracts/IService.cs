using System.Threading.Tasks;

namespace hrvster.ApplicationServices.JobContracts
{
	public interface IService
	{
		Task ProcessService(System.Threading.CancellationToken token);
	}
}
