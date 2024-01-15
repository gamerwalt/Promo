using Castle.Core.Configuration;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Web.Integration.Tests
{
    public class Feature : BaseIntegrationTest<Feature> //, IApiFeature
    {
        public TestServer TestServer { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }
        public IServiceCollection ServiceCollection { get; private set; }
        public HttpClient Client { get; private set; }
        public HttpResponseMessage Response { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public Feature()
        {
            try
            {
                TestServer = BaseTestServer;

                Client = TestServer?.CreateClient();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
