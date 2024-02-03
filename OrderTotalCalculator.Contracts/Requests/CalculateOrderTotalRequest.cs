namespace OrderTotalCalculator.Contracts.Requests
{
	/// <summary>
	/// Represents a request object for calculating the total amount of an order.
	/// </summary>
	public class CalculateOrderTotalRequest
	{
		public required String State { get; init; } // state code, not object state
		public required decimal OrderSubtotal { get; init; }
		public required decimal PercentDiscount { get; init; }
	}
}
