namespace OrderTotalCalculator.Contracts.Responses
{
	/// <summary>
	/// Represents a validation response containing details about a validation error.
	/// </summary>
	public class ValidationResponse
	{
		public required string PropertyName { get; init; }
		public required string Message { get; init; }
	}
}
