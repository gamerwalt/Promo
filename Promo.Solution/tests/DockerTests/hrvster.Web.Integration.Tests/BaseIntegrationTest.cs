using MediatR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Web.Integration.Tests
{
    public abstract class BaseIntegrationTest<TFeature> : IntegrationTestWebAppFactory<TFeature> where TFeature : class
    {
        public IMediator TestMediator { get { return ServiceProvider.GetRequiredService<IMediator>(); } }
        public TestServer BaseTestServer { get { return TestServer; } }
    }
}
