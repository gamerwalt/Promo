using FirebaseAdmin.Auth;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.Core.Authentication
{
	public interface IFirebaseAuthManager
	{
		bool VerifyToken(string token);
		Task<UserRecord> CreateUserAsync(UserRecordArgs args);
		Task<string> GenerateEmailVerificationLinkAsync(string email);
	}
	public class FirebaseAuthManager : IFirebaseAuthManager
	{
		private readonly ILogger<FirebaseAuthManager> _logger;
		private readonly FirebaseAuth _firebaseAuth;

		public FirebaseAuthManager(ILoggerFactory loggerFactory, FirebaseAuth firebaseAuth)
		{
			_logger = loggerFactory.CreateLogger<FirebaseAuthManager>();
			_firebaseAuth = firebaseAuth;
		}

		public async Task<UserRecord> CreateUserAsync(UserRecordArgs args)
			=> await _firebaseAuth.CreateUserAsync(args);


		public async Task<string> GenerateEmailVerificationLinkAsync(string email)
			=> await _firebaseAuth.GenerateEmailVerificationLinkAsync(email);

		public bool VerifyToken(string token)
		{
			try
			{
				var result = _firebaseAuth.VerifyIdTokenAsync(token).Result;

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Attempt to verify token failed.");
				return false;
			}
		}
	}
}
