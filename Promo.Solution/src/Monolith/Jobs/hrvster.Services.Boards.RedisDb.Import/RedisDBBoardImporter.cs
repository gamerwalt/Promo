using hrvster.ApplicationServices.JobContracts;
using System.Diagnostics;

namespace hrvster.Services.Boards.RedisDb.Import
{
	public class RedisDBBoardImporter : IService
	{
		private readonly ILogger<RedisDBBoardImporter> _logger;
		private readonly string _connectionString;
		private readonly string _databaseName;
		private readonly string _matchTileCollectionName;
		private readonly string _directory;
		private readonly IConfiguration _config;

		public RedisDBBoardImporter(
			ILoggerFactory loggerFactory,
			string connectionString,
			string databaseName,
			string matchTileCollectionName,
			string directory,
			IConfiguration config)
		{
			_logger = loggerFactory.CreateLogger<RedisDBBoardImporter>();
			_connectionString = connectionString;
			_databaseName = databaseName;
			_matchTileCollectionName = matchTileCollectionName;
			_directory = directory;
			_config = config;
		}

		public async Task ProcessService(CancellationToken token)
		{
			var textFiles = Directory.GetFiles(_directory, "*.text");
			var backupLocation = _config["BoardFilesBackupLocation"];

			foreach (var filePath in textFiles)
			{
				_logger.LogInformation($"Loading {filePath} into Redis...");

				// Unix/Linux command to run redis-cli with the --pipe option
				// For Windows, you might need to adjust the command to use "type" instead of "cat" and "cmd.exe" instead of "/bin/bash"
				var command = $"cat \"{filePath}\" | redis-cli --pipe";

				// ProcessStartInfo setup
				var psi = new ProcessStartInfo
				{
					FileName = "cmd.exe", // Use "cmd.exe" on Windows
					Arguments = $"-c \"{command}\"",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				};

				try
				{
					using (Process process = Process.Start(psi))
					{
						// Reading output to the console.
						string output = process.StandardOutput.ReadToEnd();
						string errors = process.StandardError.ReadToEnd();
						process.WaitForExit(); // Wait for the process to complete

						// Log or handle the output and errors appropriately
						_logger.LogInformation("Output from Redis loading process:");
						_logger.LogInformation(output);
						if (!string.IsNullOrEmpty(errors))
						{
							_logger.LogInformation("Errors during Redis loading:");
							_logger.LogError(errors);
						}

						// If the file was processed without errors, move it to the backup directory
						if (process.ExitCode == 0)
						{
							var backupFilePath = Path.Combine(backupLocation, Path.GetFileName(filePath));
							File.Move(filePath, backupFilePath);
							_logger.LogInformation($"Moved processed file to backup: {backupFilePath}");
						}
					}
				}
				catch (Exception ex)
				{
					// Log or handle exceptions
					_logger.LogInformation("Exception occurred:");
					_logger.LogInformation(ex.Message);
				}
			}
		}
	}
}
