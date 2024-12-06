using MediatR;
using System;

namespace hrvster.Core.Requests.SpinToWinGame
{
	public class CreateSpinToWinGameRequest : IRequest<bool>
	{
		public CreateSpinToWinGameRequest(
			Guid gameUid,
			string name, 
			string description, 
			DateTime startDate, 
			DateTime endDate, 
			DateTime generatePieDate, 
			bool acceptTerms, 
			int numberOfSpins, 
			int maxWinPhysicalPrizes,
			int maxWinPoints,
			int maxPointsFreePlay,
			Guid campaignUid,
			bool spreadMainPrizesPerDay)
		{
			GameUid = gameUid;
			Name = name;
			Description = description;
			StartDate = startDate;
			EndDate = endDate;
			GeneratePieDate = generatePieDate;
			AcceptTerms = acceptTerms;
			NumberOfSpins = numberOfSpins;
			MaxWinForPhysicalPrizes = maxWinPhysicalPrizes;
			MaxWinForPoints = maxWinPoints;
			PointsForFreePlay = maxPointsFreePlay;
			CampaignUid = campaignUid;
			SpreadMainPrizesPerDay = spreadMainPrizesPerDay;
		}

		public Guid GameUid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime GeneratePieDate { get; set; }
		public int WinProbability { get; set; }
		public bool AcceptTerms { get; set; }
		public int NumberOfSpins { get; set; }
		public int MaxWinForPhysicalPrizes { get; set; }
		public int MaxWinForPoints { get; set; }
		public int PointsForFreePlay { get; set; }
		public Guid CampaignUid { get; set; }
		public bool SpreadMainPrizesPerDay { get; set; }
	}
}
