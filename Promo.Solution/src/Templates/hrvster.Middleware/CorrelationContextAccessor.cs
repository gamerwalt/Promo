namespace hrvster.Templates.Middleware
{
	public interface ICorrelationContextAccessor
	{
		Guid? CorrelationId { get; set; }
	}

	public class CorrelationContextAccessor : ICorrelationContextAccessor
	{
		public Guid? CorrelationId { get; set; }
	}
}
