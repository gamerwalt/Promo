namespace hrvster.ApplicationServices.Exceptions
{
	public class NoMoreBoardForThisGameException : System.Exception
	{
		public NoMoreBoardForThisGameException(string message) : base(message)
		{
		}
	}
}
