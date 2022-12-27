namespace hrvster.Models.Sweepstakes
{
	public class SweepDrawPropertiesAndRule
	{
		public Sweep Sweep { get; set; }
		public Campaign Campaign { get; set; }
		public DrawRule DrawRule { get; set; }
		public WatchType WatchType { get; set; }
		public Rule SweepRule { get; set; }
		public Account Account { get; set; }
	}
}
