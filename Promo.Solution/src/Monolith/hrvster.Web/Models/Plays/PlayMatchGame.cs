using System;
using System.Collections.Generic;

namespace hrvster.Web.Models.Plays
{
	public class PlayMatchGame
	{
		public Guid GameUid { get; internal set; }
		public string GameName { get; internal set; }
		public string Signature { get; internal set; }
		public int NumberOfGamePlays { get; internal set; }
		public MatchBoard MatchBoard { get; internal set; }
		public List<Guid> SelectedTiles { get; set; }
	}

	public class MatchBoard
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public bool IsWinnable { get; set; }
		public MatchGame MatchGame { get; set; }
		public List<List<MatchTile>> Tiles { get; set; }
	}

	public class MatchTile
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public MatchImage MatchImage { get; set; }
		public MatchPrize Prize { get; set; }
	}
}
