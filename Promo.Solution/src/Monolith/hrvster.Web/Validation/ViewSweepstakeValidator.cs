using FluentValidation;
using hrvster.Core.Authentication;
using hrvster.Extensions.ModelExtensions;
using hrvster.Infrastructure;
using hrvster.Web.Models;
using System;
using System.Linq;

namespace hrvster.Web.Validation
{
	public class ViewSweepstakeValidator : AbstractValidator<ViewSweepstake>
	{
		private readonly ICampaignRepository _campaignRepository;
		private readonly ISweepsRepository _sweepsRepository;
		private readonly HrvsterUser _hrvsterUser;

		public ViewSweepstakeValidator(
			ICampaignRepository campaignRepository,
			ISweepsRepository sweepsRepository,
			HrvsterUser hrvsterUser)
		{
			_campaignRepository = campaignRepository;
			_sweepsRepository = sweepsRepository;
			_hrvsterUser = hrvsterUser;

			RuleFor(c => c.UpdateSweepstake.Name)
				.NotEmpty().WithMessage("The sweepstake name is required.");

			RuleFor(sweepStake => sweepStake.UpdateSweepstake.Name)
				.Must((sweepStake, name) => ShouldNotExistAlready(sweepStake, name))
				.WithMessage("The sweepstake name already exists.");

			RuleFor(sweepStake => sweepStake.UpdateSweepstake.StartDate)
				.Must((sweepStake, startDate) => BeBetweenPromotionDates(sweepStake, startDate))
				.WithMessage("The sweepstake start date should be between the promotion dates.");

			RuleFor(sweepStake => sweepStake)
				.Must(sweepStake => sweepStake.UpdateSweepstake.EndDate.ToUniversalTime() > sweepStake.UpdateSweepstake.StartDate.ToUniversalTime())
				.WithMessage(sweepStake => $"The start date {sweepStake.UpdateSweepstake.StartDate.ToString("d")} must come before the end date {sweepStake.UpdateSweepstake.EndDate.ToString("d")}");

			RuleFor(sweepStake => sweepStake.UpdateSweepstake.EndDate)
				.Must((sweepStake, endDate) => BeBetweenPromotionDates(sweepStake, endDate))
				.WithMessage("The sweepstake end date should be between the promotion dates.");

			RuleFor(sweepStake => sweepStake.UpdateSweepstake.DrawDate)
				.Must((sweepStake, drawDate) => HappenAfterTheEndDate(sweepStake, drawDate))
				.WithMessage("The draw date should happen after the sweepstake's end date.");

			RuleFor(sweepStake => sweepStake.UpdateSweepstake.DrawDate)
				.Must((sweepStake, drawDate) => BeBetweenPromotionDates(sweepStake, drawDate))
				.WithMessage("The sweepstake draw date should be between the promotion dates.");
		}

		private bool ShouldNotExistAlready(ViewSweepstake form, string name)
		{
			var accountUid = _hrvsterUser.UserUid;
			var campaignUid = form.UpdateSweepstake.CampaignUid;
			var sweeps = _sweepsRepository.GetAllForAccount(accountUid);

			var result = sweeps.Any(s => s.Name == name && s.SweepUid != form.UpdateSweepstake.SweepUid);

			return !result;
		}

		private bool HappenAfterTheEndDate(ViewSweepstake form, DateTime drawDate)
		{
			return drawDate > form.UpdateSweepstake.EndDate;
		}

		private bool BeBetweenPromotionDates(ViewSweepstake sweepStake, DateTime inputDate)
		{
			var campaign = _campaignRepository.GetCampaignByUid(sweepStake.UpdateSweepstake.CampaignUid, _hrvsterUser.UserUid);

			if (campaign == null) return true;

			return inputDate.ToUniversalTime() > campaign.StartDate.ToUniversalTime() &&
				inputDate.ToUniversalTime() <= campaign.EndDate.ToUniversalTime();
		}
	}
}
