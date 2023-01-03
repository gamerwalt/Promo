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
	public class MailHandlerTests : BuilderClassAbstract, IClassFixture<RepositoryFeature>
	{
		private readonly RepositoryFeature _feature;

		public MailHandlerTests(RepositoryFeature feature)
			=> _feature = feature;

		[Fact]
		public async Task Should_Save_Mail_Information_Into_The_Database()
		{
			_feature.GivenACleanSlate();

			var request = new SweepEntriesNotEnoughMailRequest(TestData.EmailAddress, TestData.SweepName);

			await _feature.Mediator.Send(request);

			_feature.EmailsTableShouldContainFieldEqualTo(MailTableFields.EmailAddress, TestData.EmailAddress);
			_feature.EmailsTableShouldContain(MailTableFields.Parameters, TestData.SweepName);
		}
	}
}
