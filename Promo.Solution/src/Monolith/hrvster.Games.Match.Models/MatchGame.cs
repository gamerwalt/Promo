using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Games.Match.Models
{
	public class MatchGame
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public int CampaignId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int NumberOfBoards { get; set; }
		public int TileCountPerBoard { get; set; }
		public int NumberOfWinnableBoards { get; set; }
		public int WinProbability { get; set; }
	}
}
