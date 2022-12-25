using hrvster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtilities.Builders.Resources
{
	public class SweepBuilder
	{
		#region Fields
		public int SweepId { get; set; }
		public Guid SweepUid { get; set; }
		public Campaign Campaign { get; set; }
		public Account Account { get; set; }
		public string Name { get; set; }
		public string RulesUrl { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime StartDateUtc { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime EndDateUtc { get; set; }
		public DateTime? DrawDate { get; set; }
		public DateTime DrawDateUtc { get; set; }
		public bool Drawn { get; set; }
		public DateTime? DateDrawn { get; set; }

		#endregion

		public SweepBuilder(int sweepId, Guid sweepUid, string name)
		{
			SweepId = sweepId;
			SweepUid = sweepUid;
			Name = name;
		}

		public SweepBuilder Create(int sweepId, Guid sweepUid, string name, DateTime startDate, DateTime endDate, DateTime drawDate, Campaign campaign, Account account)
			=> new SweepBuilder(sweepId, sweepUid, name)
				.WithStartDate(startDate)
				.WithEndDate(endDate)
				.WithDrawDate(drawDate)
				.WithCampaign(campaign)
				.WithAccount(account)
				.WithDrawn(false);

		public Sweep Build()
			=> new Sweep
			{
				SweepId = SweepId,
				SweepUid = SweepUid,
				Name = Name,
				StartDate = StartDate,
				EndDate = EndDate,
				DrawDate = DrawDate,
				Drawn = Drawn,
				DateDrawn = DateDrawn,
				Campaign = Campaign,
				Account = Account
			};

		public SweepBuilder WithAccount(Account account)
		{
			Account = account;

			return this;
		}

		public SweepBuilder WithCampaign(Campaign campaign)
		{
			Campaign = campaign;

			return this;
		}

		public SweepBuilder WithDrawn(bool drawn)
		{
			Drawn = drawn;

			return this;
		}

		public SweepBuilder WithStartDate(DateTime startDate)
		{
			StartDate= startDate;

			return this;
		}

		public SweepBuilder WithEndDate(DateTime endDate)
		{
			EndDate= endDate; 
			
			return this;
		}

		public SweepBuilder WithDrawDate(DateTime drawDate)
		{
			DrawDate= drawDate;

			return this;
		}
	}
}
