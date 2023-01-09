using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Web.Mvc.Common.Steps;
using TestUtilities;
using TestUtilities.Common.Steps;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
	[Collection("Sequential")]
	public class SweepHandlerTests : BuilderClassAbstract, IClassFixture<MediatorHandlerFeature>
	{
		private readonly MediatorHandlerFeature _feature;

		public SweepHandlerTests(MediatorHandlerFeature feature)
			=> _feature = feature;

	}
}