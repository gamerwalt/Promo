namespace hrvster.Exceptions
{
	public class AccountIsNotSubscribedException : Exception
	{
		public AccountIsNotSubscribedException() { }

		public AccountIsNotSubscribedException(string message)
			: base(message) { }
	}
}
