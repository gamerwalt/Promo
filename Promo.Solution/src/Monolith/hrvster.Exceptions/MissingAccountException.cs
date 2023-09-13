namespace hrvster.Exceptions
{
	public class MissingAccountException : Exception
	{
		public MissingAccountException() { }

		public MissingAccountException(string message)
			: base(message) { }
	}
}
