using Promo.TestUtilities.Database;
using System.Net.Http;

namespace Promo.TestUtilities.Common
{
    public interface IApiFeature<TFeature> : 
        IFeature<TFeature>, 
        IStorageTestFixtureBase<TFeature> where TFeature : class
    {
        public HttpResponseMessage Response { get; set; }
        public HttpClient Client { get; set; }
    }
}
