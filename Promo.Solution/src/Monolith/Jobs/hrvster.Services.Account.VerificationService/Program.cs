namespace hrvster.Services.Account.VerificationService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IHost host = Host.CreateDefaultBuilder(args)
				.ConfigureServices(services =>
				{
					services.AddHostedService<AccountVerificationHostedService>();
				})
				.Build();

			host.Run();
		}
	}
}