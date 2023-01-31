using Bogus;
using FluentAssertions;
using hrvster.Web.Validation;
using Xunit;

namespace hrvster.Web.Models.Validator.Tests
{
	public class CreateMatchGameValidatorTests
	{
		[Fact]
		public void Should_Have_A_Name()
		{
			var maxNumberOfBoards = 100;
			var minNumberOfBoards = 1;

			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => f.Random.Number(minNumberOfBoards, maxNumberOfBoards))
				.RuleFor(x => x.WinnableBoards, f => f.Random.Number(minNumberOfBoards + 1, maxNumberOfBoards - 1))
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)));

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.Name = string.Empty;

			var validator = new CreateMatchGameValidator();
			var result = validator.Validate(createMatchGame);

			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Validation_Should_Be_False_For_0_Boards()
		{
			var maxNumberOfBoards = 100;
			var minNumberOfBoards = 1;

			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => f.Random.Number(minNumberOfBoards, maxNumberOfBoards))
				.RuleFor(x => x.WinnableBoards, f => f.Random.Number(minNumberOfBoards + 1, maxNumberOfBoards - 1))
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)));

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.NumberOfBoards = 0;

			var validator = new CreateMatchGameValidator();
			var result = validator.Validate(createMatchGame);

			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Validation_Should_Be_False_For_1001_Boards()
		{
			var maxNumberOfBoards = 100;
			var minNumberOfBoards = 1;

			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => f.Random.Number(minNumberOfBoards, maxNumberOfBoards))
				.RuleFor(x => x.WinnableBoards, f => f.Random.Number(minNumberOfBoards + 1, maxNumberOfBoards - 1))
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)));

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.NumberOfBoards = 1001;

			var validator = new CreateMatchGameValidator();
			var result = validator.Validate(createMatchGame);

			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Winnable_Boards_Should_Not_Be_Greater_Than_Number_Of_Boards()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)));

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.WinnableBoards = 101;

			var validator = new CreateMatchGameValidator();
			var result = validator.Validate(createMatchGame);

			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Winnable_Boards_Should_Not_Be_Less_Than_1()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)));

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.WinnableBoards = 0;

			var validator = new CreateMatchGameValidator();
			var result = validator.Validate(createMatchGame);

			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Win_Probability_Should_Be_Between_1_And_9()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinnableBoards, f => 90)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)));

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.WinProbability = 0;

			var validator = new CreateMatchGameValidator();
			var result = validator.Validate(createMatchGame);
			result.IsValid.Should().BeFalse();

			createMatchGame.WinProbability = 10;
			validator = new CreateMatchGameValidator();
			result = validator.Validate(createMatchGame);
			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Start_Date_Should_Not_Be_Less_Then_Now()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinnableBoards, f => 90)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)));

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.StartDate = DateTime.Now.AddHours(-5);

			var validator = new CreateMatchGameValidator();
			var result = validator.Validate(createMatchGame);
			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void Start_Date_Should_Not_Be_Greater_Then_Now()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinnableBoards, f => 90)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)));

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.StartDate = DateTime.Now.AddHours(5);

			var validator = new CreateMatchGameValidator();
			var result = validator.Validate(createMatchGame);
			result.IsValid.Should().BeTrue();
		}

		[Fact]
		public void EndDate_Should_Not_Be_Less_Then_StartDate()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinnableBoards, f => 90)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)));

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.StartDate = DateTime.Now.AddHours(5);
			createMatchGame.EndDate = createMatchGame.StartDate.AddHours(-10);

			var validator = new CreateMatchGameValidator();
			var result = validator.Validate(createMatchGame);
			result.IsValid.Should().BeFalse();
		}

		[Fact]
		public void EndDate_Should_Be_Greater_Then_StartDate()
		{
			var createMatchGameFaker = new Faker<CreateMatchGame>()
				.RuleFor(x => x.Name, f => f.Lorem.Word())
				.RuleFor(x => x.Description, f => f.Lorem.Sentences(2))
				.RuleFor(x => x.NumberOfBoards, f => 100)
				.RuleFor(x => x.WinnableBoards, f => 90)
				.RuleFor(x => x.WinProbability, f => f.Random.Number(1, 9))
				.RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.Now.AddHours(3), DateTime.Now.AddHours(12)))
				.RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddDays(30)));

			var createMatchGame = createMatchGameFaker.Generate();
			createMatchGame.StartDate = DateTime.Now.AddHours(5);
			createMatchGame.EndDate = createMatchGame.StartDate.AddDays(60);

			var validator = new CreateMatchGameValidator();
			var result = validator.Validate(createMatchGame);
			result.IsValid.Should().BeTrue();
		}
	}
}