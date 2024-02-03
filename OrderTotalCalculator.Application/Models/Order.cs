namespace OrderTotalCalculator.Application.Models
{
	/// <summary>
	/// Represents an order entity in the system.
	/// </summary>
	public class Order
	{
		// properties for request
		public required String State { get; init; }
		public required decimal OrderSubtotal { get; init; }
		public required decimal PercentDiscount { get; init; }

		// properties for response
		public decimal OrderTotal { get; set; }
		public decimal TotalSalesTax { get; set; }
		public decimal DiscountedOrderTotalMinusSalesTax { get; set; }
		public decimal TotalDiscountAmount { get; set; }
	}
}
