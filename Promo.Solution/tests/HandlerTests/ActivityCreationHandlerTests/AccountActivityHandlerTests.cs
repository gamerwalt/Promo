using ActivityCreationHandlerTests.Common;
using FluentAssertions;
using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Web.Mvc.Common.Steps;
using MediatR;
using System.Threading.Tasks;
using TestUtilities.Common.Steps;
using TestUtilities.Data;
using Xunit;

namespace ActivityCreationHandlerTests
{
	[Collection("Sequential")]
	public class AccountActivityHandlerTests : IClassFixture<ActivityFeature>
	{
		private readonly ActivityFeature _feature;

		public AccountActivityHandlerTests(ActivityFeature feature)
			=> _feature = feature;

		[Fact]
		public async Task When_We_Create_A_New_User_The_User_Acitivty_Is_Saved()
		{
			_feature.GivenACleanSlate();

			var accountCreateRequest = new CreateAccountRequest(TestData.AccountUid, TestData.EmailAddress, TestData.DisplayName, TestData.PhoneNumber, TestData.FirstName, TestData.LastName, TestData.Password, true);

			var result = await _feature.Mediator.Send(accountCreateRequest);

			result.Should().BeOfType<Unit>();
			_feature.AccountsTableShouldContain(AccountTableFields.EmailAddress, TestData.EmailAddress);
			_feature.ActivitiesTableShouldContaine(ActivityTableFields.LinkedUid, TestData.AccountUid.ToString());
		}

		[Fact]
		public async Task When_A_User_Logs_In_The_Activity_Is_Stored()
		{
			var accountCreateRequest = new CreateAccountRequest(TestData.AccountUid, TestData.EmailAddress, TestData.DisplayName, TestData.PhoneNumber, TestData.FirstName, TestData.LastName, TestData.Password, true);

			var result = await _feature
				.GivenACleanSlate()
				.Mediator.Send(accountCreateRequest);

			result.Should().BeOfType<Unit>();
		}
	}
}