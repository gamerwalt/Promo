namespace hrvster.Models.Sweepstakes
{
	public class SweepDrawRule
	{
		public Sweep Sweep { get; set; }
		public Campaign Campaign { get; set; }
		public DrawRule DrawRule { get; set; }
	}
}
