namespace SweepEntrySeeder
{
	public class SweepstakeSeederOptions
	{
		public const string SectionName = "SweepstakeSeeder";

		public string SweepstakeUids { get; set; }
		public int TotalEntries { get; set; }
		public bool Duplicates { get; set; }
	}
}
