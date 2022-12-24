using FluentValidation;
using hrvster.Core.Authentication;
using hrvster.Infrastructure;
using hrvster.Web.Models;
using System;
using System.Linq;

namespace hrvster.Web.Validation
{
	public class CreateNewSweepValidator : AbstractValidator<CreateNewSweep>
	{
		private readonly ICampaignRepository _campaignRepository;
		private readonly ISweepsRepository _sweepsRepository;
		private readonly hrvster.Models.HrvsterUser _hrvsterUser;

		public CreateNewSweepValidator(
			ICampaignRepository campaignRepository,
			ISweepsRepository sweepsRepository,
			hrvster.Models.HrvsterUser hrvsterUser)
		{
			_campaignRepository = campaignRepository;
			_sweepsRepository = sweepsRepository;
			_hrvsterUser = hrvsterUser;

			RuleFor(c => c.Name)
				.NotEmpty().WithMessage("The sweepstake name is required.");

			RuleFor(c => c.Name)
				.Must(name => ShouldNotExistAlready(name))
				.WithMessage("The sweepstake name already exists.");

			RuleFor(sweepStake => sweepStake.StartDate)
				.Must(startDate => BeGreaterThanToday(startDate))
				.WithMessage("The start date must be greater than today.");

			RuleFor(sweepStake => sweepStake.StartDate)
				.Must((sweepStake, startDate) => BeBetweenPromotionDates(sweepStake, startDate))
				.WithMessage("The sweepstake start date should be between the promotion dates.");

			RuleFor(sweepStake => sweepStake)
				.Must(sweepStake => sweepStake.EndDate > sweepStake.StartDate)
				.WithMessage(sweepStake => $"The start date {sweepStake.StartDate.ToString("d")} must come before the end date {sweepStake.EndDate.ToString("d")}");

			RuleFor(sweepStake => sweepStake.EndDate)
				.Must((sweepStake, endDate) => BeBetweenPromotionDates(sweepStake, endDate))
				.WithMessage("The sweepstake end date should be between the promotion dates.");

			RuleFor(sweepStake => sweepStake.DrawDate)
				.Must((sweepStake, drawDate) => HappenAfterTheEndDate(sweepStake, drawDate))
				.WithMessage("The draw date should happen after the sweepstake's end date.");

			RuleFor(sweepStake => sweepStake.DrawDate)
				.Must((sweepStake, drawDate) => BeBetweenPromotionDates(sweepStake, drawDate))
				.WithMessage("The sweepstake draw date should be between the promotion dates.");

			RuleFor(sweepstake => sweepstake.AcceptTerms)
				.Must(c => BeAccepted(c))
				.WithMessage("You have to accept the terms and conditions of creating a game on our platform.");
		}

		private bool BeAccepted(bool accepted)
			=> accepted;

		private bool ShouldNotExistAlready(string name)
		{
			var accountUid = _hrvsterUser.UserUid;
			var sweeps = _sweepsRepository.GetAllForAccount(accountUid);

			var result = sweeps.Any(s => s.Name == name);

			return !result;
		}

		private bool HappenAfterTheEndDate(CreateNewSweep sweepStake, DateTime drawDate)
		{
			return drawDate > sweepStake.EndDate;
		}

		private bool BeBetweenPromotionDates(CreateNewSweep sweepStake, DateTime inputDate)
		{
			var campaign = _campaignRepository.GetCampaignByUid(sweepStake.Campaign.CampaignUid, _hrvsterUser.UserUid);

			if (campaign == null) return true;

			return inputDate > campaign.StartDate &&
				inputDate <= campaign.EndDate;
		}

		private bool BeGreaterThanToday(DateTime startDate)
		{
			return startDate > DateTime.Now;
		}
	}
}
