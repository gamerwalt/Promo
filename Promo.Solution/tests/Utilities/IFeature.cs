using Microsoft.Extensions.Configuration;

namespace TestUtilities
{
	public interface IFeature<TFeature> where TFeature : class
	{
		public IConfiguration Configuration { get; }
		void DeleteAllTableData();
	}
}
