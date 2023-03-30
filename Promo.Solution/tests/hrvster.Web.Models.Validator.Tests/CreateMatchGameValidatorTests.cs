using Bogus;
using FluentAssertions;
using hrvster.Core.Authentication;
using hrvster.Infrastructure;
using hrvster.Infrastructure.MatchGames;
using hrvster.Models;
using hrvster.Web.Validation;
using Moq;
using TestUtilities.Data;
using TestUtilities.Mocks.Authentication;
using Xunit;

namespace hrvster.Web.Models.Validator.Tests
{
	public class CreateMatchGameValidatorTests
	{
		private Mock<IMatchGamesRepository> _mockedMatchGameRepository;
		private Mock<ISessionManager> _mockedSessionManager;
		private Mock<ICampaignRepository> _mockedCampaignRepository;

		private CreateMatchGameValidator Validator;
		private Guid CampaignUid;

		public CreateMatchGameValidatorTests()
		{
			CampaignUid = Guid.Parse("ad21aa80-6832-4713-b668-784d76fbdf11");
			_mockedMatchGameRepository = new Mock<IMatchGamesRepository>();
			_mockedSessionManager = new Mock<ISessionManager>();
			_mockedCampaignRepository = new Mock<ICampaignRepository>();

			Randomizer.Seed = new Random(8675309);
			SetupMockedSessionManager();
			_mockedMatchGameRepository.Setup(r => r.GetAllForAccount(It.IsAny<Guid>()))
				.Returns(GetMatchGames());
			_mockedCampaignRepository.Setup(c => c.GetAccountCampaigns(It.IsAny<Guid>()))
				.Returns(GetCampaigns(CampaignUid));
		}

		[Fact]
		public void Should_Have_A_Name()
		{
			var maxNumberOfBoards = 100;
			var minNumberOfBoards = 1;
			
			ResetMocks();

			Randomizer.Seed = new Random(8675309);
			SetupMockedSessionManager();
			_mockedMatchGameRepository.Setup(r => r.GetAllForAccount(It.IsAny<Guid>()))
				.Returns(GetMatchGames());

			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.PromotionUid, f => CampaignUid)
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => f.Random.Number(minNumberOfBoards, maxNumberOfBoards))
				.RuleFor(x => x.WinnableBoards, f => f.Random.Number(minNumberOfBoards + 1, maxNumberOfBoards - 1))
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.Name = string.Empty;

