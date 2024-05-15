namespace hrvster.ApplicationServices.Exceptions
{
	public class BoardDoesNotExistException : System.Exception
	{
		public BoardDoesNotExistException(string message) : base(message)
		{
		}
	}
}
