using System.Net.Http;

namespace TestUtilities
{
    public interface IWebFeature<TFeature> where TFeature : class
    {
        public HttpResponseMessage Response { get; set; }
        public HttpClient Client { get; set; }

        void DeleteAllTableData();
    }
}