			Validator = new CreateMatchGameValidator(
				_mockedSessionManager.Object,
				_mockedMatchGameRepository.Object,
				_mockedCampaignRepository.Object);
			var result = Validator.Validate(createMatchGame);

			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Validation_Should_Be_False_For_0_Boards()
		{
			var maxNumberOfBoards = 100;
			var minNumberOfBoards = 1;

			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.PromotionUid, f => CampaignUid)
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => f.Random.Number(minNumberOfBoards, maxNumberOfBoards))
				.RuleFor(x => x.WinnableBoards, f => f.Random.Number(minNumberOfBoards + 1, maxNumberOfBoards - 1))
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.NumberOfBoards = 0;

			Validator = new CreateMatchGameValidator(
				_mockedSessionManager.Object,
				_mockedMatchGameRepository.Object,
				_mockedCampaignRepository.Object);
			var result = Validator.Validate(createMatchGame);

			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Validation_Should_Be_False_For_1001_Boards()
		{
			var maxNumberOfBoards = 100;
			var minNumberOfBoards = 1;

			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.PromotionUid, f => CampaignUid)
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => f.Random.Number(minNumberOfBoards, maxNumberOfBoards))
				.RuleFor(x => x.WinnableBoards, f => f.Random.Number(minNumberOfBoards + 1, maxNumberOfBoards - 1))
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.NumberOfBoards = 1001;

			Validator = new CreateMatchGameValidator(
				_mockedSessionManager.Object,
				_mockedMatchGameRepository.Object,
				_mockedCampaignRepository.Object);
			var result = Validator.Validate(createMatchGame);

			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Winnable_Boards_Should_Not_Be_Greater_Than_Number_Of_Boards()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.PromotionUid, f => CampaignUid)
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.WinnableBoards = 101;

			Validator = new CreateMatchGameValidator(
				_mockedSessionManager.Object,
				_mockedMatchGameRepository.Object,
				_mockedCampaignRepository.Object);
			var result = Validator.Validate(createMatchGame);

			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Winnable_Boards_Should_Not_Be_Less_Than_1()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.PromotionUid, f => CampaignUid)
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.WinnableBoards = 0;

			Validator = new CreateMatchGameValidator(
				_mockedSessionManager.Object,
				_mockedMatchGameRepository.Object,
				_mockedCampaignRepository.Object);
			var result = Validator.Validate(createMatchGame);

			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Win_Probability_Should_Be_Between_1_And_9()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.PromotionUid, f => CampaignUid)
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinnableBoards, f => 90)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.WinProbability = 0;

			Validator = new CreateMatchGameValidator(
				_mockedSessionManager.Object,
				_mockedMatchGameRepository.Object,
				_mockedCampaignRepository.Object);
			var result = Validator.Validate(createMatchGame);
			result.IsValid.Should().BeFalse();

			createMatchGame.WinProbability = 10;
			Validator = new CreateMatchGameValidator(
				_mockedSessionManager.Object,
				_mockedMatchGameRepository.Object,
				_mockedCampaignRepository.Object);
			result = Validator.Validate(createMatchGame);
			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Start_Date_Should_Not_Be_Less_Then_Now()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.PromotionUid, f => CampaignUid)
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinnableBoards, f => 90)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.StartDate = DateTime.Now.AddHours(-5);

			Validator = new CreateMatchGameValidator(
				_mockedSessionManager.Object,
				_mockedMatchGameRepository.Object,
				_mockedCampaignRepository.Object);
			var result = Validator.Validate(createMatchGame);
			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Start_Date_Should_Not_Be_Greater_Then_Now()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.PromotionUid, f => CampaignUid)
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinnableBoards, f => 90)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.StartDate = DateTime.Now.AddHours(5);

			Validator = new CreateMatchGameValidator(
				_mockedSessionManager.Object,
				_mockedMatchGameRepository.Object,
				_mockedCampaignRepository.Object);
			var result = Validator.Validate(createMatchGame);
			result.IsValid.Should().BeTrue();
		}

		[Fact]
		public void EndDate_Should_Not_Be_Less_Then_StartDate()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.PromotionUid, f => CampaignUid)
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinnableBoards, f => 90)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.StartDate = DateTime.Now.AddHours(5);
			createMatchGame.EndDate = createMatchGame.StartDate.AddHours(-10);

			Validator = new CreateMatchGameValidator(
				_mockedSessionManager.Object,
				_mockedMatchGameRepository.Object,
				_mockedCampaignRepository.Object);
			var result = Validator.Validate(createMatchGame);
			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void EndDate_Should_Be_Greater_Then_StartDate()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.PromotionUid, f => CampaignUid)
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinnableBoards, f => 90)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.StartDate = DateTime.Now.AddHours(5);
			createMatchGame.EndDate = createMatchGame.StartDate.AddDays(60);

			Validator = new CreateMatchGameValidator(
				_mockedSessionManager.Object,
				_mockedMatchGameRepository.Object,
				_mockedCampaignRepository.Object);
			var result = Validator.Validate(createMatchGame);
			result.IsValid.Should().BeTrue();
		}

		[Fact]
		public void MatchGame_Already_Exists_Return_False()
		{
			var updatedName = "NewName";
			ResetMocks();

			Randomizer.Seed = new Random(8675309);
			SetupMockedSessionManager();
			_mockedMatchGameRepository.Setup(r => r.GetAllForAccount(It.IsAny<Guid>()))
				.Returns(GetMatchGamesSetFirstName(updatedName));

			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.PromotionUid, f => CampaignUid)
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinnableBoards, f => 90)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.Name = updatedName;

			Validator = new CreateMatchGameValidator(
				_mockedSessionManager.Object,
				_mockedMatchGameRepository.Object,
				_mockedCampaignRepository.Object);
			var result = Validator.Validate(createMatchGame);
			result.IsValid.Should().BeFalse();
		}

		private void SetupMockedSessionManager()
		{
			var user = new HrvsterUser();
			user.UserUid = TestData.AccountUid;
			user.EmailAddress = TestData.EmailAddress;
			user.Token = TestData.FirebaseToken;
			user.DisplayName = TestData.DisplayName;
			user.Locale = TestData.Locale;
			user.WindowsTimeZone = TestData.WindowsTimezone;
			user.IsDaylight = false;

			_mockedSessionManager.SetupGetValue(SessionConstants.UserToken, TestData.FirebaseToken);
			_mockedSessionManager.SetupGetValue(SessionConstants.UserUid, TestData.UserUid);
			_mockedSessionManager.SetupGetValue(SessionConstants.UserDisplayName, TestData.DisplayName);
			_mockedSessionManager.SetupGetValue(SessionConstants.UserEmail, TestData.EmailAddress);
			_mockedSessionManager.SetupGetValue(SessionConstants.IsDaylight, false.ToString());
			_mockedSessionManager.SetupGetValue(SessionConstants.Locale, TestData.Locale);
			_mockedSessionManager.SetupGetValue(SessionConstants.WindowsLocale, TestData.WindowsTimezone);

			_mockedSessionManager.Setup(m => m.GetUserInformation())
				.Returns(user);
		}

		private void ResetMocks()
		{
			_mockedMatchGameRepository.Reset();
			_mockedSessionManager.Reset();
		}

		private List<Campaign> GetCampaigns(Guid existingUid)
		{
			var campaignFaker = new Faker<Campaign>()
				.RuleFor(x => x.CampaignName, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.Account, f => new Account())
				.RuleFor(x => x.CampaignType, f => new CampaignType())
				.RuleFor(x => x.CampaignUid, f => f.Random.Guid())
				.RuleFor(x => x.StartDate, f => DateTime.Now.AddDays(1))
				.RuleFor(x => x.EndDate, f => DateTime.Now.AddDays(30));

			var campaigns = campaignFaker.Generate(4);
			campaigns.FirstOrDefault().CampaignUid = existingUid;

			return campaigns;
		}

		private IEnumerable<Games.Match.Models.MatchGame> GetMatchGames()
		{
			var createMatchGameFaker = new Faker<Games.Match.Models.MatchGame>()
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => f.Random.Number(1, 1000))
				.RuleFor(x => x.NumberOfWinnableBoards, f => f.Random.Number(1 + 1, 1000 - 1))
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var games = createMatchGameFaker.Generate(10);

			return games;
		}

		private IEnumerable<Games.Match.Models.MatchGame> GetMatchGamesSetFirstName(string name)
		{
			var createMatchGameFaker = new Faker<Games.Match.Models.MatchGame>()
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => f.Random.Number(1, 1000))
				.RuleFor(x => x.NumberOfWinnableBoards, f => f.Random.Number(1 + 1, 1000 - 1))
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)))
				.RuleFor(x => x.GenerateBoardDate, f => DateTime.Now);

			var games = createMatchGameFaker.Generate(10);
			games.FirstOrDefault().Name = name;

			return games;
		}
	}
}