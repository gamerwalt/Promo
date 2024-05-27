using hrvster.ApplicationServices.JobContracts;
using System.Diagnostics;

namespace hrvster.Services.Boards.RedisDb.Import
{
	public class RedisDBBoardImporter : IService
	{
		private readonly ILogger<RedisDBBoardImporter> _logger;
		private readonly string _redisHost;
		private readonly string _redisAccessKey;
		private readonly int _redisPort;
		private readonly string _directory;
		private readonly IConfiguration _config;

		public RedisDBBoardImporter(
			ILoggerFactory loggerFactory,
			string redisHost,
			string redisAccessKey,
			string redisPort,
			string directory,
			IConfiguration config)
		{
			_logger = loggerFactory.CreateLogger<RedisDBBoardImporter>();
			_redisHost = redisHost;
			_redisAccessKey = redisAccessKey;
			_redisPort = int.Parse(redisPort);
			_directory = directory;
			_config = config;
		}

		public async Task ProcessService(CancellationToken token)
		{
			var filePaths = Directory.GetFiles(_directory, "*.text");
			var options = new ParallelOptions { MaxDegreeOfParallelism = 4 }; // Reduced parallelism
			foreach (var filePath in filePaths)
			{
				var commands = await File.ReadAllLinesAsync(filePath);
				await Parallel.ForEachAsync(commands, options, async (command, cancellationToken) =>
				{
					await ExecuteRedisCommandAsync(command);
				});

				Console.WriteLine($"Processed {filePath}");
			}
		}

		private async Task ExecuteRedisCommandAsync(string command)
		{
			var processStartInfo = new ProcessStartInfo
			{
				FileName = @"C:\Users\hrvster-dev-user\AppData\Roaming\npm\rdcli.cmd",
				Arguments = $"-h {_redisHost} -p {_redisPort} -a {_redisAccessKey} --tls \"{command}\"",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using var process = new Process { StartInfo = processStartInfo };
			process.Start();
			var result = await process.StandardOutput.ReadToEndAsync();
			var error = await process.StandardError.ReadToEndAsync();
			await process.WaitForExitAsync();

			if (!string.IsNullOrEmpty(error))
			{
				_logger.LogWarning("Error: {Error}", error);
			}
			else
			{
				_logger.LogInformation("Success: {Message}", result);
			}
		}
	}
}
