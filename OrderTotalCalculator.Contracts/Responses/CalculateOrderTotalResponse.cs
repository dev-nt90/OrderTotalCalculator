namespace OrderTotalCalculator.Contracts.Responses
{
	/// <summary>
	/// Represents a response object containing the calculated total amount and related details for an order.
	/// </summary>
	public class CalculateOrderTotalResponse
	{
		public required decimal OrderTotal { get; init; }
		public required decimal TotalSalesTax { get; init; }
		public required decimal DiscountedOrderTotalMinusSalesTax { get; init; }
		public required decimal TotalDiscountAmount { get; init; }
	}
}
