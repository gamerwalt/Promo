using System;
using System.Collections.Generic;

namespace hrvster.Models
{
	public class MatchGamePlayAndUsers
	{
		public Guid GameplayUid { get; set; }
		public bool PlayedIndicator { get; set; }
		public bool WinIndicator { get; set; }
		public List<Guid> SelectedTileGuids { get; set; }
		public string ScreenName { get; set; }
		public int TotalPlays { get; set; }
	}
}
