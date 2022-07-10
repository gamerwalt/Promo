namespace hrvster.Infrastructure
{
	public interface IDatabase
	{
		string GetConnectionString();
		string GetConnectionName();
		void DeleteTestData();
		string GetHarvesterConnectionString();
		string GetReportingConnectionString();
	}
}
