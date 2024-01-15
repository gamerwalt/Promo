using Microsoft.Extensions.Configuration;

namespace hrvster.Web.TestUtilities
{
    public interface IFeature<TFeature> where TFeature : class
    {
        public IConfiguration Configuration { get; }
        void DeleteAllTableData();
    }
}
