using System.Collections.Generic;
using System;

namespace hrvster.Web.Models.Plays
{
	public class GameplayLoss
	{
		public Guid GameUid { get; set; }
		public string GameName { get; set; }
		public MatchBoard MatchBoard { get; set; }
		public Guid BoardUid { get; set; }
		public List<MatchTile> Tiles { get; set; }
		public List<Guid> SelectedTiles { get; set; }
		public Guid GameplayUid { get; set; }
	}
}
