using System;
using System.Collections.Generic;

namespace hrvster.Web.Models.Plays
{
	public class PlayMatchGame
	{
		public Guid GameUid { get; set; }
		public string GameName { get; set; }
		public MatchBoard MatchBoard { get; set; }
		public string SelectedTiles { get; set; }
		public Guid BoardUid { get; set; }
		public List<Guid> Tiles { get; set; }
		public Guid GameplayUid { get; set; }
	}

	public class MatchBoard
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public bool IsWinnable { get; set; }
		public MatchGame MatchGame { get; set; }
	}
}
