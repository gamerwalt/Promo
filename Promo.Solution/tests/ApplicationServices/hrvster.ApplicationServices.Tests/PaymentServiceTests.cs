using hrvster.ApplicationServices.Tests.Common;
using TestUtilities;
using Xunit;

namespace hrvster.ApplicationServices.Tests
{
	public class PaymentServiceTests : BuilderClassAbstract, IClassFixture<PaymentFeature>
	{
		private readonly PaymentFeature _feature;

		public PaymentServiceTests(PaymentFeature feature)
			=> _feature = feature;


	}
}
