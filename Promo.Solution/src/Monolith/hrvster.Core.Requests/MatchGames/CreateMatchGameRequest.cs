using MediatR;
using System;

namespace hrvster.Core.Requests.MatchGames
{
	public class CreateMatchGameRequest : IRequest<bool>
	{
		private Guid _matchGammeUid;
		private Guid _accountUid;
		private Guid _promotionUid;
		private string _name;
		private string _description;
		private int _numberOfBoards;
		private int _winnableBoards;
		private int _winProbability;
		private DateTime _startDate;
		private DateTime _endDate;
		private DateTime _generateBoardDate;
		private string _timeZone;
		private bool _isDaylight;
		private bool _accumulatePointsOnLoss;
		private int _pointsPerLoss;

		public CreateMatchGameRequest(
			Guid matchGameUid, 
			Guid accountUid, 
			Guid promotionUid, 
			string name, 
			string description, 
			int numberOfBoards, 
			int winnableBoards, 
			int winProbability, 
			DateTime startDate, 
			DateTime endDate, 
			DateTime generateBoardDate, 
			bool accumulatePointsOnLoss, 
			int pointsPerLoss, 
			string timeZone, 
			bool isDaylight)
		{
			_matchGammeUid = matchGameUid;
			_accountUid = accountUid;
			_promotionUid = promotionUid;
			_name = name;
			_description = description;
			_numberOfBoards = numberOfBoards;
			_winnableBoards = winnableBoards;
			_winProbability = winProbability;
			_startDate = startDate;
			_endDate = endDate;
			_generateBoardDate = generateBoardDate;
			_timeZone = timeZone;
			_isDaylight = isDaylight;
			_accumulatePointsOnLoss = accumulatePointsOnLoss;
			_pointsPerLoss = pointsPerLoss;
		}

		public Guid MatchGameUid { get { return _matchGammeUid; } }
		public Guid AccountUid { get { return _accountUid; } }
		public Guid PromotionUid { get { return _promotionUid; } }
		public string Name { get { return _name;} }
		public string Description { get { return _description;} }
		public int NumberOfBoards { get { return _numberOfBoards;} }
		public int WinnableBoards { get { return _winnableBoards; } }
		public int WinProbability { get { return _winProbability; } }
		public DateTime StartDate { get { return _startDate; } }
		public DateTime EndDate { get { return _endDate; } }
		public DateTime GenerateBoardDate { get { return _generateBoardDate; } }
		public string TimeZone { get { return _timeZone; } }
		public bool IsDayLight { get { return _isDaylight; } }
		public bool AccumulatePointsOnLoss { get { return _accumulatePointsOnLoss; } }
		public int PointsPerLoss { get { return _pointsPerLoss; } }
	}
}
