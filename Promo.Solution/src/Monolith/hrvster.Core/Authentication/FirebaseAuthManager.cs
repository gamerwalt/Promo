using FirebaseAdmin.Auth;
using System;

namespace hrvster.Core.Authentication
{
	public interface IFirebaseAuthManager
	{
		bool VerifyToken(string token);
	}
	public class FirebaseAuthManager : IFirebaseAuthManager
	{
		private readonly FirebaseAuth _firebaseAuth;
		public FirebaseAuthManager(FirebaseAuth firebaseAuth)
		{
			_firebaseAuth = firebaseAuth;
		}

		public bool VerifyToken(string token)
		{
			try
			{
				var result = _firebaseAuth.VerifyIdTokenAsync(token).Result;

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
