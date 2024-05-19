namespace hrvster.MatchTile.Creator
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IHost host = Host.CreateDefaultBuilder(args)
				.ConfigureServices(services =>
				{
					services.AddHostedService<TileCreatorHostedService>();

					services.AddTransient<TileMongoDbConsumer>();
				})
				.Build();

			host.Run();
		}
	}
}