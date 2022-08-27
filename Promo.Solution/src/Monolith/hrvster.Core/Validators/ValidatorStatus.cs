namespace hrvster.Core.Validators
{
	public class ValidatorStatus
	{
		public bool HasError { get; private set; }
		public string Message { get; private set; }

		public static ValidatorStatus CreateSuccessStatus()
			=> new ValidatorStatus { HasError = false, Message = string.Empty };

		public static ValidatorStatus CreateFailureStatus(string message)
			=> new ValidatorStatus { HasError = true, Message = message };
	}
}
