using hrvster.Core.Handlers.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
	public class SubscriptionHandlerTests : BuilderClassAbstract, IClassFixture<MediatorHandlerFeature>
	{
		private readonly MediatorHandlerFeature _feature;

		public SubscriptionHandlerTests(MediatorHandlerFeature feature)
			=> _feature = feature;

	}
}
